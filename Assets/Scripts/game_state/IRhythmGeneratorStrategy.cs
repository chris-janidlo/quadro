using System;

public interface IRhythmGeneratorStrategy
{
	NoteSymbolBag SymbolBag { get; set; }
	Random Random { get; set; }

	Beat GetNextBeat (int positionInMeasure);
	void ClearPositionState ();
}
