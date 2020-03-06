using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Player
{
    public const int MISS_DAMAGE = 10,
                     NUM_CPUS = 4;

    public event Action<HitData> Hit;

    public Player Opponent;

    public readonly Track Track;

    public readonly SignalJammer SignalJammer;

    public readonly BoxedInt Health;
    public readonly BoxedInt Armor;

    public readonly CommandMap Commands;
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

        Commands = new CommandMap();

        foreach (var zone in EnumUtil.AllValues<CommandZone>())
        {
            foreach (var button in EnumUtil.AllValues<CommandButton>())
            {
                Commands[zone][button] = Command.FromTypeName(signalJammer.CommandClassNames[zone][button]);
            }
        }

        CPUs = new List<CPU>(NUM_CPUS);

        for (int i = 0; i < NUM_CPUS; i++)
        {
            CPUs.Add(new CPU(this));
        }

        Track.Beat += decayArmor;
    }

    public void SwitchActiveCPU (CPUSwitchInput input) => CPUIndex = (int) input;

    public void RunCommand (CommandZone zone, CommandButton button)
    {
        HitData hit = Track.TryHitNow();

        if (hit.ClearedBeat)
        {
            lastCommand = Commands[zone][button];
            ActiveCPU.Registers = lastCommand.DoEffect(this, ActiveCPU.Registers);
        }

        processHit(hit);
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
            TakeShieldedDamage(MISS_DAMAGE);
            ComboCounter = 0;
        }

        Health.Value += hit.Quality.Healing();

        Hit?.Invoke(hit);
    }
}
