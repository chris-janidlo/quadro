using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleplayerDiamond : NoteDiamond
{
    const float MULT = 1.5f;
    const int ADD = 2;

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
            // TODO:
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            // TODO:
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
            return $"increases your track's BPM by {((int) vector.Power) * Track.BPM_PER_BSTEP}";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.BSteps += (int) vector.Power;
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
            InitialVector = new EffectVector(2, false),
            Color = Color.white, // TODO: placeholder. want a blue-tinted white
            MetaEffectDescription = $"decreases the effect of the spell by {ADD}",
            // TODO:
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            // TODO:
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
            return $"clears {(int) vector.Power} cards from your track";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            if (vector.Power > 0)
            {
                input.ClearCards((int) vector.Power);
            }
            else
            {
                input.SpawnCards((int) -vector.Power);
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
            // TODO:
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            // TODO:
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
            int effect = (int) vector.Power;
            return $"increases the time between card spawns by {effect} beat{(effect > 1 ? "s" : "")}";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            input.BeatsPerCard += (int) vector.Power;
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
            // TODO:
            MainCombos = new ComboData
            {
                Up = true,
                Left = true,
                Down = true,
                Right = true
            },
            // TODO:
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
            return $"adds {(int) vector.Power} cards to your track";
		}

		public override void MainEffect (Track input, EffectVector vector)
		{
            if (vector.Power > 0)
            {
                input.SpawnCards((int) vector.Power);
            }
            else
            {
                input.ClearCards((int) -vector.Power);
            }
		}

		public override EffectVector MetaEffect (EffectVector vector)
		{
            return vector + ADD;
		}
	}
}
