using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleGameOver : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    void Update ()
    {
        if (Driver.Player.Dead)
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
