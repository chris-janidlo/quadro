using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Player
{
    public event Action<HitData> Hit;

    public readonly Track Track = new Track();

    public readonly SignalJammer SignalJammer;

    public readonly BoxedInt Health;
    public readonly BoxedInt Armor;

    public readonly ComInputBox<Com> Coms;
    public readonly List<CPU> CPUs;

    public int ComboCounter { get; private set; }
    public int CPUIndex { get; private set; }

    public bool Dead => Health.Value == 0;
    public CPU ActiveCPU => CPUs[CPUIndex];

    Com lastCom;
    float armorDecayCounter;

    public Player (SignalJammer signalJammer)
    {
        SignalJammer = signalJammer;

        int mh = signalJammer.MaxHealth;
        
        Health = new BoxedInt(mh, 0, mh);
        Armor = new BoxedInt(0, 0, mh);

        Coms = new ComInputBox<Com>();

        foreach (var direction in EnumUtil.AllValues<ComInput>())
        {
            Coms[direction] = Com.FromTypeName(signalJammer.ComClassNames[direction]);
        }

        CPUs = new List<CPU>(signalJammer.NumCPUs);

        for (int i = 0; i < signalJammer.NumCPUs; i++)
        {
            CPUs.Add(new CPU(this));
        }

        Track.DidntAttemptBeat += processHit;
        Track.Beat += decayArmor;
    }

    public void DoInput (InputFrame input)
    {
        HitData hit = Track.GetHitByAccuracy();

        if (!hit.ClearedBeat)
        {
            processHit(hit);
            return;
        }

        if (input.ComInput != null)
        {
            tryPlayDirection(input.ComInput.Value, hit);
        }
        else if (input.CPUSwitchInput != null)
        {
            trySwitchCPU(input.CPUSwitchInput.Value, hit);
        }
        else
        {
            tryExecuteCPU(hit);
        }
    }

    public bool CanComboInto (ComInput direction)
        => lastCom == null || lastCom.ComboData[direction];

    // damage must be a non-negative value
    public void TakeShieldedDamage (int damage)
    {
        if (damage == 0) return;

        if (damage < 0) throw new ArgumentException($"damage cannot be less than 0 (was given {damage})");

        int damageToBeDone = damage;

        while (damageToBeDone > 0)
        {
            if (Armor.Value > 0) Armor.Value--;
            else Health.Value--;

            damageToBeDone--;
        }
    }

    void tryPlayDirection (ComInput direction, HitData originalHit)
    {
        if (CanComboInto(direction))
        {
            lastCom = Coms[direction];

            if (Track.ClosestHittableNote().Symbol.HasInChord(direction))
                lastCom.DoEffect(ActiveCPU);

            processHit(originalHit);
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.ComCantCombo));
        }
    }

    void trySwitchCPU (CPUSwitchInput input, HitData originalHit)
    {
        int targetIndex = (int) input;

        if (targetIndex != CPUIndex && targetIndex < CPUs.Count)
        {
            CPUIndex = targetIndex;
            processHit(originalHit);
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.AlreadyOnCPU));
        }
    }

    void tryExecuteCPU (HitData originalHit)
    {
        if (ActiveCPU.Instr != null)
        {
            ActiveCPU.Execute();
            processHit(originalHit);
        }
        else
        {
            processHit(originalHit.WithMissReason(MissedHitReason.CPUHasNoInstr));
        }
    }

    void decayArmor ()
    {
        armorDecayCounter += SignalJammer.ArmorDecayPerBeat;

        while (armorDecayCounter >= 1)
        {
            Armor.Value--;
            armorDecayCounter--;
        }
    }

    void processHit (HitData hit)
    {
        if (!hit.ClearedBeat)
        {
            ComboCounter = 0;
        }

        Health.Value += hit.Quality.Healing();
        TakeShieldedDamage(hit.MissReason?.Damage() ?? 0);

        Hit?.Invoke(hit);
    }
}
