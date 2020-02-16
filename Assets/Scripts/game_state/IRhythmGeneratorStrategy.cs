using System;

public interface IRhythmGeneratorStrategy
{
	Random Random { get; set; }

	PositionChunk GetPositionsForNextBeat (int positionInMeasure);
	void ClearPositionState ();
}
