using System;
using UnityEngine;

public enum HitQuality
{
    Miss, Ok, Good, Excellent
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

    public static int Healing (this HitQuality quality)
    {
        switch (quality)
        {
            case HitQuality.Miss:
                return 0;
            
            case HitQuality.Ok:
                return 0;

            case HitQuality.Good:
                return 1;
            
            case HitQuality.Excellent:
                return 3;

            default:
                throw new ArgumentException("unexpected HitQuality value " + quality);
        }
    }

    public static Color Color (this HitQuality quality)
    {
        switch (quality)
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
                throw new ArgumentException("unexpected HitQuality value " + quality);
        }
    }
}

