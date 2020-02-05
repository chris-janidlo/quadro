using UnityEngine;
using crass;

public class DamageScreenShake : MonoBehaviour, IDriverSubscriber
{
    public ADriver Driver { get; set; }

    public float ShakeTime, ShakeAmount;

    void Start ()
    {
        int health = Driver.Player.Health.Value;

        Driver.Player.Health.ValueDidChange += newHealth =>
        {
            if (newHealth < health) CameraCache.Main.ShakeScreen2D(ShakeTime, ShakeAmount);

            health = newHealth;
        };
    }
}
