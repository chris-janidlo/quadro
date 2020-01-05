using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCountVisuals : MonoBehaviour
{
    public ADriver Driver;
    public TextMeshProUGUI Text;

    void Update ()
    {
        Text.text = Driver.State.Rhythm.ComboCounter.ToString();
    }
}
