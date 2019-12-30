using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Note
    {
        protected struct NoteData
        {
            public Direction Direction;

            // the initial state of the value that meta notes manipulate
            public float InitialPower;

            // helps to have a color for consistent visual language
            public Color Color;

            // for stuff like weapon select screens or combo list screens
            public string MetaEffectDescription;

            // the notes that can follow this if this is the first note of the spell
            public ComboData MainCombos;

            // combo data for when this is a meta note, based off of the main note for the spell
            public MetaComboData MetaCombos;
        }

        protected class ComboData : DirectionBox<bool> {}
        protected class MetaComboData : DirectionBox<ComboData> {}

        protected abstract NoteData data { get; }

        public Direction Direction => data.Direction;
        public float InitialPower => data.InitialPower;
        public Color Color => data.Color;
        public string MetaEffectDescription => data.MetaEffectDescription;

        public bool GetMainComboData (Direction nextDirection)
        {
            return data.MainCombos?[nextDirection] ?? false;
        }

        public bool GetMetaComboData (Direction mainDirection, Direction nextDirection)
        {
            return data.MetaCombos?[mainDirection]?[nextDirection] ?? false;
        }

        public abstract void MainEffect (Track input, float power);
        public abstract float MetaEffect (float power);

        // for use in the UI to describe what the spell currently is:
        public abstract string DescribeMainEffect (float power);
    }
