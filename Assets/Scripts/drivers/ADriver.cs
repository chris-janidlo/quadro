using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public abstract class ADriver : MonoBehaviour
{
    public SignalJammer SignalJammer;

    public Player Player { get; private set; }

    protected bool initialized;

    public virtual void Initialize (int seed)
    {
        Player = new Player(SignalJammer, seed);
        initialized = true;
    }
}
