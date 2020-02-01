using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public const int ARMOR_DECAY_RATE = 1;

    public event Action<HitData> Hit;

    public readonly Track Track = new Track();

    public readonly CommandDiamond CommandDiamond;

    public readonly BoxedInt Health = new BoxedInt(16, 0, 16);
    public readonly BoxedInt Armor = new BoxedInt(0, 0, int.MaxValue);

    public int ComboCounter { get; private set; }

    public bool Dead => Health.Value == 0;
    public Spell Spell => ComboCounter == 0 || justCast ? null : innerSpell;

    Spell innerSpell;
    bool justCast;

    public Player (CommandDiamond commandDiamond)
    {
        CommandDiamond = commandDiamond;

        Track.DidntAttemptBeat += processHit;
        Track.Beat += decayArmor;
    }

    public void DoCommandInput (CommandInput input)
    {
        HitData hit = Track.GetHitByAccuracy();

        if (hit.KillsSpell)
        {
            processHit(hit);
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

    // damage must be a non-negative value
    public void TakeShieldedDamage (int damage)
    {
        if (damage == 0) return;

        if (damage < 0)
        {
            throw new ArgumentException($"damage cannot be less than 0 (was given {damage})");
        }

        int damageToBeDone = damage;

        while (damageToBeDone > 0)
        {
            if (Armor.Value > 0)
            {
                Armor.Value--;
            }
            else
            {
                Health.Value--;
            }

            damageToBeDone--;
        }
    }

    void tryPlayDirection (InputDirection direction, HitData originalHit)
    {
        if (CanComboInto(direction))
        {
            Command next = CommandDiamond[direction];
            bool thisIsMainCommand = innerSpell == null || ComboCounter == 0 || justCast;

            innerSpell = thisIsMainCommand ? new Spell(next) : innerSpell.PlusMetaCommand(next);
            justCast = false;

            if (innerSpell.LastCommand.CanClear(Track.ClosestHittableNote().Symbol))
            {
                processHit(originalHit);
            }
            else
            {
                processHit(originalHit.WithMissReason(MissedHitReason.CommandCantClearAttemptedBeat));
            }
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.CommandCantCombo));
        }
    }

    void tryCastSpell (HitData originalHit)
    {
        if (Spell != null)
        {
            processHit(originalHit);
            Spell.CastOn(this);
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.InvalidCastInput));
        }

        justCast = true;
    }

    void decayArmor ()
    {
        Armor.Value -= ARMOR_DECAY_RATE;
    }

    void processHit (HitData hit)
    {
        if (hit.KillsSpell)
        {
            ComboCounter = 0;
        }
        else
        {
            ComboCounter++;
        }

        Health.Value += hit.Quality.Healing();
        TakeShieldedDamage(hit.MissReason?.Damage() ?? 0);

        Hit?.Invoke(hit);
    }
}
