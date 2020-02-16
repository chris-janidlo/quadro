using System;

public class EveryBeatStrategy : IRhythmGeneratorStrategy
{
	public Random Random { get; set; }

	public PositionChunk GetPositionsForNextBeat (int positionInMeasure)
	{
		return new PositionChunk((float) positionInMeasure);
	}

	public void ClearPositionState () {}
}
