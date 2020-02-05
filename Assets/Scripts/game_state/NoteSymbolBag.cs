using System;
using System.ComponentModel;
using System.Collections.Generic;

// rolling our own bag implementation here since 1) it's transition based and 2) we want to seed the rng and as of time of commiting that's impossible in BagRandomizer
public class NoteSymbolBag
{
	public enum Transition
	{
		Stay, GoToNext, GoToPrevious, JumpNextTwice, JumpPreviousTwice
	}

	static readonly IReadOnlyList<Transition> transitionBag = new List<Transition>
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
	}.AsReadOnly();


	List<Transition> currentTransitionBag = new List<Transition>(transitionBag);

	Random random;
	NoteSymbol currentSymbol;

	public NoteSymbolBag (Random random)
	{
		this.random = random;

		var allNoteSymbols = Enum.GetValues(typeof(NoteSymbol));
		currentSymbol = (NoteSymbol) allNoteSymbols.GetValue(random.Next(allNoteSymbols.Length));
	}

	public NoteSymbol GetNext ()
	{
		applySymbolTransition(getNextSymbolTransition());
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

	Transition getNextSymbolTransition ()
	{
		int index = random.Next(currentTransitionBag.Count);
		var transition = currentTransitionBag[index];
		currentTransitionBag.RemoveAt(index);

		if (currentTransitionBag.Count == 0) currentTransitionBag = new List<Transition>(transitionBag);

		return transition;
	}
}
