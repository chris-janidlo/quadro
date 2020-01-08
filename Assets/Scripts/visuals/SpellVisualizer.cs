using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellVisualizer : MonoBehaviour
{
    public ADriver Driver;
    public TextMeshProUGUI DescriptionText, ChipText;

    Spell spell => Driver.State.Spell;

    void Update ()
    {
        DescriptionText.text = $"Spell: {(spell != null ? spell.Description : "<null>")}";
        ChipText.text = spell != null
            ? String.Join("", spell.AllNotes.Select(n => fancyChip(n)))
            : "";
    }

    string fancyChip (Note note)
    {
        return $"<#{ColorUtility.ToHtmlStringRGB(note.Color)}>{note.Direction.ToArrow()}</color>";
    }
}
