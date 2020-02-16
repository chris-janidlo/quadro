using System;
using System.Collections.Generic;

// a beatchunk is a collection of objects based on a single integer measure position
// if that position is p, then the position of every object in the beat is guaranteed to be in range [p, p+1)
public abstract class BeatChunk<T>
{
	public int BasePosition { get; private set; }
	public IReadOnlyList<T> Values => values.AsReadOnly();

	List<T> values = new List<T>();

	public BeatChunk (int basePosition)
	{
		BasePosition = basePosition;
	}

	public BeatChunk (T value)
	{
		BasePosition = getBasePositionFromObject(value);
		AddValue(value);
	}

	public BeatChunk (IList<T> values)
	{
		BasePosition = getBasePositionFromObject(values[0]);

		foreach (T value in values)
		{
			AddValue(value);
		}
	}

	public void AddValue (T value)
	{
		if (value == null) throw new ArgumentException("can't add null Value");

		int basePos = getBasePositionFromObject(value);
		if (basePos != BasePosition)
			throw new ArgumentException($"given Value's position ({basePos}) does not match this Beat's base position ({BasePosition})");

		values.Add(value);
	}

	public void AddValues (IEnumerable<T> values)
	{
		foreach (T value in values)
		{
			AddValue(value);
		}
	}

	protected abstract int getBasePositionFromObject (T obj);
}

public class PositionChunk : BeatChunk<float>
{
	public PositionChunk (int basePosition) : base(basePosition) {}
	public PositionChunk (float value) : base(value) {}
	public PositionChunk (IList<float> values) : base(values) {}

	protected override int getBasePositionFromObject (float obj) => (int) obj;
}

public class NoteChunk : BeatChunk<Note>
{
	public NoteChunk (int basePosition) : base(basePosition) {}
	public NoteChunk (Note value) : base(value) {}
	public NoteChunk (IList<Note> values) : base(values) {}

	protected override int getBasePositionFromObject (Note obj) => (int) obj.PositionInMeasure;
}
