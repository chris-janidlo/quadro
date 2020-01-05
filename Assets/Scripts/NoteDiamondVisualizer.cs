using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class NoteDiamondVisualizer : MonoBehaviour
{
    [Serializable]
    public class ImageBox : InputDirectionBox<Image> {}

    public ADriver Driver;
    public ImageBox Images;

    void Start ()
    {
        foreach (var dir in EnumUtil.AllValues<InputDirection>())
        {
            Images[dir].color = Driver.State.NoteDiamond[dir].Color;
        }
    }

    void Update ()
    {
        foreach (var dir in EnumUtil.AllValues<InputDirection>())
        {
            Images[dir].SetA(Driver.State.CanComboInto(dir) ? 1 : .3f);
        }
    }
}
