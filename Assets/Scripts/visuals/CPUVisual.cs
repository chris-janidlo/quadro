using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class CPUVisual : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public Color ActiveColor, InactiveColor;
    public TransitionableColor VisibleColorLerp;

    public Image Background;
	public TextMeshProUGUI InstructionText;
	public List<TextMeshProUGUI> RegisterTexts;
    public List<TextMeshProUGUI> Labels;

    CPU cpu => Driver.Player.CPUs[index];

    int index;

    public void Initialize (int index)
    {
        this.index = index;

        VisibleColorLerp.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        VisibleColorLerp.StartTransitionToIfNotAlreadyStarted(Driver.Player.CPUIndex == index ? ActiveColor : InactiveColor);

        Background.color = VisibleColorLerp.Value;
		InstructionText.color = VisibleColorLerp.Value;
        foreach (var r in RegisterTexts) r.color = VisibleColorLerp.Value;
        foreach (var l in Labels) l.color = VisibleColorLerp.Value;

		InstructionText.text = cpu.Instr?.Name ?? "";
		for (int i = 0; i < 4; i++) RegisterTexts[i].text = cpu.Registers[i].ToString();
    }
}
