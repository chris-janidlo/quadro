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
        foreach (CommandInput dir in EnumUtil.AllValues<CommandInput>())
        {
            CommandVisuals[dir].Initialize(dir);
        }

        for (int i = 0; i < Driver.Player.CPUs.Count; i++)
        {
            CPUVisuals[i].Initialize(i);
        }
    }
}

[Serializable]
public class CommandVisualBox : CommandInputBox<CommandVisual> {}
