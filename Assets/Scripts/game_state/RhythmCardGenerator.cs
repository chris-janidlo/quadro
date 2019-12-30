using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class RhythmCardGenerator
{
    public readonly int Subdivisions;

    // we don't include all false as a potential value
    public int UniquePermutations => (1 << Subdivisions) - 1;

    BagRandomizer<int> indices;

    public RhythmCardGenerator (int subdivisions)
    {
        Subdivisions = subdivisions;

        indices = new BagRandomizer<int>
        {
            Items = Enumerable.Range(1, UniquePermutations).ToList()
        };
    }

    public RhythmCard GetNextCard ()
    {
        return indexToCard(indices.GetNext());
    }

    RhythmCard indexToCard (int index)
    {
        string paddedBinary = Convert.ToString(index, 2).PadLeft(Subdivisions, '0');
        return new RhythmCard(paddedBinary.Select(c => c == '1').ToArray());
    }
}
