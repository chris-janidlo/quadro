using System;

public class Note
{
	public readonly NoteSymbol Symbol;
	public readonly double PositionInBeat;

	public int BeatTicker;

	public double BeatsUntilThisNote => BeatTicker + PositionInBeat;

	public Note (double positionInBeat, NoteSymbol symbol)
	{
		if (positionInBeat < 0 || positionInBeat >= 1)
		{
			throw new ArgumentException($"note position must be in range [0, 1) (was given {positionInBeat})");
		}

		PositionInBeat = positionInBeat;
		Symbol = symbol;
	}
}

public enum NoteSymbol
{
	Zero, One, Two, Three
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
}
