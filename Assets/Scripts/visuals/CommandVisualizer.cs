using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public TextMeshProUGUI Text;

    Command command => Driver.Player.Command;

    void Update ()
    {
        Text.text = $"Command: {(command != null ? command.Description : "<null>")}";
    }
}
