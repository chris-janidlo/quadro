using System;
using System.Collections.Generic;

public class Note
{
	public readonly NoteSymbol Symbol;
	public readonly double PositionInMeasure;

	public double BeatsUntilThisNote => beatTicker + PositionInBeat - track.CurrentPositionInBeat;
	public double PositionInBeat => PositionInMeasure - (int) PositionInMeasure;

	Track track;
	int beatTicker = Track.BEATS_SHOWN_IN_ADVANCE;

	public Note (Track track, double positionInMeasure, NoteSymbol symbol)
	{
		if (positionInMeasure < 0 || positionInMeasure >= Track.BEATS_PER_MEASURE)
		{
			throw new ArgumentException($"note position must be in range [0, {Track.BEATS_PER_MEASURE}) (was given {positionInMeasure})");
		}

		this.track = track;
		PositionInMeasure = positionInMeasure;
		Symbol = symbol;

		track.Beat += () => beatTicker--;
	}
}

public enum NoteSymbol
{
	C, F, G7, a7
}

public static class NoteSymbolExtentions
{
	public static int NUM_SYMBOLS => Enum.GetNames(typeof(NoteSymbol)).Length;
	
	public static NoteSymbol Next (this NoteSymbol ns) => (NoteSymbol) (((int) ns + 1) % NUM_SYMBOLS);

	public static NoteSymbol Previous (this NoteSymbol ns)
	{
		int prev = (int) ns - 1;

		if (prev < 0) prev = NUM_SYMBOLS - 1;

		return (NoteSymbol) prev;
	}
}
