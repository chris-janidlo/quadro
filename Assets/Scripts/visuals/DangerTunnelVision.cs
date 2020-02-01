using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class DangerTunnelVision : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public Image LeftVisual, RightVisual;
    public SpriteBag Frames;

    public AnimationCurve HealthToVisualAlpha;
    public int FramesPerSecond;

    float secondsPerFrame => 1.0f / FramesPerSecond;

    float frameTimer;

    void Update ()
    {
        float alpha = HealthToVisualAlpha.Evaluate(Driver.Player.Health.Value);

        LeftVisual.SetA(alpha);
        RightVisual.SetA(alpha);

        if (alpha == 0) return;

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
