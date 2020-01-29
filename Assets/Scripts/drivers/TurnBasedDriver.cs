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
        while (!Player.Track.Dead)
        {
            while (Player.Rhythm.TruncatedPositionInMeasure == lastPositionInMeasure || Player.Track.CurrentCardAtBeat(Player.Rhythm.TruncatedPositionInMeasure) == null)
            {
                addToMeasurePosition(Time.deltaTime);
                yield return null;
            }

            CommandInput? input;

            do
            {
                input = getInput();
                yield return null;
            }
            while (input == null);

            Player.DoCommandInput(input.Value);

            lastPositionInMeasure = Player.Rhythm.TruncatedPositionInMeasure;
        }
    }

    void addToMeasurePosition (double amount)
    {
        Player.Rhythm.CurrentPositionInMeasure = (Player.Rhythm.CurrentPositionInMeasure + amount) % Track.BEATS_PER_MEASURE;
    }
}
