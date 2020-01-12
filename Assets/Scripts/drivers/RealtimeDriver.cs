using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// requires that TimingSource has a clip of 4 seconds of silence (one 4/4 measure at 60 BPM) set to loop
// TODO: set up a dictionary or something for other measure lengths (for like 2-7)
public class RealtimeDriver : ADriver
{
    public AudioSource TimingSource, BeatSource;

    public AudioClip Downbeat, Beat;

    float inverseAudioFrequency;

    void Awake ()
    {
        Initialize(new BaseSingleplayerDiamond());
    }

    public override void Initialize (NoteDiamond noteDiamond)
    {
        base.Initialize(noteDiamond);

        inverseAudioFrequency = 1f / TimingSource.clip.frequency;

        State.Rhythm.Beat += () =>
        {
            BeatSource.PlayOneShot(State.Rhythm.IsDownbeat() ? Downbeat : Beat);
            TimingSource.pitch = State.Track.BPM / 60f;
        };
    }

    void Update ()
    {
        State.Rhythm.CurrentPositionInMeasure = TimingSource.timeSamples * inverseAudioFrequency;

        var input = getInput();

        if (input != null) State.DoNoteInput((NoteInput) input);
    }
}
