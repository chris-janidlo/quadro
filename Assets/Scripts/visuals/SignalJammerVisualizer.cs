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

    public ComVisualBox ComVisuals;

    void Start ()
    {
        foreach (InputDirection dir in EnumUtil.AllValues<InputDirection>())
        {
            ComVisuals[dir].Initialize(dir);
        }
    }
}

[Serializable]
public class ComVisualBox : InputDirectionBox<ComVisual> {}
