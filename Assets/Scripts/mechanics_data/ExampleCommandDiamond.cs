using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCommandDiamond : CommandDiamond
{
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

	class Up : Command
	{
		protected override CommandData data => new CommandData
        {
            Direction = InputDirection.Up,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<BeatSymbol> { BeatSymbol.Zero, BeatSymbol.One, BeatSymbol.Two, BeatSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the spell by 1",
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

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardDelta += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Left : Command
	{
		protected override CommandData data => new CommandData
        {
            Direction = InputDirection.Left,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<BeatSymbol> { BeatSymbol.Zero, BeatSymbol.One, BeatSymbol.Two, BeatSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the spell by 1",
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

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardDelta += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Down : Command
	{
		protected override CommandData data => new CommandData
        {
            Direction = InputDirection.Down,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<BeatSymbol> { BeatSymbol.Zero, BeatSymbol.One, BeatSymbol.Two, BeatSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the spell by 1",
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

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardDelta += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Right : Command
	{
		protected override CommandData data => new CommandData
        {
            Direction = InputDirection.Right,
            InitialVector = new EffectVector(1, false),
			Symbols = new List<BeatSymbol> { BeatSymbol.Zero, BeatSymbol.One, BeatSymbol.Two, BeatSymbol.Three },
            Color = Color.white,
            MetaEffectDescription = "increases the effect of the spell by 1",
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

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardDelta += vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}
}
