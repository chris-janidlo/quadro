using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Signal Jammer", fileName = "NewJammer.asset")]
public class SignalJammer : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public CommandInputStrings CommandClassNames;
    public int MaxHealth, NumCPUs;
    public float ArmorDecayPerBeat;
}
