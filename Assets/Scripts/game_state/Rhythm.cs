using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm
{
	public event Action Beat;

    // the percentage of a beat you can "miss" by but still register as a hit. always assumed to be less than half a beat - making it more than a half beat makes no sense if beats can be on right next to each other, and the code isn't written with the potential for exactly half a beat in mind. 0.5 might work, but there might be subtle bugs, so it's highly not recommended.
    public const double SUCCESS_RANGE_BEATS = 0.3;

    public readonly Track Track = new Track();

    public double Latency; // TODO: use this

    int beatTicker;
    bool closestBeatAttempted, failedDuringLatestCard, handledEndOfBeat;

    // needs to be updated by external driver
    double _beatPos;
    public double CurrentPositionInMeasure
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
    public int TruncatedPositionInMeasure => (int) CurrentPositionInMeasure;

    // may be outside of measure, ie when CurrentPositionInMeasure > Track.BEATS_PER_MEASURE + 0.5
    int closestBeatPosition => (int) Math.Round(CurrentPositionInMeasure);

    public bool TryHitNow ()
    {
        Func<bool> checkHitInternal = () =>
        {
            if (closestBeatAttempted) return false;

            closestBeatAttempted = true;

            // is this even a valid beat
            if (Track.CurrentCardAtBeat(closestBeatPosition % Track.BEATS_PER_MEASURE) == null) return false;

            // if we're out of range
            if (Math.Abs(CurrentPositionInMeasure - closestBeatPosition) > SUCCESS_RANGE_BEATS) return false;

            return true;
        };

        bool passed = checkHitInternal();

        if (passed) ComboCounter++;
        else FailComboAndCard();

        return passed;
    }

    public bool IsDownbeat ()
    {
        return closestBeatPosition == 0;
    }

    public void FailComboAndCard ()
    {
        ComboCounter = 0;
        FailCard();
    }

    public void FailCard ()
    {
        failedDuringLatestCard = true;
    }

    void audioTimeDidUpdate ()
    {
        if (TruncatedPositionInMeasure != beatTicker)
        {
            if (CurrentPositionInMeasure >= beatTicker + 1) beatTicker++; // tick
            else if (TruncatedPositionInMeasure == 0) beatTicker = 0; // loop
            else throw new InvalidOperationException("something fucky with beats");

            updateCardState();

            Beat?.Invoke();
        }

        double fractionalPart = CurrentPositionInMeasure - TruncatedPositionInMeasure;

        if (fractionalPart > SUCCESS_RANGE_BEATS && !handledEndOfBeat)
        {
            handledEndOfBeat = true;

            // if the player completely skipped this beat when they shouldn't have, fail
            if (Track.CurrentCardAtBeat(TruncatedPositionInMeasure) != null && !closestBeatAttempted) FailComboAndCard();

            closestBeatAttempted = false;
        }
        else if (fractionalPart <= SUCCESS_RANGE_BEATS)
        {
            handledEndOfBeat = false;
        }
    }

    void updateCardState ()
    {
        if (TruncatedPositionInMeasure != 0) return;

        RhythmCard failedCard = null;

        if (failedDuringLatestCard) failedCard = Track.RemoveFailedCard();
        else Track.ClearCards(1);

        failedDuringLatestCard = false;

        Track.SpawnCards(Track.CardsPerSpawn);

        if (failedCard != null) Track.RespawnFailedCard(failedCard);
    }
}
