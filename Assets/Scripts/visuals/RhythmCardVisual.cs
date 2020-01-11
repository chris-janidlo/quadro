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
        Text.text = makeText(new RhythmCard(new bool[emptyLines]));
    }

    string makeText (RhythmCard card)
    {
        int beats = card.BeatValues.Count;

        string output = makeBeatLineText(false, 0, beats, '-');
        string spacer = makeBeatLineText(false, 0, beats, ' ');

        for (int i = beats - 1; i >= 0; i--)
        {
            output += "\n" + makeBeatLineText(card[i], i, beats, ' ');

            if (i > 0) output += "\n" + spacer;
        }

        return output;
    }

    string makeBeatLineText (bool on, int positionInMeasure, int measureLength, char spacer)
    {
        char[] chars = new char[(measureLength + 1) * horSpace + 1];

        for (int i = 0; i < chars.Length; i++)
        {
            if (on && i == (positionInMeasure + 1) * horSpace)
                chars[i] = positionInMeasure.ToString()[0];
            else
                chars[i] = spacer;
        }

        return new String(chars);
    }
}
