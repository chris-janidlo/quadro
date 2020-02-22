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

    protected abstract CommandData _data { get; }

    private CommandData? _cachedData;
    protected CommandData cachedData => (_cachedData ?? (_cachedData = _data)).Value;

    public string Name => cachedData.Name;
    public string Description => cachedData.Description;
    public Color Color => cachedData.Color;
    public CommandInputBools ComboData => cachedData.ComboData;

    public abstract void DoEffect (CPU cpu);
}
