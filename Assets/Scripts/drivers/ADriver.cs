using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADriver : MonoBehaviour
{
    public ButtonBox DirectionalKeys;
    public string CastKey;

    public Player Player { get; private set; }

    public virtual void Initialize (SignalJammer signalJammer)
    {
        Player = new Player(signalJammer);

        Player.Hit += hit => Debug.Log(hit.ToString());
    }

    protected ComInput? getInput ()
    {
        if (Input.GetKeyDown(CastKey))
        {
            return ComInput.Cast;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Up]))
        {
            return ComInput.Up;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Left]))
        {
            return ComInput.Left;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Down]))
        {
            return ComInput.Down;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Right]))
        {
            return ComInput.Right;
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class ButtonBox : InputDirectionBox<string> {}
