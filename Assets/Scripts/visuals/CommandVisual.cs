using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class CommandVisual : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public Color ActiveColor, InactiveColor;
    public TransitionableColor VisibleColorLerp;

    public Image Border;
    public TextMeshProUGUI NameText;

    Command command => Driver.Player.Commands[dir];

    CommandInput dir;

    public void Initialize (CommandInput dir)
    {
        this.dir = dir;

        NameText.text = command.Name;

        VisibleColorLerp.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        VisibleColorLerp.StartTransitionToIfNotAlreadyStarted(Driver.Player.CanComboInto(dir) ? ActiveColor : InactiveColor);

        Border.color = VisibleColorLerp.Value;
        NameText.color = VisibleColorLerp.Value;
    }
}
