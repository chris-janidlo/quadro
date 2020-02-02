using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

// TODO: text popup that shows actual number change
public class HealthBar : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public TransitionableFloat VisibleHealth, HealthSegmentTemplate;

    public int HealthSegments;
    [Range(0, 1)]
    public float MaxSegmentAlpha;
    public Color LabelColor, BarColor;
    public TextMeshProUGUI LabelDisplay, BarDisplay;

    TransitionableFloat[] segmentAlphas;

    void Start ()
    {
        VisibleHealth.AttachMonoBehaviour(this);

        segmentAlphas = new TransitionableFloat[HealthSegments];

        for (int i = 0; i < HealthSegments; i++)
        {
            segmentAlphas[i] = new TransitionableFloat { Time = HealthSegmentTemplate.Time, Ease = HealthSegmentTemplate.Ease };
            segmentAlphas[i].AttachMonoBehaviour(this);
        }
    }

    void Update ()
    {
        VisibleHealth.StartTransitionToIfNotAlreadyStarted(Driver.Player.Health.Value);

        int currentSegment = (int) (HealthSegments * VisibleHealth.Value / Driver.Player.Health.Max) - 1;

        for (int i = 0; i < HealthSegments; i++)
        {
            segmentAlphas[i].StartTransitionToIfNotAlreadyStarted((i <= currentSegment) ? MaxSegmentAlpha : 0);
        }

        LabelDisplay.text = labelText();
        BarDisplay.text = barText();
    }

    string labelText ()
    {
        return $" Health: {Driver.Player.Health.Value}/{Driver.Player.Health.Max}".WrapInTMProColorTag(LabelColor);
    }

    string barText ()
    {
        StringBuilder sb = new StringBuilder("", HealthSegments * 10);

        for (int i = 0; i < HealthSegments; i++)
        {
            Color highlightColor = new Color
            (
                BarColor.r,
                BarColor.g,
                BarColor.b,
                segmentAlphas[i].Value
            );

            string rgba = ColorUtility.ToHtmlStringRGBA(highlightColor);

            // \u00A0 == non-breaking space, since trailing regular whitespace is ignored
            string highlightedChar = $"<mark=#{rgba}>{'\u00A0'}</mark>";

            sb.Append(highlightedChar);
        }

        return sb.ToString();
    }
}
