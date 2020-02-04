using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class ComVisual : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public Color NormalColor, InactiveColor;
    public TransitionableFloat VisibleColorLerp;

    public Image Border;
    public TextMeshProUGUI TitleTextMain, TitleTextMeta, EffectText, ClearsText;

    Com com => Driver.Player.SignalJammer[dir];

    InputDirection dir;

    public void Initialize (InputDirection dir)
    {
        this.dir = dir;

        ClearsText.text = String.Join(" ", com.Symbols.Select(s => s.ToRadixRepresentation()));

        VisibleColorLerp.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        bool showMain = Driver.Player.Command == null;
        bool showAtAll = Driver.Player.CanComboInto(dir);

        EffectText.text = showMain ? com.BaseMainEffectDescription : com.MetaEffectDescription;

        VisibleColorLerp.StartTransitionToIfNotAlreadyStarted(showAtAll ? 1 : 0);

        Color active = Color.Lerp(InactiveColor, NormalColor, VisibleColorLerp.Value);
        Color inactive = Color.Lerp(NormalColor, InactiveColor, VisibleColorLerp.Value);

        TitleTextMain.color = (showAtAll && showMain) ? active : inactive;
        TitleTextMeta.color = (showAtAll && !showMain) ? active : inactive;

        Border.color = active;
        EffectText.color = active;
        ClearsText.color = active;
    }
}
