using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellDescriptor : MonoBehaviour
{
    public ADriver Driver;
    public TextMeshProUGUI Text;

    void Update ()
    {
        string text = "";

        if (Driver.State.Rhythm.ComboCounter > 0 && Driver.State.CurrentSpell != null)
        {
            text = Driver.State.CurrentSpell.Description;
        }

        Text.text = text;
    }
}
