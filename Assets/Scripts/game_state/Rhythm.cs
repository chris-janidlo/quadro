using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm
{
	public event Action Beat;

    // the percentage of a beat you can "miss" by but still register as a hit. always assumed to be less than half a beat - making it more than a half beat makes no sense if beats can be on right next to each other, and the code isn't written with the potential for exactly half a beat in mind. 0.5 might work, but there might be subtle bugs, so it's highly not recommended.
    public const double SUCCESS_RANGE_BEATS = 0.49;

    public readonly Track Track = new Track();

    public double Latency; // TODO: use this

    int beatTicker, cardSpawnTicker;
    bool closestBeatAttempted, failedDuringLatestCard, handledEndOfBeat, shouldSpawnCard, shouldUpCombo;

    // needs to be updated by external driver
    double _beatPos;
    public double CurrentPositionWithinMeasure
    {
        get => _beatPos;
        set
        {
            if (value < 0 || value >= Track.BEATS_PER_MEASURE)
            {
                throw new ArgumentException("value must be between 0 and " + Track.BEATS_PER_MEASURE);
            }

            _beatPos = value;
            audioTimeDidUpdate();
        }
    }

    public int ComboCounter { get; private set; }
    public int TruncatedPositionWithinMeasure => (int) CurrentPositionWithinMeasure;

    int closestPositionWithinMeasure => (int) Math.Round(CurrentPositionWithinMeasure);

    double secondsPerBeat => 60.0 / Track.BPM;

    public Rhythm ()
    {
        Beat += trackCardSpawning;
    }

    public bool TryHitNow ()
    {
        Func<bool> checkHitInternal = () =>
        {
            if (closestBeatAttempted) return false;

            closestBeatAttempted = true;

            // is this even a valid beat
            if (!beatIsOn(closestPositionWithinMeasure)) return false;

            // if we're out of range
            if (Math.Abs(CurrentPositionWithinMeasure - (int) CurrentPositionWithinMeasure) > SUCCESS_RANGE_BEATS) return false;

            return true;
        };

        bool passed = checkHitInternal();

        if (passed) shouldUpCombo = true;
        else FailCombo();

        return passed;
    }

    public bool IsDownbeat ()
    {
        return closestPositionWithinMeasure == 0;
    }

    public void FailCombo ()
    {
        failedDuringLatestCard = true;
        ComboCounter = 0;
    }

    void audioTimeDidUpdate ()
    {
        if (TruncatedPositionWithinMeasure != beatTicker)
        {
            if (CurrentPositionWithinMeasure >= beatTicker + 1) beatTicker++; // tick
            else if (TruncatedPositionWithinMeasure == 0) beatTicker = 0; // loop
            else throw new InvalidOperationException("something fucky with beats");

            Beat?.Invoke();
        }

        if (CurrentPositionWithinMeasure - (int) CurrentPositionWithinMeasure > SUCCESS_RANGE_BEATS)
        {
            if (!handledEndOfBeat)
            {
                handledEndOfBeat = true;

                // if the player completely skipped this beat when they shouldn't have, fail
                if (beatIsOn(TruncatedPositionWithinMeasure) && !closestBeatAttempted)
                {
                    FailCombo();
                }

                closestBeatAttempted = false;

                if (TruncatedPositionWithinMeasure == Track.BEATS_PER_MEASURE - 1)
                {
                    if (failedDuringLatestCard) Track.FailCard();
                    else Track.ClearCards(1);

                    failedDuringLatestCard = false;
                }
            }
        }
        else
        {
            handledEndOfBeat = false;
        }

        if (shouldUpCombo)
        {
            shouldUpCombo = false;
            ComboCounter++;
        }

        if (shouldSpawnCard)
        {
            shouldSpawnCard = false;
            Track.SpawnCards(1);
        }
    }

    bool beatIsOn (int positionWithinMeasure)
    {
        if (Track.Cards.Count == 0) return false;

        return Track.Cards[0][positionWithinMeasure];
    }

    void trackCardSpawning ()
    {
        cardSpawnTicker++;

        if (cardSpawnTicker >= Track.BeatsPerCard)
        {
            shouldSpawnCard = true;
            cardSpawnTicker = 0;
        }
    }
}
