using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackCapacityMeter : MonoBehaviour
{
    public int WarningDistance, DangerDistance;

    public TrackCapacitySegment SegmentPrefab;

    public ADriver Driver;
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

        Driver.State.Track.CardAdded += addCard;
        Driver.State.Track.CardRemoved += removeCard;
    }

    void addCard ()
    {
        if (index >= 0)
        {
            setSegmentStateForMostRecentCard(false);
        }

        index--;
    }

    void removeCard ()
    {
        index++;

        if (index >= 0)
        {
            setSegmentStateForMostRecentCard(true);
        }
    }

    void setSegmentStateForMostRecentCard (bool value)
    {
        meterParent.GetChild(index).GetComponent<TrackCapacitySegment>().SetActive(value);
    }
}
