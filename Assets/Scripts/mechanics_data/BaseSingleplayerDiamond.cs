using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleplayerDiamond : CommandDiamond
{
    const float MULT = 2f;
    const int ADD = 1;

	protected override InputDirectionBox<Command> initializeCommands()
	{
        return new InputDirectionBox<Command>
        {
            // all 4 commands are functionally the same, just have different internal direction values (which is necessary for things like spells to work)
            Up = new Up(),
            Left = new Left(),
            Down = new Down(),
            Right = new Right(),
        };
	}

    static string polarityEffect (float power) => (power < 0 ? "-" : "+") + " " + Mathf.Abs(power);

    // main: bpm up
    // meta: multiply
	class Up : Command
	{
		protected override CommandData data => new CommandData
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

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.BSteps += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector * MULT;
		}
	}

    // main: cards down
    // meta: subtract
	class Left : Command
	{
		protected override CommandData data => new CommandData
        {
            Direction = InputDirection.Left,
            InitialVector = new EffectVector(-1, false),
            Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.Two },
            Color = new Color(0.87f, 1, 0.99f),
            MetaEffectDescription = $"Power - {ADD}",
            MainCombos = new ComboData
            {
                Left = true,
                Down = true
            },
            // max clear is 3 cards at a 7 command combo (LLDURDU). after that commands can only decrease clear amount. also a couple stalls (LR, DU)
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
            return "Packets " + polarityEffect(vector.Power);
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardDelta += vector.Power;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector - ADD;
		}
	}

    // main: card spawn rate down
    // meta: negadivide
	class Down : Command
	{
		protected override CommandData data => new CommandData
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

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardSpawnRate += vector.Power;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return -vector / MULT;
		}
	}

    // main: cards up
    // meta: add
	class Right : Command
	{
		protected override CommandData data => new CommandData
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
            return "Packets " + polarityEffect(vector.Power);
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardDelta += vector.Power;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + ADD;
		}
	}
}
