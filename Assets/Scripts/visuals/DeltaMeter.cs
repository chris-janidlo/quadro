using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class DeltaMeter : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public TransitionableFloat VisibleDelta, VisibleSpawnRate;
    public TransitionableColor SpawnRateFlasher;

    public VerticalLayoutGroup UpperHalfGroup, LowerHalfGroup;
    public Image UpperHalfFiller, LowerHalfFiller, HiderSegmentPrefab;
    public TextMeshProUGUI SpawnRateDisplay;

    void Start ()
    {
        VisibleDelta.AttachMonoBehaviour(this);
        VisibleSpawnRate.AttachMonoBehaviour(this);
        SpawnRateFlasher.AttachMonoBehaviour(this);

        SpawnRateDisplay.color = Colors.Instance.Neutral;
        Driver.State.Track.HandledMiddleOfMeasure += () =>
        {
            SpawnRateFlasher.Value = Colors.Instance.Bad;
            SpawnRateFlasher.StartTransitionTo(Colors.Instance.Neutral);
        };

        for (int i = 0; i < Track.CARDS_UNTIL_DEAD; i++)
        {
            Instantiate(HiderSegmentPrefab, UpperHalfGroup.transform);
            Instantiate(HiderSegmentPrefab, LowerHalfGroup.transform);
        }
    }

    void Update ()
    {
        VisibleDelta.StartTransitionToIfNotAlreadyStarted(Driver.State.Track.CardDelta);
        updateDeltaVisual();

        VisibleSpawnRate.StartTransitionToIfNotAlreadyStarted(Driver.State.Track.CardSpawnRate);
        SpawnRateDisplay.text = VisibleSpawnRate.Value.ToString("+0.##;-0.##");

        SpawnRateDisplay.color = SpawnRateFlasher.Value;
    }

    void updateDeltaVisual ()
    {
        float value = VisibleDelta.Value;

        if (value <= 0)
        {
            UpperHalfFiller.fillAmount = 0;
            LowerHalfFiller.fillAmount = -value / Track.CARDS_UNTIL_DEAD;
        }

        if (value >= 0)
        {
            LowerHalfFiller.fillAmount = 0;
            UpperHalfFiller.fillAmount = value / Track.CARDS_UNTIL_DEAD;
        }
    }
}
