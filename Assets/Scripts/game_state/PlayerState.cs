using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public event Action<HitData> HitAttempted;

    public readonly Rhythm Rhythm = new Rhythm();
    public readonly NoteDiamond NoteDiamond;

    public Spell Spell => Rhythm.ComboCounter > 0 ? innerSpell : null;
    public Track Track => Rhythm.Track;

    Spell innerSpell;

    public PlayerState (NoteDiamond noteDiamond)
    {
        NoteDiamond = noteDiamond;
    }

    public void DoNoteInput (NoteInput input)
    {
        bool comboWasZero = Rhythm.ComboCounter == 0;

        HitData hit = Rhythm.TryHitNow();

        HitAttempted?.Invoke(hit);
        if (!hit.IsSuccessful) return;

        if (input != NoteInput.Cast)
        {
            playDirection((InputDirection) input, comboWasZero);

            if (!innerSpell.LastNote.CanClear((BeatSymbol) Track.CurrentCardAtBeat(Rhythm.ClosestPositionInMeasure)))
                Rhythm.FailCard();
        }
        else if (Rhythm.IsDownbeat())
        {
            castSpell();
        }
        else
        {
            Rhythm.FailComboAndCard();
        }
    }

    public bool CanComboInto (InputDirection direction)
    {
        return innerSpell == null || innerSpell.CanComboInto(direction);
    }

    void playDirection (InputDirection direction, bool thisIsMainNote)
    {
        Note next = NoteDiamond[direction];

        if (innerSpell == null) // never played a note before / just casted a spell
        {
            innerSpell = new Spell(next);
        }
        else if (innerSpell.CanComboInto(direction))
        {
            innerSpell = thisIsMainNote ? new Spell(next) : innerSpell.PlusMetaNote(next);
        }
        else
        {
            Rhythm.FailComboAndCard();
        }
    }

    void castSpell ()
    {
        if (innerSpell != null)
        {
            innerSpell.CastOn(Track);
            innerSpell = null;
        }
        else
        {
            Rhythm.FailComboAndCard();
        }
    }
}
