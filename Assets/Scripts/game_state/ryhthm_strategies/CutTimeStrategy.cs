using System;

// only works for 4/4 time
public class CutTimeStrategy : IRhythmGeneratorStrategy
{
	public NoteSymbolBag SymbolBag { get; set; }
	public Random Random { get; set; }

	public enum NoteLength
	{
		Eighth = -1,
		Quarter = 0,
		Half = 1,
		Whole = 2,
		Breve = 3
	}

	float offbeatChance;
	float noteFrequency;

	int measureTicker;

	public CutTimeStrategy (NoteLength guaranteedNotesBetweenSpawn, float offbeatChance)
	{
		if (offbeatChance < 0 || offbeatChance > 1)
		{
			throw new ArgumentException($"offbeatChance must be a probability in range [0, 1] (was given {offbeatChance})");
		}

		this.offbeatChance = offbeatChance;

		noteFrequency = (float) Math.Pow(2f, (int) guaranteedNotesBetweenSpawn);
	}

	public Beat GetNextBeat (int positionInMeasure)
	{
		if (noteFrequency >= 8)
		{
			return getWholeMeasureNotes(positionInMeasure);
		}
		else if (noteFrequency == 4)
		{
			return getOnceAMeasure(positionInMeasure);
		}
		else if (noteFrequency == 2)
		{
			return getEveryTwoNotes(positionInMeasure);
		}
		else // if (noteFrequency <= 1)
		{
			return getSubBeatNotes(positionInMeasure);
		}
	}

	public void ClearPositionState ()
	{
		measureTicker = 0;
	}

	Beat getWholeMeasureNotes (int positionInMeasure)
	{
		Beat beat = new Beat(positionInMeasure);

		if (positionInMeasure != 0) return beat;

		int measureFrequency = (int) noteFrequency / 4;

		if (measureTicker == 0 || (shouldSpawnOffbeat() && measureTicker == measureFrequency / 2))
		{
			beat.AddNote(new Note(positionInMeasure, SymbolBag.GetNext()));
		}

		measureTicker = (measureTicker + 1) % measureFrequency;

		return beat;
	}

	Beat getOnceAMeasure (int positionInMeasure)
	{
		Beat beat = new Beat(positionInMeasure);

		if (positionInMeasure == 0 || (shouldSpawnOffbeat() && positionInMeasure == 2))
		{
			beat.AddNote(new Note(positionInMeasure, SymbolBag.GetNext()));
		}

		return beat;
	}

	Beat getEveryTwoNotes (int positionInMeasure)
	{
		Beat beat = new Beat(positionInMeasure);

		if (positionInMeasure == 0 || positionInMeasure == 2 || shouldSpawnOffbeat())
		{
			beat.AddNote(new Note(positionInMeasure, SymbolBag.GetNext()));
		}

		return beat;
	}

	Beat getSubBeatNotes (int positionInMeasure)
	{
		Beat beat = new Beat(positionInMeasure);

		float ticker = 0;

		while (ticker < 1)
		{
			if (ticker % noteFrequency == 0 || shouldSpawnOffbeat())
			{
				beat.AddNote(new Note(positionInMeasure + ticker, SymbolBag.GetNext()));
			}

			ticker += noteFrequency / 2;
		}

		return beat;
	}

	bool shouldSpawnOffbeat ()
	{
		return Random.NextDouble() < offbeatChance;
	}
}
