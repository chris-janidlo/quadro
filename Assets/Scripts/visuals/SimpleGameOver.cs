using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleGameOver : MonoBehaviour
{
    public ADriver Driver;

    void Update ()
    {
        if (Driver.State.Track.Dead)
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
