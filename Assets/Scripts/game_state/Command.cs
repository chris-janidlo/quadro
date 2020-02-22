using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class Command
{
    public static Command FromTypeName (string name)
    {
        // TODO: not sure if this is right
        // TODO: restrict commands to a specific namespace
        return (Command) Activator.CreateInstance(null, name).Unwrap();
    }

    protected struct CommandData
    {
        public string Name, Description;

        // helps to have a color for consistent visual language
        public Color Color;

        // the commands that can follow this
        public CommandInputBools ComboData;
    }

    protected abstract CommandData data { get; }

    public string Name => data.Name;
    public string Description => data.Description;
    public Color Color => data.Color;
    public CommandInputBools ComboData => data.ComboData;

    public abstract void DoEffect (CPU currentCPU);
}
