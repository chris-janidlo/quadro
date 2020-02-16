using System;
using System.ComponentModel;
using System.Collections.Generic;
using crass;

// rolling our own bag implementation here since 1) it's transition based and 2) we want to seed the rng and as of time of commiting that's impossible in BagRandomizer
public class NoteSymbolBag
{
	public enum Transition
	{
		Stay, GoToNext, GoToPrevious, JumpNextTwice, JumpPreviousTwice
	}

	BagRandomizer<Transition> transitionBag = new BagRandomizer<Transition>
	{
		Items = new List<Transition>
		{
			Transition.Stay,
			Transition.Stay,
			Transition.Stay,
			Transition.Stay,
			Transition.GoToNext,
			Transition.GoToNext,
			Transition.GoToNext,
			Transition.GoToNext,
			Transition.GoToPrevious,
			Transition.GoToPrevious,
			Transition.GoToPrevious,
			Transition.GoToPrevious,
			Transition.JumpNextTwice,
			Transition.JumpNextTwice,
			Transition.JumpPreviousTwice,
			Transition.JumpPreviousTwice
		},
		Type = BagRandomizer<Transition>.PRNGType.Local
	};

	NoteSymbol currentSymbol;

	public NoteSymbolBag (Random random)
	{
		transitionBag.SetPRNG(random);

		var allNoteSymbols = Enum.GetValues(typeof(NoteSymbol));
		currentSymbol = (NoteSymbol) allNoteSymbols.GetValue(random.Next(allNoteSymbols.Length));
	}

	public NoteSymbol GetNext ()
	{
		applySymbolTransition(transitionBag.GetNext());
		return currentSymbol;
	}

	void applySymbolTransition (Transition transition)
	{
		switch (transition)
		{
			case Transition.Stay:
				// do nothing
				break;

			case Transition.GoToNext:
				currentSymbol = currentSymbol.Next();
				break;

			case Transition.GoToPrevious:
				currentSymbol = currentSymbol.Previous();
				break;

			case Transition.JumpNextTwice:
				currentSymbol = currentSymbol.Next().Next();
				break;

			case Transition.JumpPreviousTwice:
				currentSymbol = currentSymbol.Previous().Previous();
				break;

			default:
				throw new InvalidEnumArgumentException("unexpected NoteSymbolTransition " + transition.ToString());
		}
	}
}
