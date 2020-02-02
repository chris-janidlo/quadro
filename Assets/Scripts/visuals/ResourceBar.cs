using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public abstract class ResourceBar : MonoBehaviour
{
    protected abstract float currentValue { get; }
    protected abstract float maxValue { get; }

    public TransitionableFloat VisibleValue, SegmentAlphaTemplate;

    public int BarSegments;
    [Range(0, 1)]
    public float MaxSegmentAlpha;
    public string Label;
    public Color LabelColor, BarColor;

    public TextMeshProUGUI LabelDisplay, BarDisplay;

    TransitionableFloat[] segmentAlphas;

    void Start ()
    {
        VisibleValue.AttachMonoBehaviour(this);

        segmentAlphas = new TransitionableFloat[BarSegments];

        for (int i = 0; i < BarSegments; i++)
        {
            segmentAlphas[i] = new TransitionableFloat { Time = SegmentAlphaTemplate.Time, Ease = SegmentAlphaTemplate.Ease };
            segmentAlphas[i].AttachMonoBehaviour(this);
        }
    }

    void Update ()
    {
        VisibleValue.StartTransitionToIfNotAlreadyStarted(currentValue);

        int currentSegment = (int) (BarSegments * VisibleValue.Value / maxValue) - 1;

        for (int i = 0; i < BarSegments; i++)
        {
            segmentAlphas[i].StartTransitionToIfNotAlreadyStarted((i <= currentSegment) ? MaxSegmentAlpha : 0);
        }

        LabelDisplay.text = labelText();
        BarDisplay.text = barText();
    }

    string labelText ()
    {
        return $" {Label} {(int) currentValue}/{(int) maxValue}".WrapInTMProColorTag(LabelColor);
    }

    string barText ()
    {
        StringBuilder sb = new StringBuilder("", BarSegments * 10);

        for (int i = 0; i < BarSegments; i++)
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
