using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

// TODO: text popup that shows actual number change
public class HealthBar : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public TransitionableFloat VisibleHealth;

    public LayoutGroup SegmentGroup;
    public Image Bar, HiderSegmentPrefab;

    void Start ()
    {
        VisibleHealth.AttachMonoBehaviour(this);

        for (int i = 0; i < Driver.Player.Health.Max; i++)
        {
            Instantiate(HiderSegmentPrefab, SegmentGroup.transform);
        }
    }

    void Update ()
    {
        VisibleHealth.StartTransitionToIfNotAlreadyStarted(Driver.Player.Health.Value);
        Bar.fillAmount = VisibleHealth.Value / Driver.Player.Health.Max;
    }
}
