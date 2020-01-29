using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public TextMeshProUGUI DescriptionText, ChipText;

    Spell spell => Driver.Player.Spell;

    void Update ()
    {
        DescriptionText.text = $"Command: {(spell != null ? spell.Description : "<null>")}";
        ChipText.text = spell != null
            ? String.Join("", spell.AllCommands.Select(n => fancyChip(n)))
            : "";
    }

    string fancyChip (Command command)
    {
        return $"<#{ColorUtility.ToHtmlStringRGB(command.Color)}>{command.Direction.ToArrow()}</color>";
    }
}
