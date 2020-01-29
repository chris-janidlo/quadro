using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrackVisualizer : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public Transform TrackMover, CardContainer;
    public RhythmCardVisual CardVisualPrefab;

    Track track => Driver.Player.Track;

    RhythmCardVisual currentCardVisual, previewCardVisual;

    void Start ()
    {
        previewCardVisual = Instantiate(CardVisualPrefab, CardContainer);
        previewCardVisual.SetCard(RhythmCard.EmptyCard(Track.BEATS_PER_MEASURE));

        currentCardVisual = Instantiate(CardVisualPrefab, CardContainer);
        currentCardVisual.SetCard(RhythmCard.EmptyCard(Track.BEATS_PER_MEASURE));
    }

    void Update ()
    {
        updateCardVisuals();
        moveTrack();

        currentCardVisual.Text.color = track.FailedCurrentCard ? Colors.Instance.Bad : Colors.Instance.Neutral;
    }

    void updateCardVisuals ()
    {
        int cardCount = track.Cards.Count;
        bool cardAboutToSpawn = track.CardDelta >= 1;

        RhythmCard previewCard;
        if (cardCount > 1)
        {
            previewCard = track.Cards[1];
        }
        else if (cardAboutToSpawn)
        {
            previewCard = track.NextToSpawn;
        }
        else if (track.FailedCurrentCard && track.Cards.Count != 0)
        {
            previewCard = track.Cards[0];
        }
        else
        {
            previewCard = RhythmCard.EmptyCard(Track.BEATS_PER_MEASURE);
        }
        
        RhythmCard currentCard = cardCount != 0
            ? track.Cards[0]
            : RhythmCard.EmptyCard(Track.BEATS_PER_MEASURE);

        previewCardVisual.SetCard(previewCard);
        currentCardVisual.SetCard(currentCard);
    }

    void moveTrack ()
    {
        float offsetScale = currentCardVisual.GetComponent<RectTransform>().rect.height / Track.BEATS_PER_MEASURE;
        TrackMover.localPosition = Vector2.down * (float) Driver.Player.Rhythm.CurrentPositionInMeasure * offsetScale;
    }
}
