using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDriver : MonoBehaviour
{
    public AudioSource AudioSource;

    public AudioClip Downbeat, Beat;

    PlayerState state = new PlayerState(new BaseSingleplayerDiamond());

    void Start ()
    {
        state.Rhythm.Beat += () =>
        {
            Debug.Log("state:");
            Debug.Log("cards: " + prettyPrintCards());
            Debug.Log("spell: " + prettyPrintSpell());
            AudioSource.PlayOneShot(state.Rhythm.IsDownbeat() ? Downbeat : Beat);
        };
    }

    void Update ()
    {
        state.Rhythm.AudioTime = Time.time;


        if (Input.GetButtonDown("NoteUp"))
        {
            doInput(NoteInput.Down);
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
        state.DoNoteInput(input);
    }


    string prettyPrintCards ()
    {
        return String.Join("|", state.Track.Cards.Select(c => c.ToString()));
    }

    string prettyPrintSpell ()
    {
        if (state.CurrentSpell == null) return "(none)";
        return String.Join(", ", state.CurrentSpell.AllNotes.Select(n => nameof(n.Direction)[0]));
    }
}
