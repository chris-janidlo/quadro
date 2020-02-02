using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class DangerTunnelVision : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public TransitionableFloat VisibleAlpha;
    public Image LeftVisual, RightVisual;
    public SpriteBag Frames;

    public AnimationCurve HealthToVisualAlpha;
    public int FramesPerSecond;

    float secondsPerFrame => 1.0f / FramesPerSecond;

    float frameTimer;

    void Start ()
    {
        VisibleAlpha.AttachMonoBehaviour(this);
    }

    void Update ()
    {
        VisibleAlpha.StartTransitionToIfNotAlreadyStarted(HealthToVisualAlpha.Evaluate(Driver.Player.Health.Value));

        LeftVisual.SetA(VisibleAlpha.Value);
        RightVisual.SetA(VisibleAlpha.Value);

        if (VisibleAlpha.Value == 0) return;

        frameTimer += Time.deltaTime;

        if (frameTimer > secondsPerFrame)
        {
            LeftVisual.sprite = Frames.GetNext();
            RightVisual.sprite = Frames.GetNext();
            frameTimer = 0;
        }
    }
}

[System.Serializable]
public class SpriteBag : BagRandomizer<Sprite> {}
