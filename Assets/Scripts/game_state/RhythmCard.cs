using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class RhythmCard
{
    public readonly ReadOnlyCollection<BeatSymbol?> BeatValues;

    public BeatSymbol? this[int i] => BeatValues[i];

    static Dictionary<int, RhythmCard> emptyCardCache = new Dictionary<int, RhythmCard>();

    public static RhythmCard EmptyCard (int length)
    {
        if (!emptyCardCache.ContainsKey(length)) emptyCardCache[length] = new RhythmCard(new BeatSymbol?[length]);

        return emptyCardCache[length];
    }

    public RhythmCard (IList<BeatSymbol?> beatValues)
    {
        BeatValues = new ReadOnlyCollection<BeatSymbol?>(beatValues);
    }

    public override string ToString ()
    {
        return new String(BeatValues.Select(b => b == null ? '_' : b.ToRadixRepresentation()).ToArray());
    }

    public override bool Equals (object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return BeatValues.SequenceEqual(((RhythmCard) obj).BeatValues);
    }
    
    public override int GetHashCode()
    {
        return BeatValues.GetHashCode();
    }
}
