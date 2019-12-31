using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public Track Track => rhythm.Track;

    readonly NoteDiamond noteDiamond;
    Spell currentSpell;
    int comboCounter;
    Rhythm rhythm = new Rhythm();

    public PlayerState (NoteDiamond noteDiamond)
    {
        this.noteDiamond = noteDiamond;
    }

    public void DoNoteInput (NoteInput input)
    {
        bool success = rhythm.TryHitNow();

        if (success && input == NoteInput.Cast && rhythm.IsDownbeat())
        {
            castSpell();
        }
        else if (success)
        {
            playDirection((InputDirection) input);
        }
        else
        {
            comboCounter = 0;
        }
    }

    void playDirection (InputDirection direction)
    {
        Note next = noteDiamond[direction];

        if (currentSpell == null || comboCounter == 0) // never played a note before / just casted a spell / just failed a spell
        {
            currentSpell = new Spell(next);
            comboCounter++;
        }
        else if (currentSpell.CanComboInto(direction))
        {
            currentSpell = comboCounter == 0 ? new Spell(next) : currentSpell.PlusMetaNote(next);
            comboCounter++;
        }
        else
        {
            comboCounter = 0;
        }
    }

    void castSpell ()
    {
        currentSpell.CastOn(Track);
        currentSpell = null;
        comboCounter = 0;
    }
}
