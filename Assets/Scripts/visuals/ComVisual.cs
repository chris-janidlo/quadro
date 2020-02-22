using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class ComVisual : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public Color ActiveColor, InactiveColor;
    public TransitionableColor VisibleColorLerp;

    public Image Border;
    public TextMeshProUGUI NameText;

    Com com => Driver.Player.Coms[dir];

    ComInput dir;

    public void Initialize (ComInput dir)
    {
        this.dir = dir;

        NameText.text = com.Name;

        VisibleColorLerp.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        VisibleColorLerp.StartTransitionToIfNotAlreadyStarted(Driver.Player.CanComboInto(dir) ? ActiveColor : InactiveColor);

        Border.color = VisibleColorLerp.Value;
        NameText.color = VisibleColorLerp.Value;
    }
}
