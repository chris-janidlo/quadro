using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDriver : MonoBehaviour
{
    public AudioSource AudioSource;

    public AudioClip Downbeat, Beat;

    public readonly PlayerState State = new PlayerState(new BaseSingleplayerDiamond());

    void Start ()
    {
        State.Rhythm.Beat += () =>
        {
            Debug.Log("spell: " + prettyPrintSpell());
            AudioSource.PlayOneShot(State.Rhythm.IsDownbeat() ? Downbeat : Beat);
        };
    }

    void Update ()
    {
        State.Rhythm.AudioTime = Time.time;

        if (Input.GetButtonDown("NoteUp"))
        {
            doInput(NoteInput.Up);
        }
        else if (Input.GetButtonDown("NoteLeft"))
        {
            doInput(NoteInput.Left);
        }
        else if (Input.GetButtonDown("NoteDown"))
        {
            doInput(NoteInput.Down);
        }
        else if (Input.GetButtonDown("NoteRight"))
        {
            doInput(NoteInput.Right);
        }
    }

    void doInput (NoteInput input)
    {
        State.DoNoteInput(input);
    }


    string prettyPrintCards ()
    {
        return String.Join("|", State.Track.Cards.Select(c => c.ToString()));
    }

    string prettyPrintSpell ()
    {
        if (State.CurrentSpell == null) return "(none)";
        return String.Join(", ", State.CurrentSpell.AllNotes.Select(n => n.Direction.ToString()[0]));
    }
}
