using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class SignalJammerVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public CommandVisualBox CommandVisuals;
    public List<CPUVisual> CPUVisuals;

    void Start ()
    {
        foreach (var zone in EnumUtil.AllValues<CommandZone>())
        {
            foreach (var button in EnumUtil.AllValues<CommandButton>())
            {
                CommandVisuals[zone][button].Initialize(zone, button);
            }
        }

        for (int i = 0; i < Player.NUM_CPUS; i++)
        {
            CPUVisuals[i].Initialize(i);
        }
    }
}

[Serializable]
public class CommandVisualBox : CommandBox<CommandVisual, CommandVisualBox.CommandVisualButtons>
{
    [Serializable] public class CommandVisualButtons : CommandBox<CommandVisual, CommandVisualBox.CommandVisualButtons>.Buttons<CommandVisual> {}
}
