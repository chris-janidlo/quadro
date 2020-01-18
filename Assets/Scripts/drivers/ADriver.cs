using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADriver : MonoBehaviour
{
    public ButtonBox DirectionalKeys;
    public string CastKey;

    public PlayerState State { get; private set; }

    public virtual void Initialize (NoteDiamond noteDiamond)
    {
        State = new PlayerState(noteDiamond);

        State.Hit += hit => Debug.Log(hit.ToString());
    }

    protected NoteInput? getInput ()
    {
        if (Input.GetKeyDown(CastKey))
        {
            return NoteInput.Cast;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Up]))
        {
            return NoteInput.Up;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Left]))
        {
            return NoteInput.Left;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Down]))
        {
            return NoteInput.Down;
        }
        else if (Input.GetKeyDown(DirectionalKeys[InputDirection.Right]))
        {
            return NoteInput.Right;
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class ButtonBox : InputDirectionBox<string> {}
