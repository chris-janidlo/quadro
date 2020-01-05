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
        Text.text = Driver.State.Spell?.Description ?? "";
    }
}
