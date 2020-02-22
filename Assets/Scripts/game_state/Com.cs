using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class Com
{
    public static Com FromTypeName (string name)
    {
        // TODO: not sure if this is right
        // TODO: restrict coms to a specific namespace
        return (Com) Activator.CreateInstance(null, name).Unwrap();
    }

    protected struct ComData
    {
        public string Name, Description;

        // helps to have a color for consistent visual language
        public Color Color;

        // the coms that can follow this
        public ComInputBools ComboData;
    }

    protected abstract ComData data { get; }

    public string Name => data.Name;
    public string Description => data.Description;
    public Color Color => data.Color;
    public ComInputBools ComboData => data.ComboData;

    public abstract void DoEffect (CPU currentCPU);
}
