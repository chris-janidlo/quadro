using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class RhythmCardGenerator
{
	public enum NoteSymbolTransition
	{
		Stay, GoToNext, GoToPrevious, JumpNextTwice, JumpPreviousTwice
	}

	// don't use the bag generator in crass for this class, since it doesn't let you seed its rng
	public static readonly IReadOnlyList<NoteSymbolTransition> TransitionBag = new List<NoteSymbolTransition>
	{
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.JumpNextTwice,
		NoteSymbolTransition.JumpNextTwice,
		NoteSymbolTransition.JumpPreviousTwice,
		NoteSymbolTransition.JumpPreviousTwice
	}.AsReadOnly();

	public RhythmCard Peek { get; private set; }

	private Random random; // each card generator gets its own rng for better fine-grained control
	private NoteSymbol currentSymbol;
	private int beatsPerMeasure;
	private List<NoteSymbolTransition> currentTransitionBag = new List<NoteSymbolTransition>(TransitionBag);

	public RhythmCardGenerator (int beatsPerMeasure) : this(beatsPerMeasure, Environment.TickCount) {}

	public RhythmCardGenerator (int beatsPerMeasure, int randomSeed)
	{
		this.beatsPerMeasure = beatsPerMeasure;
		random = new Random(randomSeed);

		var allNoteSymbols = Enum.GetValues(typeof(NoteSymbol));
		currentSymbol = (NoteSymbol) allNoteSymbols.GetValue(random.Next(allNoteSymbols.Length));

		Peek = generateNextCard();
	}

	public RhythmCard GetNext ()
	{
		var card = Peek;
		Peek = generateNextCard();
		return card;
	}

	RhythmCard generateNextCard ()
	{
		bool[] beatPattern = randomBeatPattern();

		NoteSymbol?[] symbols = new NoteSymbol?[beatPattern.Length];

		for (int i = 0; i < beatPattern.Length; i++)
		{
			if (beatPattern[i])
			{
				symbols[i] = currentSymbol;
				applySymbolTransition(getNextSymbolTransition());
			}
			else
			{
				symbols[i] = null;
			}
		}

		return new RhythmCard(symbols);
	}

	bool[] randomBeatPattern ()
	{
        int uniqueCardPermutations = (1 << beatsPerMeasure) - 1;
		int index = random.Next(1, uniqueCardPermutations);

        string paddedBinary = Convert.ToString(index, 2).PadLeft(beatsPerMeasure, '0');
        return paddedBinary.Select(c => c == '1').ToArray();
    }

	void applySymbolTransition (NoteSymbolTransition transition)
	{
		switch (transition)
		{
			case NoteSymbolTransition.Stay:
				// do nothing
				break;

			case NoteSymbolTransition.GoToNext:
				currentSymbol = currentSymbol.Next();
				break;

			case NoteSymbolTransition.GoToPrevious:
				currentSymbol = currentSymbol.Previous();
				break;

			case NoteSymbolTransition.JumpNextTwice:
				currentSymbol = currentSymbol.Next().Next();
				break;

			case NoteSymbolTransition.JumpPreviousTwice:
				currentSymbol = currentSymbol.Previous().Previous();
				break;

			default:
				throw new InvalidEnumArgumentException("unexpected NoteSymbolTransition " + transition.ToString());
		}
	}

	NoteSymbolTransition getNextSymbolTransition ()
	{
		int index = random.Next(currentTransitionBag.Count);
		var transition = currentTransitionBag[index];
		currentTransitionBag.RemoveAt(index);

		if (currentTransitionBag.Count == 0) currentTransitionBag = new List<NoteSymbolTransition>(TransitionBag);

		return transition;
	}
}
