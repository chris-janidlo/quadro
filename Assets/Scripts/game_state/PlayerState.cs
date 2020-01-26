using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public event Action<HitData> Hit;

    public readonly Rhythm Rhythm = new Rhythm();
    public readonly NoteDiamond NoteDiamond;

    public Spell Spell => Rhythm.ComboCounter == 0 || justCast ? null : innerSpell;
    public Track Track => Rhythm.Track;

    Spell innerSpell;
    bool justCast;

    public PlayerState (NoteDiamond noteDiamond)
    {
        NoteDiamond = noteDiamond;

        Rhythm.Hit += hit => {
            if (hit.MissReason != null && hit.MissReason.Value == MissReasonEnum.NeverAttemptedBeat)
            {
                Hit?.Invoke(hit);
            }
        };
    }

    public void DoNoteInput (NoteInput input)
    {
        HitData hit = Rhythm.TryHitNow();

        if (!hit.IsSuccessful)
        {
            Hit?.Invoke(hit);
        }
        else if (input == NoteInput.Cast)
        {
            tryCastSpell(hit);
        }
        else
        {
            tryPlayDirection((InputDirection) input, hit);
        }
    }

    public bool CanComboInto (InputDirection direction)
    {
        return innerSpell == null || innerSpell.CanComboInto(direction);
    }

    void tryPlayDirection (InputDirection direction, HitData originalHit)
    {
        if (CanComboInto(direction))
        {
            Note next = NoteDiamond[direction];
            bool thisIsMainNote = innerSpell == null || Rhythm.ComboCounter == 1 || justCast;

            innerSpell = thisIsMainNote ? new Spell(next) : innerSpell.PlusMetaNote(next);
            justCast = false;

            if (innerSpell.LastNote.CanClear(Track.CurrentCardAtBeat(Rhythm.ClosestPositionInMeasure).Value))
            {
                Hit?.Invoke(originalHit);
            }
            else
            {
                Track.FailCurrentCard();
                Hit?.Invoke(originalHit.WithMissReason(MissReasonEnum.NoteCantClearAttemptedBeat));
            }
        }
        else
        {
            Rhythm.FailComboAndCard();
            Hit?.Invoke(originalHit.WithMissReason(MissReasonEnum.NoteCantCombo));
        }
    }

    void tryCastSpell (HitData originalHit)
    {
        if (Spell != null)
        {
            Spell.CastOn(Track);
            Hit?.Invoke(originalHit);
        }
        else
        {
            Rhythm.FailComboAndCard();
            Hit?.Invoke(originalHit.WithMissReason(MissReasonEnum.InvalidCastInput));
        }

        justCast = true;
    }
}
