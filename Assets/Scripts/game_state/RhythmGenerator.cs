using System;
using System.Collections.Generic;

public class RhythmGenerator
{
	public const int MIN_DIFFICULTY = 1, MAX_DIFFICULTY = 16;

	readonly IReadOnlyDictionary<int, IRhythmGeneratorStrategy> strategies;

	int previousDifficulty = MIN_DIFFICULTY - 1;

	public RhythmGenerator (int beatsPerMeasure, int randomSeed)
	{
		Random random = new Random(randomSeed);
		strategies = initializeStrategies(beatsPerMeasure, random);
	}

	public RhythmGenerator (int beatsPerMeasure) : this(beatsPerMeasure, Environment.TickCount) {}

	// TODO: inner-beat patterns: eighth notes, sixteenth notes, triplets
	// TODO: multi-beat patterns: broken triplets, 4 against 3, any other arbitrary pattern
	public Beat GetNextBeat (int positionInMeasure, int difficulty)
	{
		if (previousDifficulty == MIN_DIFFICULTY - 1) previousDifficulty = difficulty;

		int prev = previousDifficulty;
		previousDifficulty = difficulty;

		if (difficulty != prev)
		{
			strategies[prev].ClearPositionState();
			return new Beat(positionInMeasure);
		}
		else
		{
			return strategies[difficulty].GetNextBeat(positionInMeasure);
		}
	}

	Dictionary<int, IRhythmGeneratorStrategy> initializeStrategies (int beatsPerMeasure, Random random)
	{
		NoteSymbolBag symbolBag = new NoteSymbolBag(random);

		// really spacious for easy editing!
		var strats = new Dictionary<int, IRhythmGeneratorStrategy>
		{
			{
				1,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Breve, 0f)
			},
			{
				2,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Breve, 0.5f)
			},
			{
				3,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Whole, 0.5f)
			},
			{
				4,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Half, 0f)
			},
			{
				5,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Half, 0.1f)
			},
			{
				6,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Half, 0.25f)
			},
			{
				7,
				new CutTimeStrategy(CutTimeStrategy.NoteLength.Half, 0.5f)
			},
			{
				8,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				9,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				10,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				11,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				12,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				13,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				14,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				15,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			},
			{
				16,
				new RandomOnTheBeatStrategy(beatsPerMeasure)
			}
		};

		// dependency injection
		foreach (KeyValuePair<int, IRhythmGeneratorStrategy> strat in strats)
		{
			strat.Value.SymbolBag = symbolBag;
			strat.Value.Random = random;
		}

		return strats;
	}
}
