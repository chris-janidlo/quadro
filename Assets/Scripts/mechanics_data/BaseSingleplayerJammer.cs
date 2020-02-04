using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleplayerJammer : SignalJammer
{
    const float MULT = 2f;
    const int ADD = 1;

	protected override InputDirectionBox<Com> initializeComs()
	{
        return new InputDirectionBox<Com>
        {
            // all 4 coms are functionally the same, just have different internal direction values (which is necessary for things like commands to work)
            Up = new Up(),
            Left = new Left(),
            Down = new Down(),
            Right = new Right(),
        };
	}

    static string polarityEffect (float power) => (power < 0 ? "-" : "+") + " " + Mathf.Abs(power);

    // main: bpm up
    // meta: multiply
	class Up : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Up,
            InitialVector = new EffectVector(3, false),
            Symbols = new List<NoteSymbol> { NoteSymbol.Two, NoteSymbol.Three },
            Color = Color.red,
            MetaEffectDescription = $"Power * {MULT}",
            MainCombos = new ComboData
            {
                Left = true,
                Right = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Left = true,
                    Down = true,
                },
                Left = new ComboData
                {
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Up = true,
                    Down = true
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return "Scan rate " + polarityEffect(vector.IntPower);
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Track.BSteps.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector * MULT;
		}
	}

    // main: cards down
    // meta: subtract
	class Left : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Left,
            InitialVector = new EffectVector(-1, false),
            Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.Two },
            Color = new Color(0.5f, 0.8f, 1),
            MetaEffectDescription = $"Power - {ADD}",
            MainCombos = new ComboData
            {
                Left = true,
                Down = true
            },
            // max clear is 3 cards at a 7 com combo (LLDURDU). after that coms can only decrease clear amount. also a couple stalls (LR, DU)
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Down = true,
                    Right = true
                },
                Left = new ComboData
                {
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Left = true,
                    Down = true,
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return "Armor " + polarityEffect(vector.Power);
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Armor.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector - ADD;
		}
	}

    // main: card spawn rate down
    // meta: negadivide
	class Down : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Down,
            InitialVector = new EffectVector(-1, false),
            Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.One },
            Color = new Color(0.25f, 0.52f, 0.96f),
            MetaEffectDescription = $"Power * -{1/MULT}",
            MainCombos = new ComboData
            {
                Left = true,
                Right = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Left = true,
                    Down = true,
                },
                Left = new ComboData
                {
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Up = true,
                    Down = true
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return "Bandwidth " + polarityEffect(vector.Power);;
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Track.RhythmDifficulty.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return -vector / MULT;
		}
	}

    // main: cards up
    // meta: add
	class Right : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Right,
            InitialVector = new EffectVector(2, false),
            Symbols = new List<NoteSymbol> { NoteSymbol.One, NoteSymbol.Three },
            Color = new Color(0.98f, 0.58f, 0.01f),
            MetaEffectDescription = $"Power + {ADD}",
            MainCombos = new ComboData
            {
                Up = true,
                Left = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Down = true,
                    Right = true
                },
                Left = new ComboData
                {
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Left = true,
                    Down = true,
                },
                Down = new ComboData
                {
                    Up = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return "Armor " + polarityEffect(vector.Power);
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Armor.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + ADD;
		}
	}
}
