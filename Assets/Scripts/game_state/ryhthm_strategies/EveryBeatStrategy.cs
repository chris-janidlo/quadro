using System.Collections.Generic;

public class EveryBeatStrategy : IRhythmGeneratorStrategy
{
	public NoteSymbolBag SymbolBag { get; set; }

	public Beat GetNextBeat (int positionInMeasure)
	{
		return new Beat(new Note(positionInMeasure, SymbolBag.GetNext()));
	}

	public void ClearPositionState () {}
}
