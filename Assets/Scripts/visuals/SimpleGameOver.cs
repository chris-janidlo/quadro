﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleGameOver : MonoBehaviour
{
    public HealthBar HealthBar;

    void Update ()
    {
        if (HealthBar.VisibleHealth.Value == 0)
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
