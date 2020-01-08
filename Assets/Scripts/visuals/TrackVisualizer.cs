using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackVisualizer : MonoBehaviour
{
    public ADriver Driver;
    public Transform MainTrackContainer, PreviewContainer;
    public RhythmCardVisual CardVisualPrefab;

    public int PreviewBeats;

    Track track => Driver.State.Track;

    List<RhythmCard> cardCache = new List<RhythmCard>();

    void Update ()
    {
        if (Driver.State.Rhythm.BeatsUntilNextSpawn <= PreviewBeats && PreviewContainer.childCount == 0)
        {
            Instantiate(CardVisualPrefab, PreviewContainer).Initialize(track.NextToSpawn, true);
        }
        if (Driver.State.Rhythm.BeatsUntilNextSpawn > PreviewBeats && PreviewContainer.childCount != 0)
        {
            Destroy(PreviewContainer.GetChild(0).gameObject);
        }

        if (track.Cards == null || track.Cards.Count == 0)
        {
            if (MainTrackContainer.childCount != 0) destroyCardVisuals();
            return;
        }

        if (!cardCache.SequenceEqual(track.Cards))
        {
            cardCache = new List<RhythmCard>(track.Cards);
            destroyCardVisuals();
            createCardVisuals();
        }

        float height = ((RectTransform) MainTrackContainer.GetChild(0)).rect.height;
        float offsetScale = height / Track.BEATS_PER_MEASURE;

        MainTrackContainer.localPosition = Vector2.down * (float) Driver.State.Rhythm.CurrentPositionInMeasure * offsetScale;
    }

    void destroyCardVisuals ()
    {
        foreach (Transform child in MainTrackContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void createCardVisuals ()
    {
        int upper = cardCache.Count - 1;
		for (int i = upper; i >= 0; i--)
        {
			Instantiate(CardVisualPrefab, MainTrackContainer).Initialize(cardCache[i], false);
        }
    }
}
