using UnityEngine;
using crass;

public class Colors : Singleton<Colors>
{
    public Color Neutral, Excellent, Good, Ok, Bad;

    void Awake ()
    {
        if (SingletonGetInstance() != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SingletonSetInstance(this, true);
        }
    }
}
