using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Player
{
    public event Action<HitData> Hit;

    public Player Opponent;

    public readonly Track Track;

    public readonly SignalJammer SignalJammer;

    public readonly BoxedInt Health;
    public readonly BoxedInt Armor;

    public readonly CommandInputBox<Command> Commands;
    public readonly List<CPU> CPUs;

    public int ComboCounter { get; private set; }
    public int CPUIndex { get; private set; }

    public bool Dead => Health.Value == 0;
    public CPU ActiveCPU => CPUs[CPUIndex];

    Command lastCommand;
    float armorDecayCounter;

    public Player (SignalJammer signalJammer, int seed)
    {
        Track = new Track(seed);
    
        SignalJammer = signalJammer;

        int mh = signalJammer.MaxHealth;
        
        Health = new BoxedInt(mh, 0, mh);
        Armor = new BoxedInt(0, 0, mh);

        Commands = new CommandInputBox<Command>();

        foreach (var direction in EnumUtil.AllValues<CommandInput>())
        {
            Commands[direction] = Command.FromTypeName(signalJammer.CommandClassNames[direction]);
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
        if (input.CPUSwitchInput != null)
        {
            CPUIndex = (int) input.CPUSwitchInput.Value;
            return;
        }

        HitData hit = Track.GetHitByAccuracy();

        if (!hit.ClearedBeat)
        {
            processHit(hit);
        }
        else if (input.CommandInput != null)
        {
            tryPlayDirection(input.CommandInput.Value, hit);
        }
    }

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

    void tryPlayDirection (CommandInput direction, HitData originalHit)
    {
        lastCommand = Commands[direction];

        if (Track.ClosestHittableNote().Symbol.HasInChord(direction))
            ActiveCPU.Registers = lastCommand.DoEffect(this, ActiveCPU.Registers);

        processHit(originalHit);
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
