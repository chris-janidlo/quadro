using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackCapacitySegment : MonoBehaviour
{
    Image image => GetComponentInChildren<Image>();

    public void SetActive (bool value)
    {
        image.enabled = value;
    }

    public void SetColor (Color color)
    {
        image.color = color;
    }
}
