using System;

public enum BeatSymbol
{
	Zero, One, Two, Three
}

public static class BeatSymbolExtensions
{
	public static int NUM_SYMBOLS => Enum.GetNames(typeof(BeatSymbol)).Length;

	public static char ToRadixRepresentation (this BeatSymbol bs) =>
		"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"[(int) bs];

	public static char ToRadixRepresentation (this BeatSymbol? bs) =>
		bs == null ? ' ' : ToRadixRepresentation((BeatSymbol) bs);
	
	public static BeatSymbol Next (this BeatSymbol bs) => (BeatSymbol) (((int) bs + 1) % NUM_SYMBOLS);

	public static BeatSymbol Previous (this BeatSymbol bs)
	{
		int prev = (int) bs - 1;

		if (prev < 0) prev = NUM_SYMBOLS - 1;

		return (BeatSymbol) prev;
	}
}
