using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm
{
	public event Action Quaver, Beat;

    // the percentage of a quaver you can "miss" by but still register as a hit. always assumed to be less than half a quaver - making it more than a half quaver makes no sense if quavers can be on right next to each other, and the code isn't written with the potential for exactly half a quaver in mind. 0.5 might work, but there might be subtle bugs, so it's highly not recommended.
    public const double SUCCESS_RANGE_QUAVERS = 0.2;

    public readonly Track Track = new Track();

    public double Latency;

    int quaverTicker, beatTicker;
    bool closestQuaverAttempted, failedDuringLatestCard;

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

    public double CurrentQuaverPosition => (AudioTime - Latency) / (secondsPerBeat / Track.QUAVERS_PER_BEAT);
    public double CurrentBeatPosition => (AudioTime - Latency) / secondsPerBeat;

    int positionWithinBeat => (int) Math.Round(CurrentQuaverPosition - ((int) CurrentBeatPosition * Track.QUAVERS_PER_BEAT));

    double secondsPerBeat => 60.0 / Track.BPM;

    public bool TryHitNow ()
    {
        Func<bool> checkHitInternal = () =>
        {
            if (closestQuaverAttempted) return false;

            closestQuaverAttempted = true;

            // is this even a valid quaver
            if (!Track.Cards[0][positionWithinBeat]) return false;

            // if we're out of range
            if (Math.Abs(CurrentQuaverPosition - (int) CurrentQuaverPosition) > SUCCESS_RANGE_QUAVERS) return false;

            return true;
        };

        bool passed = checkHitInternal();

        if (passed) ComboCounter++;
        else FailCombo();

        return passed;
    }

    public bool IsDownbeat ()
    {
        return positionWithinBeat == 0;
    }

    public void FailCombo ()
    {
        failedDuringLatestCard = true;
        ComboCounter = 0;
    }

    void audioTimeDidUpdate ()
    {
        if (CurrentQuaverPosition > quaverTicker + 1)
        {
            quaverTicker++;
            Quaver?.Invoke();
        }

        if (CurrentBeatPosition > beatTicker + 1)
        {
            beatTicker++;
            Beat?.Invoke();
        }

        if (AudioTime > Math.Floor(CurrentQuaverPosition) + SUCCESS_RANGE_QUAVERS)
        {
            closestQuaverAttempted = false;

            if (positionWithinBeat == Track.QUAVERS_PER_BEAT - 1)
            {
                if (failedDuringLatestCard) Track.FailCard();
                else Track.ClearCards(1);

                failedDuringLatestCard = false;
            }
        }
    }
}
