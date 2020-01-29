using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class RhythmCard
{
    public readonly ReadOnlyCollection<NoteSymbol?> BeatValues;

    public NoteSymbol? this[int i] => BeatValues[i];

    static Dictionary<int, RhythmCard> emptyCardCache = new Dictionary<int, RhythmCard>();

    public static RhythmCard EmptyCard (int length)
    {
        if (!emptyCardCache.ContainsKey(length)) emptyCardCache[length] = new RhythmCard(new NoteSymbol?[length]);

        return emptyCardCache[length];
    }

    public RhythmCard (IList<NoteSymbol?> beatValues)
    {
        BeatValues = new ReadOnlyCollection<NoteSymbol?>(beatValues);
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
