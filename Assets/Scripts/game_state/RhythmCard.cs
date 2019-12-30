using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class RhythmCard
{
    public readonly ReadOnlyCollection<bool> BeatValues;

    public RhythmCard (IList<bool> beatValues)
    {
        BeatValues = new ReadOnlyCollection<bool>(beatValues);
    }
}
