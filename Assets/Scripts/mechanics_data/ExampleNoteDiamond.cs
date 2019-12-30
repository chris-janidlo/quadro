using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleNoteDiamond : NoteDiamond
{
	protected override DirectionBox<Note> initializeNotes()
	{
        return new DirectionBox<Note>
        {
            // all 4 notes are functionally the same, just have different internal direction values (which is necessary for things like spells to work)
            Up = new ExampleNote1(),
            Left = new ExampleNote2(),
            Down = new ExampleNote3(),
            Right = new ExampleNote4(),
        };
	}

	class ExampleNote1 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Up,
            InitialVector = new EffectVector(1, false),
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

	class ExampleNote2 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Left,
            InitialVector = new EffectVector(1, false),
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

	class ExampleNote3 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Down,
            InitialVector = new EffectVector(1, false),
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

	class ExampleNote4 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Right,
            InitialVector = new EffectVector(1, false),
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
