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
            Color = Color.red, // TODO: specify red
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
            Color = Color.white, // TODO: placeholder. want a blue-tinted white
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
            Color = Color.blue, // TODO: specify blue
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
            return $"increases the time between card spawns by {effect} beat{(effect > 1 ? "s" : "")}";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.BeatsPerCard += vector.IntPower;
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
            Color = Color.magenta, // TODO: placeholder. want an orange
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
