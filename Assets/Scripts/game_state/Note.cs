using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class Note
    {
        public struct EffectVector
        {
            public readonly float Power; // generic numeric scalar
            public readonly bool TargetsEnemy; // TODO: do something with this

            public int IntPower => (int) Power;

            public EffectVector (float power, bool targetsEnemy)
            {
                Power = power;
                TargetsEnemy = targetsEnemy;
            }

            public static EffectVector operator + (EffectVector v) => v;
            public static EffectVector operator - (EffectVector v) => new EffectVector(-v.Power, v.TargetsEnemy);

            public static EffectVector operator + (EffectVector v, float f) => new EffectVector(v.Power + f, v.TargetsEnemy);
            public static EffectVector operator - (EffectVector v, float f) => v + (-f);

            public static EffectVector operator * (EffectVector v, float f) => new EffectVector(v.Power * f, v.TargetsEnemy);
            public static EffectVector operator / (EffectVector v, float f)
            {
                if (f == 0)
                {
                    throw new DivideByZeroException();
                }

                return new EffectVector(v.Power / f, v.TargetsEnemy);
            }
        }

        protected struct NoteData
        {
            public InputDirection Direction;

            // the initial state of the value that meta notes manipulate
            public EffectVector InitialVector;

            // the symbols that this note can clear
            public List<BeatSymbol> Symbols;

            // helps to have a color for consistent visual language
            public Color Color;

            // for stuff like weapon select screens or combo list screens
            public string MetaEffectDescription;

            // the notes that can follow this if this is the first note of the spell
            public ComboData MainCombos;

            // combo data for any meta notes following this as the main note
            public MetaComboData MetaCombos;
        }

        protected class ComboData : InputDirectionBox<bool> {}
        protected class MetaComboData : InputDirectionBox<ComboData> {}

        protected abstract NoteData data { get; }

        public InputDirection Direction => data.Direction;
        public EffectVector InitialVector => data.InitialVector;
        public ReadOnlyCollection<BeatSymbol> Symbols => data.Symbols.AsReadOnly();
        public Color Color => data.Color;
        public string MetaEffectDescription => data.MetaEffectDescription;

        public bool GetMainComboData (InputDirection nextDirection)
        {
            return data.MainCombos?[nextDirection] ?? false;
        }

        public bool GetMetaComboData (InputDirection mainDirection, InputDirection nextDirection)
        {
            return data.MetaCombos?[mainDirection]?[nextDirection] ?? false;
        }

        public bool CanClear (BeatSymbol symbol)
        {
            return Symbols.Contains(symbol);
        }

        public abstract void MainEffect (Track input, EffectVector vector);
        public abstract EffectVector MetaEffect (EffectVector vector);

        // for use in the UI to describe what the spell currently is:
        public abstract string DescribeMainEffect (EffectVector vector);
    }
