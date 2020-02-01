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
                return new Vector2(0.3f, Mathf.Infinity);

            case HitQuality.Ok:
                return new Vector2(0.15f, 0.3f);

            case HitQuality.Good:
                return new Vector2(0.05f, 0.15f);

            case HitQuality.Excellent:
                return new Vector2(0, 0.05f);

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
                return 2;

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

