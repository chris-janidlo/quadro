using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADriver : MonoBehaviour
{
    public ButtonBox DirectionalKeys;
    public string CastKey;

    public Player Player { get; private set; }

    public virtual void Initialize (CommandDiamond commandDiamond)
    {
        Player = new Player(commandDiamond);

        Player.Hit += hit => Debug.Log(hit.ToString());
    }

    protected CommandInput? getInput ()
    {
        if (Input.GetKeyDown(CastKey))
        {
            return CommandInput.Cast;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Up]))
        {
            return CommandInput.Up;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Left]))
        {
            return CommandInput.Left;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Down]))
        {
            return CommandInput.Down;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Right]))
        {
            return CommandInput.Right;
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class ButtonBox : InputDirectionBox<string> {}
