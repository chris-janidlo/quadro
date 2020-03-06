using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Signal Jammer", fileName = "NewJammer.asset")]
public class SignalJammer : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public CommandStrings CommandClassNames;
    public int MaxHealth;
    public float ArmorDecayPerBeat;
}
