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
            InitialPower = 1,
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

		public override string DescribeMainEffect (float power)
		{
            return $"Adds {(int) power} cards to your track.";
		}

		public override void MainEffect (Track input, float power)
		{
            // TODO: add cards
		}

		public override float MetaEffect (float power)
		{
            return power + 1;
		}
	}

	class ExampleNote2 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Left,
            InitialPower = 1,
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

		public override string DescribeMainEffect (float power)
		{
            return $"Adds {(int) power} cards to your track.";
		}

		public override void MainEffect (Track input, float power)
		{
            // TODO: add cards
		}

		public override float MetaEffect (float power)
		{
            return power + 1;
		}
	}

	class ExampleNote3 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Down,
            InitialPower = 1,
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

		public override string DescribeMainEffect (float power)
		{
            return $"Adds {(int) power} cards to your track.";
		}

		public override void MainEffect (Track input, float power)
		{
            // TODO: add cards
		}

		public override float MetaEffect (float power)
		{
            return power + 1;
		}
	}

	class ExampleNote4 : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = Direction.Right,
            InitialPower = 1,
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

		public override string DescribeMainEffect (float power)
		{
            return $"Adds {(int) power} cards to your track.";
		}

		public override void MainEffect (Track input, float power)
		{
            // TODO: add cards
		}

		public override float MetaEffect (float power)
		{
            return power + 1;
		}
	}
}
