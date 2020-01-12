using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME:
public class TurnBasedDriver : ADriver
{
    void Awake ()
    {
        Initialize(new BaseSingleplayerDiamond());
        fastForward();
    }

    void Update ()
    {
        var input = getInput();

        if (input != null)
        {
            State.DoNoteInput((NoteInput) input);
            fastForward();
        }
    }

    void fastForward ()
    {
        do
        {
            State.Rhythm.CurrentPositionInMeasure++;
            State.Rhythm.CurrentPositionInMeasure %= Track.BEATS_PER_MEASURE;
        }
        while (State.Track.CurrentCardAtBeat(State.Rhythm.TruncatedPositionInMeasure) == null);
    }
}
