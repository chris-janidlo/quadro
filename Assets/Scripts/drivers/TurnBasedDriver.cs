using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// currently for debugging purposes only; not suitable for public consumption
public class TurnBasedDriver : ADriver
{
    int lastPositionInMeasure = -1;

    void Awake ()
    {
        Initialize(new BaseSingleplayerDiamond());
    }

    IEnumerator Start ()
    {
        while (!State.Track.Dead)
        {
            while (State.Rhythm.TruncatedPositionInMeasure == lastPositionInMeasure || State.Track.CurrentCardAtBeat(State.Rhythm.TruncatedPositionInMeasure) == null)
            {
                addToMeasurePosition(Time.deltaTime);
                yield return null;
            }

            NoteInput? input;

            do
            {
                input = getInput();
                yield return null;
            }
            while (input == null);

            State.DoNoteInput(input.Value);

            lastPositionInMeasure = State.Rhythm.TruncatedPositionInMeasure;
        }
    }

    void addToMeasurePosition (double amount)
    {
        State.Rhythm.CurrentPositionInMeasure = (State.Rhythm.CurrentPositionInMeasure + amount) % Track.BEATS_PER_MEASURE;
    }
}
