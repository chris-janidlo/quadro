using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// made assuming 4 beats per measure
public class TestRhythmCardVisual : MonoBehaviour
{
    public float Width;

    public SpriteRenderer One, Two, Three, Four;

    public void Initialize (RhythmCard card)
    {
        One.enabled = card[0];
        Two.enabled = card[1];
        Three.enabled = card[2];
        Four.enabled = card[3];
    }
}
