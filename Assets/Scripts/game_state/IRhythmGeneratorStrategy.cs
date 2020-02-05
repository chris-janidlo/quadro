using System.Collections.Generic;

public interface IRhythmGeneratorStrategy
{
	NoteSymbolBag SymbolBag { get; set; }

	Beat GetNextBeat (int positionInMeasure);
	void ClearPositionState ();
}
