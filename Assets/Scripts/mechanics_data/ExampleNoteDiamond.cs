using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleNoteDiamond : NoteDiamond
{
	protected override InputDirectionBox<Note> initializeNotes()
	{
        return new InputDirectionBox<Note>
        {
            // all 4 notes are functionally the same, just have different internal direction values (which is necessary for things like spells to work)
            Up = new Up(),
            Left = new Left(),
            Down = new Down(),
            Right = new Right(),
        };
	}

	class Up : Note
	{
		protected override NoteData data => new NoteData
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
            return $"Adds {(int) vector.Power} cards to your track.";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.SpawnCards((int) vector.Power);
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Left : Note
	{
		protected override NoteData data => new NoteData
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
            return $"Adds {(int) vector.Power} cards to your track.";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.SpawnCards((int) vector.Power);
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Down : Note
	{
		protected override NoteData data => new NoteData
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
            return $"Adds {(int) vector.Power} cards to your track.";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.SpawnCards((int) vector.Power);
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}

	class Right : Note
	{
		protected override NoteData data => new NoteData
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
            return $"Adds {(int) vector.Power} cards to your track.";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.SpawnCards((int) vector.Power);
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + 1;
		}
	}
}
