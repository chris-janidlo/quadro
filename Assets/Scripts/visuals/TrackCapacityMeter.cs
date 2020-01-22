using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackCapacityMeter : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public int WarningDistance, DangerDistance;

    public TrackCapacitySegment SegmentPrefab;

    public HorizontalLayoutGroup MeterGroup;

    RectTransform meterParent => MeterGroup.GetComponent<RectTransform>();

    int index = Track.CARDS_UNTIL_DEAD - 1;

    void Start ()
    {
        for (int i = 0; i < Track.CARDS_UNTIL_DEAD; i++)
        {
            var segment = Instantiate(SegmentPrefab, meterParent);

            if (i <= DangerDistance - 1)
            {
                segment.SetColor(Colors.Instance.Bad);
            }
            else if (i <= WarningDistance - 1)
            {
                segment.SetColor(Colors.Instance.Ok);
            }
            else
            {
                segment.SetColor(Colors.Instance.Neutral);
            }
        }

        Driver.State.Track.CardsBatchUpdated += cardUpdate;
    }

    void cardUpdate ()
    {
        for (int i = 0; i < meterParent.childCount; i++)
        {
            meterParent.GetChild(i).GetComponent<TrackCapacitySegment>().SetActive(i < Driver.State.Track.Cards.Count);
        }
    }
}
