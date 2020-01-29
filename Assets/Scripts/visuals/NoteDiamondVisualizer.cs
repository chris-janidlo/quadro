using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class NoteDiamondVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public TextMeshProUGUI Text;

    void Update ()
    {
        Text.text = "  " + diamondDir(InputDirection.Up) + "\n" + diamondDir(InputDirection.Left) + "  " + diamondDir(InputDirection.Right) + "\n  " + diamondDir(InputDirection.Down);
    }

    string diamondDir (InputDirection dir)
    {
        string rgb = ColorUtility.ToHtmlStringRGB(Driver.Player.NoteDiamond[dir].Color);
        string a = (Driver.Player.CanComboInto(dir) ? "FF" : "66");

        return $"<#{rgb}{a}>{dir.ToArrow()}</color>";
    }
}
