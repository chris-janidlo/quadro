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

    public double Latency;

    int beatTicker, cardSpawnTicker;
    bool closestBeatAttempted, failedDuringLatestCard, handledEndOfBeat, shouldSpawnCard, shouldUpCombo;

    // needs to be updated by external driver
    double _audioTime;
    public double AudioTime
    {
        get => _audioTime;
        set
        {
            _audioTime = value;

            audioTimeDidUpdate();
        }
    }

    public int ComboCounter { get; private set; }

    public double CurrentBeatPosition => (AudioTime - Latency) / secondsPerBeat;
    public double CurrentPositionWithinMeasure => CurrentBeatPosition % Track.BEATS_PER_MEASURE;

    int closestPositionWithinMeasure => (int) Math.Round(CurrentPositionWithinMeasure);
    int previousPositionWithinMeasure => (int) CurrentPositionWithinMeasure;

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
            if (Math.Abs(CurrentBeatPosition - (int) CurrentBeatPosition) > SUCCESS_RANGE_BEATS) return false;

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
        if (CurrentBeatPosition > beatTicker + 1)
        {
            beatTicker++;
            Beat?.Invoke();
        }

        if (AudioTime > (Math.Floor(CurrentBeatPosition) + SUCCESS_RANGE_BEATS) * secondsPerBeat)
        {
            if (!handledEndOfBeat)
            {
                handledEndOfBeat = true;

                // if the player completely skipped this beat when they shouldn't have, fail
                if (beatIsOn(previousPositionWithinMeasure) && !closestBeatAttempted)
                {
                    FailCombo();
                }

                closestBeatAttempted = false;

                if (previousPositionWithinMeasure == Track.BEATS_PER_MEASURE - 1)
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
