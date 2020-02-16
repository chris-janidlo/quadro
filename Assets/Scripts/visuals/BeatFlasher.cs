using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatFlasher : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    [Range(0, 1)]
    public float FlashTimeBeats;

    public AnimationCurve LerpSmoother;

    public List<LineRenderer> Lines;

    Color flashColor => Colors.Instance.Good;
    Color baseColor => Color.clear;

	void Update ()
    {
        float positionInBeat = (float) Driver.Player.Track.CurrentPositionInBeat;

        Color color = (positionInBeat <= FlashTimeBeats)
            ? Color.Lerp(flashColor, baseColor, LerpSmoother.Evaluate(positionInBeat / FlashTimeBeats))
            : baseColor;
        
        foreach (LineRenderer line in Lines)
        {
            line.startColor = color;
            line.endColor = color;
        }
    }
}
