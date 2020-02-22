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
	One, Two, Four, Five
}

public static class NoteSymbolExtentions
{
	public static int NUM_SYMBOLS => Enum.GetNames(typeof(NoteSymbol)).Length;

	public static char ToRadixRepresentation (this NoteSymbol ns) =>
		"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"[(int) ns];

	public static char ToRadixRepresentation (this NoteSymbol? ns) =>
		ns == null ? ' ' : ToRadixRepresentation(ns.Value);
	
	public static NoteSymbol Next (this NoteSymbol ns) => (NoteSymbol) (((int) ns + 1) % NUM_SYMBOLS);

	public static NoteSymbol Previous (this NoteSymbol ns)
	{
		int prev = (int) ns - 1;

		if (prev < 0) prev = NUM_SYMBOLS - 1;

		return (NoteSymbol) prev;
	}

	public static bool HasInChord (this NoteSymbol ns, ComInput ci)
	{
		switch (ns)
		{
			case NoteSymbol.One: return new List<ComInput> { ComInput.C, ComInput.E, ComInput.G }.Contains(ci);
			case NoteSymbol.Two: return new List<ComInput> { ComInput.D, ComInput.F, ComInput.A }.Contains(ci);
			case NoteSymbol.Four: return new List<ComInput> { ComInput.F, ComInput.A, ComInput.C }.Contains(ci);
			case NoteSymbol.Five: return new List<ComInput> { ComInput.G, ComInput.B, ComInput.D }.Contains(ci);
			default: throw new ArgumentException("unexpected NoteSymbol " + ns);
		}
	}
}
