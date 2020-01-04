using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCardVisualizer : MonoBehaviour
{
    public float OffsetScale, CardGap;

    public ADriver Driver;
    public TestRhythmCardVisual RhythmCardVisualPrefab;

    Track track => Driver.State.Track;

    List<RhythmCard> cardCache = new List<RhythmCard>();

    GameObject cardVisualParent;

    // Update is called once per frame
    void Update ()
    {
        if (track.Cards == null || track.Cards.Count == 0) return;

        if (!cardCache.SequenceEqual(track.Cards))
        {
            cardCache = new List<RhythmCard>(track.Cards);
            createCardVisuals();
        }

        cardVisualParent.transform.localPosition = Vector2.left * (float) Driver.State.Rhythm.CurrentPositionWithinMeasure * OffsetScale;
    }

    void createCardVisuals ()
    {
        Destroy(cardVisualParent);

        cardVisualParent = new GameObject();
        cardVisualParent.transform.parent = transform;

		for (int i = 0; i < cardCache.Count; i++)
        {
			TestRhythmCardVisual vis = Instantiate(RhythmCardVisualPrefab);
            vis.Initialize(cardCache[i]);
            vis.transform.parent = cardVisualParent.transform;
            vis.transform.localPosition = Vector2.right * (vis.Width + CardGap) * i;
        }
    }
}
