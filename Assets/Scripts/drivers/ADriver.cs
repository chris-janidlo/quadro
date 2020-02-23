using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public abstract class ADriver : MonoBehaviour
{
    public SignalJammer SignalJammer;

    public CommandInputStrings CommandKeys;
    public CPUSwitchInputStrings CPUSwitchKeys;
    public string CPUExecuteKey;

    public Player Player { get; private set; }

    protected bool initialized;

    public virtual void Initialize (int seed)
    {
        Player = new Player(SignalJammer, seed);
        initialized = true;
    }

    protected InputFrame getInput ()
    {
        return new InputFrame
        (
            EnumUtil.AllValues<CommandInput>()
                .Select(ci => (CommandInput?) ci)
                .FirstOrDefault(cin => Input.GetKeyDown(CommandKeys[cin.Value])),

            EnumUtil.AllValues<CPUSwitchInput>()
                .Select(csi => (CPUSwitchInput?) csi)
                .FirstOrDefault(csin => Input.GetKeyDown(CPUSwitchKeys[csin.Value])),

            Input.GetKeyDown(CPUExecuteKey)
        );
    }
}
