using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public event Action<HitData> Hit;

    public readonly Rhythm Rhythm = new Rhythm();
    public readonly NoteDiamond NoteDiamond;

    public Spell Spell => Rhythm.ComboCounter > 0 ? innerSpell : null;
    public Track Track => Rhythm.Track;

    Spell innerSpell;

    public PlayerState (NoteDiamond noteDiamond)
    {
        NoteDiamond = noteDiamond;

        Rhythm.Hit += hit => {
            if (hit.MissReason == null || hit.MissReason.Value != MissReasonEnum.NeverAttemptedBeat) return;
            Hit?.Invoke(hit);
        };
    }

    public void DoNoteInput (NoteInput input)
    {
        bool comboWasZero = Rhythm.ComboCounter == 0;

        HitData hit = Rhythm.TryHitNow();

        if (!hit.IsSuccessful)
        {
            Hit?.Invoke(hit);
            return;
        }

        if (input != NoteInput.Cast)
        {
            bool comboSuccess = tryPlayDirection((InputDirection) input, comboWasZero);

            if (!comboSuccess)
            {
                hit = hit.WithMissReason(MissReasonEnum.NoteCantCombo);
            }
            else if (!innerSpell.LastNote.CanClear(Track.CurrentCardAtBeat(Rhythm.ClosestPositionInMeasure).Value))
            {
                hit = hit.WithMissReason(MissReasonEnum.NoteCantClearAttemptedBeat);
                Rhythm.FailCard();
            }
        }
        else if (Rhythm.IsDownbeat())
        {
            if (!tryCastSpell()) hit = hit.WithMissReason(MissReasonEnum.InvalidCastInput);
        }
        else
        {
            hit = hit.WithMissReason(MissReasonEnum.InvalidCastInput);
            Rhythm.FailComboAndCard();
        }

        Hit?.Invoke(hit);
    }

    public bool CanComboInto (InputDirection direction)
    {
        return innerSpell == null || innerSpell.CanComboInto(direction);
    }

    bool tryPlayDirection (InputDirection direction, bool thisIsMainNote)
    {
        Note next = NoteDiamond[direction];

        if (innerSpell == null) // never played a note before / just casted a spell
        {
            innerSpell = new Spell(next);
            return true;
        }
        else if (innerSpell.CanComboInto(direction))
        {
            innerSpell = thisIsMainNote ? new Spell(next) : innerSpell.PlusMetaNote(next);
            return true;
        }
        else
        {
            Rhythm.FailComboAndCard();
            return false;
        }
    }

    bool tryCastSpell ()
    {
        if (innerSpell != null)
        {
            innerSpell.CastOn(Track);
            innerSpell = null;
            return true;
        }
        else
        {
            Rhythm.FailComboAndCard();
            return false;
        }
    }
}
