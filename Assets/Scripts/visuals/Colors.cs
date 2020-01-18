using UnityEngine;
using crass;

public class Colors : Singleton<Colors>
{
    public Color Neutral, Excellent, Good, Ok, Ambiguous, Bad;

    void Awake ()
    {
        if (SingletonGetInstance() != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SingletonSetInstance(this, false);
        }
    }
}
