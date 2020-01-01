using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public readonly Rhythm Rhythm = new Rhythm();

    public Track Track => Rhythm.Track;

    readonly NoteDiamond noteDiamond;
    Spell currentSpell;

    public PlayerState (NoteDiamond noteDiamond)
    {
        this.noteDiamond = noteDiamond;
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
        Note next = noteDiamond[direction];

        if (currentSpell == null || Rhythm.ComboCounter == 0) // never played a note before / just casted a spell / just failed a spell
        {
            currentSpell = new Spell(next);
        }
        else if (currentSpell.CanComboInto(direction))
        {
            currentSpell = Rhythm.ComboCounter == 0 ? new Spell(next) : currentSpell.PlusMetaNote(next);
        }
        else
        {
            Rhythm.FailCombo();
        }
    }

    void castSpell ()
    {
        currentSpell.CastOn(Track);
        currentSpell = null;
    }
}
