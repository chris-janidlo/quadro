using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class RhythmGenerator
{
	public enum NoteSymbolTransition
	{
		Stay, GoToNext, GoToPrevious, JumpNextTwice, JumpPreviousTwice
	}

	// don't use the bag generator in crass for this class, since it doesn't let you seed its rng
	public static readonly IReadOnlyList<NoteSymbolTransition> TransitionBag = new List<NoteSymbolTransition>
	{
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.Stay,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToNext,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.GoToPrevious,
		NoteSymbolTransition.JumpNextTwice,
		NoteSymbolTransition.JumpNextTwice,
		NoteSymbolTransition.JumpPreviousTwice,
		NoteSymbolTransition.JumpPreviousTwice
	}.AsReadOnly();

	Random random; // each card generator gets its own rng for better fine-grained control
	List<NoteSymbolTransition> currentTransitionBag = new List<NoteSymbolTransition>(TransitionBag);

	int beatsPerMeasure;

	NoteSymbol currentSymbol;
	bool[] currentMeasurePattern;
	int positionInMeasurePattern;

	public RhythmGenerator (int beatsPerMeasure, int randomSeed)
	{
		this.beatsPerMeasure = beatsPerMeasure;
		random = new Random(randomSeed);

		var allNoteSymbols = Enum.GetValues(typeof(NoteSymbol));
		currentSymbol = (NoteSymbol) allNoteSymbols.GetValue(random.Next(allNoteSymbols.Length));
	}

	public RhythmGenerator (int beatsPerMeasure) : this(beatsPerMeasure, Environment.TickCount) {}

	// Returns note data for the range [b, b+1) where b is the next un-generated note. Updates internal state as it goes
	// TODO: take difficulty input
	public List<Note> GetNotesForNextBeat ()
	{
		List<Note> notes = new List<Note>();

		// TODO: inner-beat patterns: eighth notes, sixteenth notes, triplets
		// TODO: multi-beat patterns: broken triplets, 4 against 3, any other arbitrary pattern

		if (currentMeasurePattern == null)
		{
			currentMeasurePattern = randomBeatPattern();
			positionInMeasurePattern = 0;
		}

		if (currentMeasurePattern[positionInMeasurePattern])
		{
			notes.Add(new Note(0, currentSymbol));
			applySymbolTransition(getNextSymbolTransition());
		}

		positionInMeasurePattern++;


		if (positionInMeasurePattern >= currentMeasurePattern.Length)
		{
			currentMeasurePattern = null;
			positionInMeasurePattern = 0;
		}

		return notes;
	}

	bool[] randomBeatPattern ()
	{
        int uniqueCardPermutations = (1 << beatsPerMeasure) - 1;
		int index = random.Next(1, uniqueCardPermutations);

        string paddedBinary = Convert.ToString(index, 2).PadLeft(beatsPerMeasure, '0');
        return paddedBinary.Select(c => c == '1').ToArray();
    }

	void applySymbolTransition (NoteSymbolTransition transition)
	{
		switch (transition)
		{
			case NoteSymbolTransition.Stay:
				// do nothing
				break;

			case NoteSymbolTransition.GoToNext:
				currentSymbol = currentSymbol.Next();
				break;

			case NoteSymbolTransition.GoToPrevious:
				currentSymbol = currentSymbol.Previous();
				break;

			case NoteSymbolTransition.JumpNextTwice:
				currentSymbol = currentSymbol.Next().Next();
				break;

			case NoteSymbolTransition.JumpPreviousTwice:
				currentSymbol = currentSymbol.Previous().Previous();
				break;

			default:
				throw new InvalidEnumArgumentException("unexpected NoteSymbolTransition " + transition.ToString());
		}
	}

	NoteSymbolTransition getNextSymbolTransition ()
	{
		int index = random.Next(currentTransitionBag.Count);
		var transition = currentTransitionBag[index];
		currentTransitionBag.RemoveAt(index);

		if (currentTransitionBag.Count == 0) currentTransitionBag = new List<NoteSymbolTransition>(TransitionBag);

		return transition;
	}
}
