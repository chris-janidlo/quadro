using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class CommandVisual : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public TextMeshProUGUI NameText;

    public void Initialize (CommandInput dir)
    {
        NameText.text = Driver.Player.Commands[dir].Name;
    }
}
