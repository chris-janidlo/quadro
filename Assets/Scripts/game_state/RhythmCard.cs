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

    public RhythmCard (IList<BeatSymbol?> beatValues)
    {
        BeatValues = new ReadOnlyCollection<BeatSymbol?>(beatValues);
    }

    public override string ToString ()
    {
        return new String(BeatValues.Select(b => b == null ? '_' : b.ToRadixRepresentation()).ToArray());
    }
}
