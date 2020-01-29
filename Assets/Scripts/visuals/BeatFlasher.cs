using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatFlasher : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    [Range(0, 1)]
    public float FlashTimeBeats;

    public AnimationCurve LerpSmoother;

    public Graphic Graphic;

    Color flashColor => Colors.Instance.Excellent;
    Color baseColor => Colors.Instance.Neutral;

	void Update ()
    {
        float fractionalPart = (float) Driver.Player.Rhythm.FractionalPartOfPosition;

        if (fractionalPart <= FlashTimeBeats)
        {
            Graphic.color = Color.Lerp(flashColor, baseColor, LerpSmoother.Evaluate(fractionalPart / FlashTimeBeats));
        }
        else
        {
            Graphic.color = baseColor;
        }
    }
}
