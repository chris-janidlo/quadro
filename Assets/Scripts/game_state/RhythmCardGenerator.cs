using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class RhythmCardGenerator
{
	public enum BeatSymbolTransition
	{
		Stay, GoToNext, GoToPrevious, JumpNextTwice, JumpPreviousTwice
	}

	// don't use the bag generator in crass for this class, since it doesn't let you seed its rng
	public static readonly IReadOnlyList<BeatSymbolTransition> TransitionBag = new List<BeatSymbolTransition>
	{
		BeatSymbolTransition.Stay,
		BeatSymbolTransition.Stay,
		BeatSymbolTransition.Stay,
		BeatSymbolTransition.Stay,
		BeatSymbolTransition.GoToNext,
		BeatSymbolTransition.GoToNext,
		BeatSymbolTransition.GoToNext,
		BeatSymbolTransition.GoToNext,
		BeatSymbolTransition.GoToPrevious,
		BeatSymbolTransition.GoToPrevious,
		BeatSymbolTransition.GoToPrevious,
		BeatSymbolTransition.GoToPrevious,
		BeatSymbolTransition.JumpNextTwice,
		BeatSymbolTransition.JumpNextTwice,
		BeatSymbolTransition.JumpPreviousTwice,
		BeatSymbolTransition.JumpPreviousTwice
	}.AsReadOnly();

	public RhythmCard Peek { get; private set; }

	private Random random; // each card generator gets its own rng for better fine-grained control
	private BeatSymbol currentSymbol;
	private int beatsPerMeasure;
	private List<BeatSymbolTransition> currentTransitionBag = new List<BeatSymbolTransition>(TransitionBag);

	public RhythmCardGenerator (int beatsPerMeasure) : this(beatsPerMeasure, Environment.TickCount) {}

	public RhythmCardGenerator (int beatsPerMeasure, int randomSeed)
	{
		this.beatsPerMeasure = beatsPerMeasure;
		random = new Random(randomSeed);

		var allBeatSymbols = Enum.GetValues(typeof(BeatSymbol));
		currentSymbol = (BeatSymbol) allBeatSymbols.GetValue(random.Next(allBeatSymbols.Length));

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

		BeatSymbol?[] symbols = new BeatSymbol?[beatPattern.Length];

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

	void applySymbolTransition (BeatSymbolTransition transition)
	{
		switch (transition)
		{
			case BeatSymbolTransition.Stay:
				// do nothing
				break;

			case BeatSymbolTransition.GoToNext:
				currentSymbol = currentSymbol.Next();
				break;

			case BeatSymbolTransition.GoToPrevious:
				currentSymbol = currentSymbol.Previous();
				break;

			case BeatSymbolTransition.JumpNextTwice:
				currentSymbol = currentSymbol.Next().Next();
				break;

			case BeatSymbolTransition.JumpPreviousTwice:
				currentSymbol = currentSymbol.Previous().Previous();
				break;

			default:
				throw new InvalidEnumArgumentException("unexpected BeatSymbolTransition " + transition.ToString());
		}
	}

	BeatSymbolTransition getNextSymbolTransition ()
	{
		int index = random.Next(currentTransitionBag.Count);
		var transition = currentTransitionBag[index];
		currentTransitionBag.RemoveAt(index);

		if (currentTransitionBag.Count == 0) currentTransitionBag = new List<BeatSymbolTransition>(TransitionBag);

		return transition;
	}
}
