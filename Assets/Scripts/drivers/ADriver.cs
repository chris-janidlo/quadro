using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public abstract class ADriver : MonoBehaviour
{
    public SignalJammer SignalJammer;

    public ComInputStrings ComKeys;
    public CPUSwitchInputStrings CPUSwitchKeys;
    public string CPUExecuteKey;

    public Player Player { get; private set; }

    protected bool initialized;

    public virtual void Initialize ()
    {
        Player = new Player(SignalJammer);
        initialized = true;
    }

    protected InputFrame getInput ()
    {
        return new InputFrame
        (
            EnumUtil.AllValues<ComInput>()
                .Select(ci => (ComInput?) ci)
                .FirstOrDefault(cin => Input.GetKeyDown(ComKeys[cin.Value])),

            EnumUtil.AllValues<CPUSwitchInput>()
                .Select(csi => (CPUSwitchInput?) csi)
                .FirstOrDefault(csin => Input.GetKeyDown(CPUSwitchKeys[csin.Value])),

            Input.GetKeyDown(CPUExecuteKey)
        );
    }
}
