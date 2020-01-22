using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrackVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public Transform TrackMover, RealCardContainer, PreviewContainer;
    public CanvasGroup PreviewGroup;
    public RhythmCardVisual CardVisualPrefab;

    Track track => Driver.State.Track;

    bool handledPreview = false;

    void Start ()
    {
        track.CardsBatchUpdated += updateVisuals;
    }

    void Update ()
    {
        if (Driver.State.Rhythm.TruncatedPositionInMeasure == 0 && !handledPreview)
        {
            handledPreview = true;

            foreach (Transform child in PreviewContainer) Destroy(child.gameObject);
            
            Instantiate(CardVisualPrefab, PreviewContainer).Initialize(track.NextToSpawn);

            if (track.Cards.Count == 0)
                Instantiate(CardVisualPrefab, PreviewContainer).Initialize(Track.BEATS_PER_MEASURE);
        }
        else if (Driver.State.Rhythm.TruncatedPositionInMeasure != 0)
        {
            handledPreview = false;
        }

        PreviewGroup.alpha = (float) Driver.State.Rhythm.CurrentPositionInMeasure / Track.BEATS_PER_MEASURE;

        RectTransform firstChild = firstCardObject();
        if (firstChild == null) return;

        firstChild.GetComponent<RhythmCardVisual>().Text.color = Driver.State.Track.FailedCurrentCard ? Colors.Instance.Bad : Colors.Instance.Neutral;

        float offsetScale = firstChild.rect.height / Track.BEATS_PER_MEASURE;

        TrackMover.localPosition = Vector2.down * (float) Driver.State.Rhythm.CurrentPositionInMeasure * offsetScale;
    }

    void updateVisuals ()
    {
        foreach (Transform child in RealCardContainer) Destroy(child.gameObject);

        for (int i = track.Cards.Count - 1; i >= 0; i--)
        {
            Instantiate(CardVisualPrefab, RealCardContainer).Initialize(track.Cards[i]);
        }
    }

    RectTransform firstCardObject ()
    {
        RectTransform first (Transform parent) =>
            parent.childCount > 0 ? (RectTransform) parent.GetChild(parent.childCount - 1) : null;

        return first(RealCardContainer) ?? first(PreviewContainer);
    }
}
