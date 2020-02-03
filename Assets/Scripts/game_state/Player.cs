using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Player
{
    public const int ARMOR_DECAY_RATE = 1;
    public const int NOTE_CLEARS_PER_DIFFICULTY_INCREASE = 24;

    public const float SCREEN_SHAKE_TIME = 0.3f;
    public const float SCREEN_SHAKE_AMOUNT = 2;

    public event Action<HitData> Hit;

    public readonly Track Track = new Track();

    public readonly SignalJammer SignalJammer;

    public readonly BoxedInt Health = new BoxedInt(16, 0, 16);
    public readonly BoxedInt Armor = new BoxedInt(0, 0, 16);

    public int ComboCounter { get; private set; }
    public int DifficultyIncreaseTicker { get; private set; } = NOTE_CLEARS_PER_DIFFICULTY_INCREASE;

    public bool Dead => Health.Value == 0;
    public Command Command => ComboCounter == 0 || justCast ? null : innerCommand;

    Command innerCommand;
    bool justCast;

    public Player (SignalJammer signalJammer)
    {
        SignalJammer = signalJammer;

        Track.DidntAttemptBeat += processHit;
        Track.Beat += decayArmor;
    }

    public void DoComInput (ComInput input)
    {
        HitData hit = Track.GetHitByAccuracy();

        if (hit.KillsCommand)
        {
            processHit(hit);
        }
        else if (input == ComInput.Cast)
        {
            tryCastCommand(hit);
        }
        else
        {
            tryPlayDirection((InputDirection) input, hit);
        }
    }

    public bool CanComboInto (InputDirection direction)
    {
        return innerCommand == null || innerCommand.CanComboInto(direction);
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
        int healthDamage = 0;

        while (damageToBeDone > 0)
        {
            if (Armor.Value > 0)
            {
                Armor.Value--;
            }
            else
            {
                Health.Value--;
                healthDamage++;
            }

            damageToBeDone--;
        }

        CameraCache.Main.ShakeScreen2D(SCREEN_SHAKE_TIME, SCREEN_SHAKE_AMOUNT);
    }

    void tryPlayDirection (InputDirection direction, HitData originalHit)
    {
        if (CanComboInto(direction))
        {
            Com next = SignalJammer[direction];
            bool thisIsMainCom = innerCommand == null || ComboCounter == 0 || justCast;

            innerCommand = thisIsMainCom ? new Command(next) : innerCommand.PlusMetaCom(next);
            justCast = false;

            if (innerCommand.LastCom.CanClear(Track.ClosestHittableNote().Symbol))
            {
                processHit(originalHit);
            }
            else
            {
                processHit(originalHit.WithMissReason(MissedHitReason.ComCantClearAttemptedBeat));
            }
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.ComCantCombo));
        }
    }

    void tryCastCommand (HitData originalHit)
    {
        if (Command != null)
        {
            processHit(originalHit);
            Command.CastOn(this);
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.InvalidCastInput));
        }

        justCast = true;
    }

    void decayArmor ()
    {
        if (Track.TruncatedPositionInMeasure == 0)
        {
            Armor.Value -= ARMOR_DECAY_RATE;
        }
    }

    void processHit (HitData hit)
    {
        if (hit.KillsCommand)
        {
            ComboCounter = 0;
        }
        else
        {
            ComboCounter++;

            DifficultyIncreaseTicker--;

            if (DifficultyIncreaseTicker <= 0)
            {
                DifficultyIncreaseTicker = NOTE_CLEARS_PER_DIFFICULTY_INCREASE;
                Track.BSteps.Value++;
                Track.RhythmDifficulty.Value++;
            }
        }

        Health.Value += hit.Quality.Healing();
        TakeShieldedDamage(hit.MissReason?.Damage() ?? 0);

        Hit?.Invoke(hit);
    }
}
