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

    public void Initialize (RhythmCard card, bool addBottomLine)
    {
        Text.text = makeText(card, addBottomLine);
    }

    string makeText (RhythmCard card, bool addBottomLine)
    {
        int beats = card.BeatValues.Count;

        string line = makeBeatLineText(false, 0, beats, '-');
        string spacer = makeBeatLineText(false, 0, beats, ' ');

        string output = line;

        for (int i = beats - 1; i >= 0; i--)
        {
            output += "\n" + makeBeatLineText(card[i], i, beats, ' ');

            if (i > 0) output += "\n" + spacer;
        }

        if (addBottomLine) output += "\n" + line;

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
