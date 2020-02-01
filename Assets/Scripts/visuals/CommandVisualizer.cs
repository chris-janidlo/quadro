using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public TextMeshProUGUI DescriptionText, ChipText;

    Command command => Driver.Player.Command;

    void Update ()
    {
        DescriptionText.text = $"Command: {(command != null ? command.Description : "<null>")}";
        ChipText.text = command != null
            ? String.Join("", command.AllComs.Select(n => fancyChip(n)))
            : "";
    }

    string fancyChip (Com com)
    {
        return $"<#{ColorUtility.ToHtmlStringRGB(com.Color)}>{com.Direction.ToArrow()}</color>";
    }
}
