using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class HitData
{
    public readonly double DistanceFromBeat;
    public readonly HitQuality Quality;
    public readonly MissedHitReason? MissReason;

    public bool ClearedBeat => MissReason == null;

    public bool KillsCommand => !ClearedBeat && MissReason != MissedHitReason.ComCantClearAttemptedBeat;

    public HitData (double distanceFromBeat, MissedHitReason? missReason = null)
    {
        if (distanceFromBeat < 0)
            throw new ArgumentException("hit cannot be closer than 0");

        DistanceFromBeat = distanceFromBeat;
        MissReason = missReason;

        if (MissReason != null)
        {
            Quality = HitQuality.Miss;
            return;
        }

        Quality = Enum.GetValues(typeof(HitQuality)).Cast<HitQuality>().Single(hq => {
            var range = hq.BeatDistanceRange();
            return distanceFromBeat >= range.x && distanceFromBeat <= range.y;
        });

        if (Quality == HitQuality.Miss)
        {
            MissReason = MissedHitReason.ClosestBeatOutOfRange;
        }
    }

    private HitData () {}

    public override string ToString ()
    {
        return Quality.ToString() + String.Format(" - ({0})", MissReason == null ? DistanceFromBeat.ToString("0.##") : MissReason.ToString());
    }

    public string ShortDescription ()
    {
        if (MissReason != null &&
            (MissReason.Value == MissedHitReason.ComCantCombo ||
             MissReason.Value == MissedHitReason.InvalidCastInput ||
             MissReason.Value == MissedHitReason.ComCantClearAttemptedBeat))
        {
            return "Invalid";
        }

        return Quality.ToString();
    }

    public HitData WithMissReason (MissedHitReason missReason)
    {
        return new HitData(DistanceFromBeat, missReason);
    }

    public Color Color ()
    {
        switch (Quality)
        {
            case HitQuality.Miss:
                return Colors.Instance.Bad;

            case HitQuality.Ok:
                return Colors.Instance.Ok;

            case HitQuality.Good:
                return Colors.Instance.Good;

            case HitQuality.Excellent:
                return Colors.Instance.Excellent;

            default:
                throw new InvalidOperationException("unexpected HitQuality value " + Quality);
        }
    }
}
