using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// requires that TimingSource has a clip of 4 seconds of silence (one 4/4 measure at 60 BPM) set to loop
public class RealtimeDriver : ADriver
{
    public AudioSource TimingSource, BeatSource;

    public AudioClip Downbeat, Beat;

    float positionScale;

    void Awake ()
    {
        Initialize(new BaseSingleplayerDiamond());
    }

    public override void Initialize (NoteDiamond noteDiamond)
    {
        base.Initialize(noteDiamond);

        var measureScalar = (float) Track.BEATS_PER_MEASURE / 4;
        var timingScale = measureScalar * 60f;

        var inverseAudioFrequency = 1f / TimingSource.clip.frequency;
        positionScale = measureScalar * inverseAudioFrequency;

        State.Rhythm.Beat += () =>
        {
            BeatSource.PlayOneShot(State.Rhythm.IsDownbeat() ? Downbeat : Beat);
            TimingSource.pitch = State.Track.BPM / timingScale;
        };
    }

    void Update ()
    {
        State.Rhythm.CurrentPositionInMeasure = TimingSource.timeSamples * positionScale;

        var input = getInput();

        if (input != null) State.DoNoteInput(input.Value);
    }
}
