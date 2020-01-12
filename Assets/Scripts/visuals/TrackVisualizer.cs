using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrackVisualizer : MonoBehaviour
{
    public ADriver Driver;
    public Transform TrackMover, RealCardContainer, PreviewContainer;
    public CanvasGroup PreviewGroup;
    public RhythmCardVisual CardVisualPrefab;

    Track track => Driver.State.Track;

    bool handledPreview = false;

    void Start ()
    {
        track.CardAdded += addCard;
        track.CardRemoved += removeCard;
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

        float offsetScale = firstChild.rect.height / Track.BEATS_PER_MEASURE;

        TrackMover.localPosition = Vector2.down * (float) Driver.State.Rhythm.CurrentPositionInMeasure * offsetScale;
    }

    void addCard ()
    {
        var card = Instantiate(CardVisualPrefab, RealCardContainer);
        card.Initialize(track.Cards[track.Cards.Count - 1]);
        card.gameObject.transform.SetAsFirstSibling();
    }

    void removeCard ()
    {
        Destroy(RealCardContainer.GetChild(RealCardContainer.childCount - 1).gameObject);
    }

    RectTransform firstCardObject ()
    {
        RectTransform first (Transform parent) =>
            parent.childCount > 0 ? (RectTransform) parent.GetChild(parent.childCount - 1) : null;

        return first(RealCardContainer) ?? first(PreviewContainer);
    }
}
