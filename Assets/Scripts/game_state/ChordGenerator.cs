using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using crass;

public class ChordGenerator
{
	public Chord CurrentChord { get; private set; }
	public Chord NextChord { get; private set; }

	public int BeatsUntilNextChord { get; private set; }

	enum Transition
	{
		Stay, GoToNext, GoToPrevious, JumpNextTwice, JumpPreviousTwice
	}

	BagRandomizer<Transition> transitionBag = new BagRandomizer<Transition>
	{
		Items = new List<Transition>
		{
			Transition.Stay,
			Transition.Stay,
			Transition.GoToNext,
			Transition.GoToNext,
			Transition.GoToPrevious,
			Transition.GoToPrevious,
			Transition.JumpNextTwice,
			Transition.JumpPreviousTwice
		},
		Type = BagRandomizer<Transition>.PRNGType.Local
	};

	BagRandomizer<int> durationBag = new BagRandomizer<int>
	{
		Items = new List<int>
		{
			4,
			4,
			4,
			4,
			4,
			2, 2,
			2, 2,
			3, 1
		},
		Type = BagRandomizer<int>.PRNGType.Local
	};

	Transition nextTransition;

	public ChordGenerator (int randomSeed)
	{
		Random random = new Random(randomSeed);

		transitionBag.SetPRNG(random);
		durationBag.SetPRNG(random);

		nextTransition = transitionBag.GetNext();
		BeatsUntilNextChord = durationBag.GetNext() + 1;

		// can't use PickRandom here because we need to specify exactly what PRNG is used
		var allChords = EnumUtil.AllValues<Chord>();
		CurrentChord = allChords.ToList()[random.Next(allChords.Count())];
	}

	public void TickBeat ()
	{
		BeatsUntilNextChord--;

		if (BeatsUntilNextChord <= 0)
		{
			CurrentChord = doNextTransition(CurrentChord);

			nextTransition = transitionBag.GetNext();
			NextChord = doNextTransition(CurrentChord);

			BeatsUntilNextChord = durationBag.GetNext();
		}
	}

	Chord doNextTransition (Chord chord)
	{
		switch (nextTransition)
		{
			case Transition.Stay: return chord;
			case Transition.GoToNext: return chord.Next();
			case Transition.GoToPrevious: return chord.Previous();
			case Transition.JumpNextTwice: return chord.Next().Next();
			case Transition.JumpPreviousTwice: return chord.Previous().Previous();

			default: throw new InvalidEnumArgumentException("unexpected ChordTransition " + nextTransition.ToString());
		}
	}
}
