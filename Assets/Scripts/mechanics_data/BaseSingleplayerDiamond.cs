using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleplayerDiamond : NoteDiamond
{
    const float MULT = 2f;
    const int ADD = 1;

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

    // main: bpm up
    // meta: multiply
	class Up : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = InputDirection.Up,
            InitialVector = new EffectVector(3, false),
            Symbols = new List<BeatSymbol> { BeatSymbol.Two, BeatSymbol.Three },
            Color = Color.red,
            MetaEffectDescription = $"multiplies the effect of the spell by {MULT}",
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
            return $"increases your track's BPM by {vector.IntPower} steps ({vector.IntPower * Track.BPM_PER_BSTEP} BPM)";
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
	class Left : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = InputDirection.Left,
            InitialVector = new EffectVector(3, false),
            Symbols = new List<BeatSymbol> { BeatSymbol.Zero, BeatSymbol.Two },
            Color = new Color(0.87f, 1, 0.99f),
            MetaEffectDescription = $"decreases the effect of the spell by {ADD}",
            MainCombos = new ComboData
            {
                Left = true
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
            return $"clears {vector.IntPower} cards from your track";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            if (vector.Power > 0)
            {
                input.ClearCards(vector.IntPower);
            }
            else
            {
                input.SpawnCards(-vector.IntPower);
            }
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector - ADD;
		}
	}

    // main: card spawn rate down
    // meta: negadivide
	class Down : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = InputDirection.Down,
            InitialVector = new EffectVector(1, false),
            Symbols = new List<BeatSymbol> { BeatSymbol.Zero, BeatSymbol.One },
            Color = new Color(0.25f, 0.52f, 0.96f),
            MetaEffectDescription = $"flips the polarity of the spell and multiplies its effect by {1/MULT}",
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
            int effect = vector.IntPower;
            return $"reduces the number of cards spawned every measure by {effect}";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.CardsPerSpawn -= vector.IntPower;
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return -vector / MULT;
		}
	}

    // main: cards up
    // meta: add
	class Right : Note
	{
		protected override NoteData data => new NoteData
        {
            Direction = InputDirection.Right,
            InitialVector = new EffectVector(2, false),
            Symbols = new List<BeatSymbol> { BeatSymbol.One, BeatSymbol.Three },
            Color = new Color(0.98f, 0.58f, 0.01f),
            MetaEffectDescription = $"increases the effect of the spell by {ADD}",
            MainCombos = new ComboData
            {
                Up = true,
                Left = true
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
            return $"adds {vector.IntPower} cards to your track";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            if (vector.Power > 0)
            {
                input.SpawnCards(vector.IntPower);
            }
            else
            {
                input.ClearCards(-vector.IntPower);
            }
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + ADD;
		}
	}
}
