using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    readonly NoteDiamond noteDiamond;
    Spell currentSpell;
    int comboCounter;
    Track track;

    public PlayerState (NoteDiamond noteDiamond, Track track = null)
    {
        this.noteDiamond = noteDiamond;
        this.track = track ?? new Track();
    }

    public void PlayDirection (Direction direction)
    {
        Note next = noteDiamond[direction];

        if (currentSpell == null) // never played a note before / just casted a spell
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

    public void CastSpell ()
    {
        currentSpell.CastOn(track);
        currentSpell = null;
        comboCounter = 0;
    }
}
