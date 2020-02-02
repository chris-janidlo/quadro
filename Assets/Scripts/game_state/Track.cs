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
    public event Action<HitData> DidntAttemptBeat;
    public event Action<Note> NoteSpawned, NoteDespawned;

    public const int BEATS_PER_MEASURE = 4;
    public const int NOTES_PER_DIFFICULTY_INCREASE = 24;
    public const int BEATS_SHOWN_IN_ADVANCE = 8;
    public const int BPM_PER_BSTEP = 10;

    public readonly BoxedInt BSteps = new BoxedInt(8, 4, 20);
    public readonly BoxedInt RhythmDifficulty = new BoxedInt(4, 1, 16);

    public double Latency; // TODO: use this

    // needs to be updated by external driver
    double _beatPos;
    public double CurrentPositionInMeasure
    {
        get => _beatPos;
        set
        {
            if (value < 0 || value >= Track.BEATS_PER_MEASURE)
            {
                throw new ArgumentException("value must be between 0 and " + Track.BEATS_PER_MEASURE);
            }

            _beatPos = value;
            audioTimeDidUpdate();
        }
    }

    public int BPM => (int) (actualBSteps * BPM_PER_BSTEP);

    public int TruncatedPositionInMeasure => (int) CurrentPositionInMeasure;
    public double FractionalPartOfPosition => CurrentPositionInMeasure - TruncatedPositionInMeasure;

    int closestBeatPosition => (int) Math.Round(CurrentPositionInMeasure);

    List<Note> notes = new List<Note>();
    RhythmGenerator generator = new RhythmGenerator(BEATS_PER_MEASURE);

    int beatTicker = -1, emptyBeatSpawnTicker;
    Note previousHittableNote;
    bool closestHittableNoteAttempted;
    double actualBSteps = 8;

    public HitData GetHitByAccuracy ()
    {
        HitData hit = null;
        double hitDistance = Math.Abs(CurrentPositionInMeasure - closestBeatPosition);

        if (closestHittableNoteAttempted)
            hit = new HitData(hitDistance, MissedHitReason.AlreadyAttemptedBeat);

        closestHittableNoteAttempted = true;

        if (hit == null && ClosestHittableNote() == null)
            hit = new HitData(hitDistance, MissedHitReason.ClosestBeatIsOff);

        if (hit == null)
            hit = new HitData(hitDistance);

        return hit;
    }

    // closest note that is not a miss
    public Note ClosestHittableNote ()
    {
        Note closest = null;
        double currentDistance = double.MaxValue;

        foreach (Note note in notes)
        {
            if (note.BeatTicker > 1) continue;

            double distance = Math.Abs(FractionalPartOfPosition - note.BeatsUntilThisNote);

            if (distance <= HitQuality.Miss.BeatDistanceRange().x && distance < currentDistance)
            {
                closest = note;
                currentDistance = distance;
            }
        }

        return closest;
    }

    void audioTimeDidUpdate ()
    {
        if (TruncatedPositionInMeasure != beatTicker)
        {
            if (CurrentPositionInMeasure >= beatTicker + 1) beatTicker++; // tick
            else if (TruncatedPositionInMeasure == 0) beatTicker = 0; // loop
            else throw new InvalidOperationException("something fucky with beats");

            Beat?.Invoke();

            tickNotes();
            spawnNotesForNextBeat();
        }

        actualBSteps = Mathf.Lerp((float) actualBSteps, BSteps.Value, EasingFunction.EaseInQuint(0, 1, (float) FractionalPartOfPosition));

        if (previousHittableNote != ClosestHittableNote())
        {
            // we have entered this if because one of the following is true:
                // the previous note is out of hittable range
                // a new note is closer than the previous note, even though they're closer to each other than 2*hittable range
            // in either case, in the current naive implementation, we now know whether or not the player ever attempted the previous note.
        
            // if the player completely skipped this beat when they shouldn't have, fail
            if (previousHittableNote != null && !closestHittableNoteAttempted)
            {
                DidntAttemptBeat?.Invoke(new HitData(FractionalPartOfPosition, MissedHitReason.NeverAttemptedBeat));
            }

            closestHittableNoteAttempted = false;
        }

        previousHittableNote = ClosestHittableNote();
    }

    void spawnNotesForNextBeat ()
    {
        List<Note> newNotes = generator.GetNotesForNextBeat();

        foreach (Note note in newNotes)
        {
            note.BeatTicker = BEATS_SHOWN_IN_ADVANCE;
            notes.Add(note);
            NoteSpawned?.Invoke(note);
        }
    }

    void tickNotes ()
    {
		for (int i = notes.Count - 1; i >= 0; i--)
        {
			Note note = notes[i];

            note.BeatTicker--;

            if (note.BeatTicker <= -2)
            {
                notes.RemoveAt(i);
                NoteDespawned?.Invoke(note);
            }
        }
    }
}
