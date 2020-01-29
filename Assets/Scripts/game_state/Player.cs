using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public event Action<HitData> Hit;

    public readonly Rhythm Rhythm = new Rhythm();
    public readonly CommandDiamond CommandDiamond;

    public Spell Spell => Rhythm.ComboCounter == 0 || justCast ? null : innerSpell;
    public Track Track => Rhythm.Track;

    Spell innerSpell;
    bool justCast;

    public Player (CommandDiamond commandDiamond)
    {
        CommandDiamond = commandDiamond;

        Rhythm.Hit += hit => {
            if (hit.MissReason != null && hit.MissReason.Value == MissReasonEnum.NeverAttemptedBeat)
            {
                Hit?.Invoke(hit);
            }
        };
    }

    public void DoCommandInput (CommandInput input)
    {
        HitData hit = Rhythm.TryHitNow();

        if (!hit.IsSuccessful)
        {
            Hit?.Invoke(hit);
        }
        else if (input == CommandInput.Cast)
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
            Command next = CommandDiamond[direction];
            bool thisIsMainCommand = innerSpell == null || Rhythm.ComboCounter == 1 || justCast;

            innerSpell = thisIsMainCommand ? new Spell(next) : innerSpell.PlusMetaCommand(next);
            justCast = false;

            if (innerSpell.LastCommand.CanClear(Track.CurrentCardAtBeat(Rhythm.ClosestPositionInMeasure).Value))
            {
                Hit?.Invoke(originalHit);
            }
            else
            {
                Track.FailCurrentCard();
                Hit?.Invoke(originalHit.WithMissReason(MissReasonEnum.CommandCantClearAttemptedBeat));
            }
        }
        else
        {
            Rhythm.FailComboAndCard();
            Hit?.Invoke(originalHit.WithMissReason(MissReasonEnum.CommandCantCombo));
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
