using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME:
public class TurnBasedDriver : ADriver
{
    void Start ()
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
            State.Rhythm.CurrentPositionWithinMeasure++;
            State.Rhythm.CurrentPositionWithinMeasure %= Track.BEATS_PER_MEASURE;
        }
        while (State.Track.FirstCardHasBeat(State.Rhythm.TruncatedPositionWithinMeasure));
    }
}
