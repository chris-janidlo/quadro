﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSignalJammer : SignalJammer
{
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

	class Up : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Up,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.One, NoteSymbol.Two, NoteSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the command by 1",
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Left = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return $"Adds {vector.IntPower} cards to your track.";
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Health.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Left : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Left,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.One, NoteSymbol.Two, NoteSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the command by 1",
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Left = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return $"Adds {vector.IntPower} cards to your track.";
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Health.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Down : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Down,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.One, NoteSymbol.Two, NoteSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the command by 1",
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Left = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return $"Adds {vector.IntPower} cards to your track.";
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Health.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Right : Com
	{
		protected override ComData data => new ComData
        {
            Direction = InputDirection.Right,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<NoteSymbol> { NoteSymbol.Zero, NoteSymbol.One, NoteSymbol.Two, NoteSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the command by 1",
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            MetaCombos = new MetaComboData
            {
                Up = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Left = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Right = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                },
                Down = new ComboData
                {
                    Up = true,
                    Left = true,
                    Down = true,
                    Right = true
                }
            }
        };

		public override string DescribeMainEffect (EffectVector vector)
		{
            return $"Adds {vector.IntPower} cards to your track.";
		}

		public override void MainEffect (Player input, EffectVector vector)
		{
            input.Health.Value += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}
}