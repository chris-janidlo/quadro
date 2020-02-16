using System;
using System.Linq;
using System.Collections.Generic;

public class RhythmGenerator
{
	public const int MIN_DIFFICULTY = 1, MAX_DIFFICULTY = 16;

	readonly IReadOnlyDictionary<int, IRhythmGeneratorStrategy> strategies;
	int previousDifficulty;
	bool difficultyNotInitialized = true;
	
	int beatsPerMeasure;
	Track track;
	Random random;
	NoteSymbolBag symbolBag;

	public RhythmGenerator (Track track, int beatsPerMeasure, int randomSeed)
	{
		this.track = track;
		this.beatsPerMeasure = beatsPerMeasure;

		random = new Random(randomSeed);
		symbolBag = new NoteSymbolBag(random);

		strategies = initializeStrategies();
	}

	public RhythmGenerator (Track track, int beatsPerMeasure) : this(track, beatsPerMeasure, Environment.TickCount) {}

	// TODO: inner-beat patterns: eighth notes, sixteenth notes, triplets
	// TODO: multi-beat patterns: broken triplets, 4 against 3, any other arbitrary pattern
	public NoteChunk GetNextBeat (int positionInMeasure, int difficulty)
	{
		if (difficultyNotInitialized)
		{
			previousDifficulty = difficulty;
			difficultyNotInitialized = false;
		}

		if (difficulty < MIN_DIFFICULTY || difficulty > MAX_DIFFICULTY)
			throw new ArgumentException($"difficulty must be in range ({MIN_DIFFICULTY}, {MAX_DIFFICULTY}); was given {difficulty}");

		int prev = previousDifficulty;
		previousDifficulty = difficulty;

		NoteChunk beat = new NoteChunk(positionInMeasure);

		if (difficulty != prev)
		{
			strategies[prev].ClearPositionState();
		}
		else
		{
			PositionChunk next = strategies[difficulty].GetPositionsForNextBeat(positionInMeasure);
			beat.AddValues(next.Values.Select(p => new Note(track, p, symbolBag.GetNext())));
		}

		return beat;
	}

	Dictionary<int, IRhythmGeneratorStrategy> initializeStrategies ()
	{
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
			strat.Value.Random = random;

		return strats;
	}
}
