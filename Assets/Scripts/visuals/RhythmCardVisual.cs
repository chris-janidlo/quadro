using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RhythmCardVisual : MonoBehaviour
{
    public TextMeshProUGUI Text;

    const int horSpace = 4;

    public void Initialize (RhythmCard card)
    {
        Text.text = makeText(card);
    }

    public void Initialize (int emptyLines)
    {
        Text.text = makeText(new RhythmCard(new BeatSymbol?[emptyLines]));
    }

    string makeText (RhythmCard card)
    {
        int beats = card.BeatValues.Count;

        string output = makeBeatLineText(null, 0, beats, '-');
        string spacer = makeBeatLineText(null, 0, beats, ' ');

        for (int i = beats - 1; i >= 0; i--)
        {
            output += "\n" + makeBeatLineText(card[i], i, beats, ' ');

            if (i > 0) output += "\n" + spacer;
        }

        return output;
    }

    string makeBeatLineText (BeatSymbol? symbol, int positionInMeasure, int measureLength, char spacer)
    {
        char[] chars = new char[(measureLength + 1) * horSpace + 1];

        for (int i = 0; i < chars.Length; i++)
        {
            if (symbol != null && i == (positionInMeasure + 1) * horSpace)
                chars[i] = symbol.ToRadixRepresentation();
            else
                chars[i] = spacer;
        }

        return new String(chars);
    }
}
