using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class HitData
{
    public readonly double DistanceFromBeat;
    public readonly HitQuality Quality;
    public readonly BadHitReason? BadHitReason;

    public bool IsSuccessful => BadHitReason == null && Quality != HitQuality.Miss;

    public HitData (double distanceFromBeat, BadHitReason? badHitReason = null)
    {
        if (distanceFromBeat < 0)
            throw new ArgumentException("hit cannot be closer than 0");

        DistanceFromBeat = distanceFromBeat;
        BadHitReason = badHitReason;

        if (BadHitReason != null)
        {
            Quality = HitQuality.Miss;
            return;
        }

        Quality = Enum.GetValues(typeof(HitQuality)).Cast<HitQuality>().Single(hq => {
            var range = hq.BeatDistanceRange();
            return distanceFromBeat > range.x && distanceFromBeat <= range.y;
        });
    }

    private HitData () {}

    public override string ToString ()
    {
        return Quality.ToString() + String.Format(" - ({0})", BadHitReason == null ? DistanceFromBeat.ToString("0.##") : BadHitReason.ToString());
    }
}

public enum HitQuality
{
    Miss, Ok, Good, Excellent
}

public enum BadHitReason
{
    AlreadyAttemptedBeat, BeatIsOff
}

public static class HitQualityExtensions
{
    public static Vector2 BeatDistanceRange (this HitQuality quality)
    {
        switch (quality)
        {
            case HitQuality.Miss:
                return new Vector2(0.4f, Mathf.Infinity);

            case HitQuality.Ok:
                return new Vector2(0.25f, 0.4f);

            case HitQuality.Good:
                return new Vector2(0.1f, 0.25f);

            case HitQuality.Excellent:
                return new Vector2(0, 0.1f);

            default:
                throw new ArgumentException("unexpected HitQuality value " + quality);
        }
    }
}
