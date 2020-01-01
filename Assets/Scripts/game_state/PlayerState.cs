using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public readonly Rhythm Rhythm = new Rhythm();
    public readonly NoteDiamond NoteDiamond;

    public Spell CurrentSpell { get; private set; }

    public Track Track => Rhythm.Track;


    public PlayerState (NoteDiamond noteDiamond)
    {
        NoteDiamond = noteDiamond;
    }

    public void DoNoteInput (NoteInput input)
    {
        if (!Rhythm.TryHitNow()) return;

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
            Rhythm.FailCombo();
        }
    }

    void playDirection (InputDirection direction)
    {
        Note next = NoteDiamond[direction];

        if (CurrentSpell == null || Rhythm.ComboCounter == 0) // never played a note before / just casted a spell / just failed a spell
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
        CurrentSpell.CastOn(Track);
        CurrentSpell = null;
    }
}
