using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackVisualizer : MonoBehaviour
{
    public ADriver Driver;
    public Transform RealCardContainer, PreviewContainer;
    public RhythmCardVisual CardVisualPrefab;

    public int PreviewBeats;

    Track track => Driver.State.Track;

    RectTransform firstChild => firstRect(RealCardContainer) ?? firstRect(PreviewContainer);

    void Start ()
    {
        track.CardAdded += addCard;
        track.CardRemoved += removeCard;
    }

    void Update ()
    {
        if (Driver.State.Rhythm.BeatsUntilNextSpawn <= PreviewBeats && PreviewContainer.childCount == 0)
        {
            Instantiate(CardVisualPrefab, PreviewContainer).Initialize(track.NextToSpawn);
        }
        if (Driver.State.Rhythm.BeatsUntilNextSpawn > PreviewBeats && PreviewContainer.childCount != 0)
        {
            foreach (Transform child in PreviewContainer) Destroy(child.gameObject);
        }

        if (firstChild == null) return;

        float offsetScale = firstChild.rect.height / Track.BEATS_PER_MEASURE;

        RealCardContainer.localPosition = Vector2.down * (float) Driver.State.Rhythm.CurrentPositionInMeasure * offsetScale;
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

    RectTransform firstRect (Transform parent) =>
        parent.childCount > 0 ? (RectTransform) parent.GetChild(0) : null;
}
