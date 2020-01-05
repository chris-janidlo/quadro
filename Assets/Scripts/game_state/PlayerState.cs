﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public readonly Rhythm Rhythm = new Rhythm();
    public readonly NoteDiamond NoteDiamond;

    public Spell CurrentSpell { get; private set; }

    public Track Track => Rhythm.Track;

    bool justFailed;

    public PlayerState (NoteDiamond noteDiamond)
    {
        NoteDiamond = noteDiamond;
    }

    public void DoNoteInput (NoteInput input)
    {
        if (!Rhythm.TryHitNow())
        {
            justFailed = true;
            return;
        }

        justFailed = false;

        if (input != NoteInput.Cast)
        {
            playDirection((InputDirection) input);
        }
        else if (Rhythm.IsDownbeat())
        {
            castSpell();
        }
        else
        {
            justFailed = true;
            Rhythm.FailCombo();
        }
    }

    public bool CanComboInto (InputDirection direction)
    {
        return CurrentSpell == null || CurrentSpell.CanComboInto(direction);
    }

    void playDirection (InputDirection direction)
    {
        Note next = NoteDiamond[direction];

        if (CurrentSpell == null || justFailed) // never played a note before / just casted a spell / just failed a spell
        {
            CurrentSpell = new Spell(next);
        }
        else if (CurrentSpell.CanComboInto(direction))
        {
            CurrentSpell = Rhythm.ComboCounter == 0 ? new Spell(next) : CurrentSpell.PlusMetaNote(next);
        }
        else
        {
            Rhythm.FailCombo();
        }
    }

    void castSpell ()
    {
        if (CurrentSpell != null)
        {
            CurrentSpell.CastOn(Track);
            CurrentSpell = null;
        }
        else
        {
            justFailed = true;
            Rhythm.FailCombo();
        }
    }
}
