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
				new RandomOnTheBeatStrategy(beatsPerMeasure, random)
			},
			{
				2,
				new RandomOnTheBeatStrategy(beatsPerMeasure, random)
			},
			{
				3,
				new RandomOnTheBeatStrategy(beatsPerMeasure, random)
			},
			{
				4,
				new RandomOnTheBeatStrategy(beatsPerMeasure, random)
			},
			{
				5,
				new EveryBeatStrategy()
			},
			{
				6,
				new EveryBeatStrategy()
			},
			{
				7,
				new EveryBeatStrategy()
			},
			{
				8,
				new EveryBeatStrategy()
			},
			{
				9,
				new EveryBeatStrategy()
			},
			{
				10,
				new EveryBeatStrategy()
			},
			{
				11,
				new EveryBeatStrategy()
			},
			{
				12,
				new EveryBeatStrategy()
			},
			{
				13,
				new EveryBeatStrategy()
			},
			{
				14,
				new EveryBeatStrategy()
			},
			{
				15,
				new EveryBeatStrategy()
			},
			{
				16,
				new EveryBeatStrategy()
			}
		};

		foreach (KeyValuePair<int, IRhythmGeneratorStrategy> strat in strats)
		{
			strat.Value.SymbolBag = symbolBag;
		}

		return strats;
	}
}
