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
        float fractionalPart = (float) Driver.Player.Track.FractionalPartOfPosition;

        Color color = (fractionalPart <= FlashTimeBeats)
            ? Color.Lerp(flashColor, baseColor, LerpSmoother.Evaluate(fractionalPart / FlashTimeBeats))
            : baseColor;
        
        foreach (LineRenderer line in Lines)
        {
            line.startColor = color;
            line.endColor = color;
        }
    }
}
