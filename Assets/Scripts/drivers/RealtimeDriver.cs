using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// requires that TimingSource has a clip of 4 seconds of silence (one 4/4 measure at 60 BPM) set to loop
public class RealtimeDriver : ADriver
{
    public AudioSource TimingSource;

    public int TimingClipBPM, TimingClipMeasureLength;

    float inverseAudioFrequency;

    void Awake ()
    {
        Initialize(new BaseSingleplayerDiamond());
    }

    public override void Initialize (ComDiamond comDiamond)
    {
        base.Initialize(comDiamond);
    }

    void Update ()
    {
        Player.Track.CurrentPositionInMeasure = (float) TimingSource.timeSamples * TimingClipBPM / 60 / TimingSource.clip.frequency;
        TimingSource.pitch = (float) Player.Track.BPM / TimingClipBPM * 4 / Track.BEATS_PER_MEASURE;

        var input = getInput();

        if (input != null) Player.DoComInput(input.Value);
    }
}
