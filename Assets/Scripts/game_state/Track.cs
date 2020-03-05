using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using crass;

// TODO: if the closest note is attempted but there's another note within hit range, and the player attempts a hit, do we count the second hit?
// TODO: say there are two notes a and b. b is closer to the current position in measure, and neither have ever been attempted, but a is still in hit range. if the player attempts a hit, should that count for a or b?
public class Track
{
	public event Action Beat;
    public event Action<Note> NoteSpawned, NoteDespawned;

    public const int BEATS_PER_MEASURE = 4;
    public const int BEATS_SHOWN_IN_ADVANCE = 8;
    public const int BPM_PER_BSTEP = 10;

    static readonly IReadOnlyList<int> LEGAL_SUBDIVISIONS = new List<int> { 3, 4 };

    // never get the current BPM from this value; always get it from BPM property below
    public readonly BoxedInt BSteps = new BoxedInt(8, 4, 20);
    public readonly BoxedInt RhythmDifficulty = new BoxedInt(7, RhythmGenerator.MIN_DIFFICULTY, RhythmGenerator.MAX_DIFFICULTY);

    public double Latency; // TODO: use this

    // needs to be updated by external driver
    double _beatPos;
    public double CurrentPositionInMeasure
    {
        get => _beatPos;
        set
        {
            if (value < 0 || value >= BEATS_PER_MEASURE)
            {
                throw new ArgumentException($"value must be in range [0, {BEATS_PER_MEASURE}); was given ${value}");
            }

            _beatPos = value;
            audioTimeDidUpdate();
        }
    }

    public int BPM => (int) (apparentBSteps * BPM_PER_BSTEP);

    public int CurrentBeatPosition => (int) CurrentPositionInMeasure;
    public double CurrentPositionInBeat => CurrentPositionInMeasure - CurrentBeatPosition;

    int closestBeatPosition => (int) Math.Round(CurrentPositionInMeasure);

    List<Note> notes = new List<Note>();
    RhythmGenerator generator;

    List<int> currentlyLegalSubdivisions;

    int beatTicker = -1, emptyBeatSpawnTicker;

    double previousHittablePositionInBeat;
    bool closestHittableNoteAttempted;

    // the actual value that is currently in play, which lags behind BSteps a bit (based on an easing function) in order to make the BPM change not so sudden
    double apparentBSteps;

    public Track (int seed)
    {
        generator = new RhythmGenerator(this, BEATS_PER_MEASURE, seed);

        apparentBSteps = BSteps.Value;
        currentlyLegalSubdivisions = new List<int>(LEGAL_SUBDIVISIONS);
    }

    public HitData TryHitNow ()
    {
        double closestTargetTime = closestTargetablePositionInBeat();

        HitData hit = new HitData(Math.Abs(CurrentPositionInBeat - closestTargetTime));

        if (closestHittableNoteAttempted) hit = hit.WithMissReason(MissedHitReason.AlreadyAttemptedBeat);
        closestHittableNoteAttempted = true;

        if (!hit.ClearedBeat)
        {
            currentlyLegalSubdivisions.Clear();
        }
        else if (closestTargetTime % 1 != 0)
        {
            currentlyLegalSubdivisions.RemoveAll(s => closestTargetTime % ((double) 1 / s) != 0);
        }

        return hit;
    }

    public NoteSymbol CurrentChord ()
    {
        return notes.Where(n => n.BeatsUntilThisNote >= -1).OrderBy(n => n.BeatsUntilThisNote).First().Symbol;
    }

    double closestTargetablePositionInBeat ()
    {
        List<double> targetablePositions = new List<double> { 0, 1 };

        foreach (int subdivision in currentlyLegalSubdivisions)
        {
            double smallestDuration = (double) 1 / subdivision;
            for (int i = 0; i < subdivision; i++) targetablePositions.Add(smallestDuration * i);
        }

        return targetablePositions
            .OrderBy(p => Math.Abs(CurrentPositionInBeat - p))
            .First();
    }

    void audioTimeDidUpdate ()
    {
        if (CurrentBeatPosition != beatTicker)
        {
            if (CurrentPositionInMeasure >= beatTicker + 1) beatTicker++; // tick
            else if (CurrentBeatPosition == 0) beatTicker = 0; // loop
            else throw new InvalidOperationException("something fucky with beats");

            Beat?.Invoke();

            clearStaleNotes();
            spawnNotesForNextBeat();

            currentlyLegalSubdivisions.Clear();
            currentlyLegalSubdivisions.AddRange(LEGAL_SUBDIVISIONS);
        }

        if (apparentBSteps != BSteps.Value)
        {
            apparentBSteps = Mathf.Lerp((float) apparentBSteps, BSteps.Value, EasingFunction.EaseInQuint(0, 1, (float) CurrentPositionInBeat));
        }

        double closestHittablePositionInBeat = closestTargetablePositionInBeat();

        if (closestHittablePositionInBeat != previousHittablePositionInBeat)
        {
            if (!(closestHittablePositionInBeat == 0 && previousHittablePositionInBeat == 1))
                closestHittableNoteAttempted = false;
            previousHittablePositionInBeat = closestHittablePositionInBeat;
        }
    }

    void spawnNotesForNextBeat ()
    {
        NoteChunk nextBeat = generator.GetNextBeat(CurrentBeatPosition, RhythmDifficulty.Value);

        foreach (Note note in nextBeat.Values)
        {
            notes.Add(note);
            NoteSpawned?.Invoke(note);
        }
    }

    void clearStaleNotes ()
    {
		for (int i = notes.Count - 1; i >= 0; i--)
        {
			Note note = notes[i];

            if (note.BeatsUntilThisNote <= -1)
            {
                notes.RemoveAt(i);
                NoteDespawned?.Invoke(note);
            }
        }
    }
}
