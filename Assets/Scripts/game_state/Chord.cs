using System;

public enum Chord
{
	C, F, G7, a7
}

public static class ChordExtentions // not to be confused with extension chords
{
	public static int NUM_SYMBOLS => Enum.GetNames(typeof(Chord)).Length;
	
	public static Chord Next (this Chord ns) => (Chord) (((int) ns + 1) % NUM_SYMBOLS);

	public static Chord Previous (this Chord ns)
	{
		int prev = (int) ns - 1;

		if (prev < 0) prev = NUM_SYMBOLS - 1;

		return (Chord) prev;
	}
}
