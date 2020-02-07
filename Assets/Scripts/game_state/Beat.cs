using System;
using System.Collections.Generic;

// a beat is a collection of notes based on a single integer measure position.
// if that position is p, then the position of every note in the beat is guaranteed to be in range [p, p+1).
public class Beat
{
	public int? BasePosition { get; private set; }
	public IReadOnlyList<Note> Notes => notes.AsReadOnly();

	List<Note> notes = new List<Note>();

	public Beat () {}

	public Beat (int basePosition)
	{
		BasePosition = basePosition;
	}

	public Beat (Note note)
	{
		AddNote(note);
	}

	public Beat (IEnumerable<Note> notes)
	{
		foreach (Note note in notes)
		{
			AddNote(note);
		}
	}

	public void AddNote (Note note)
	{
		if (note == null) throw new ArgumentException("can't add null Note");

		if (BasePosition == null)
		{
			BasePosition = (int) note.PositionInMeasure;
		}
		else if ((int) note.PositionInMeasure != BasePosition.Value)
		{
			throw new ArgumentException($"given Note's position ({note.PositionInMeasure}) does not match this Beat's base position ({BasePosition.Value})");
		}

		notes.Add(note);
	}
}
