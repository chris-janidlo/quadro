using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm
{
	public event Action Beat;
    public event Action<HitData> Hit;

    public readonly Track Track = new Track();

    public double Latency; // TODO: use this

    int beatTicker;
    bool closestBeatAttempted, handledEndOfBeat;

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
    public int ClosestPositionInMeasure => closestBeatPosition % Track.BEATS_PER_MEASURE;
    public double FractionalPartOfPosition => CurrentPositionInMeasure - TruncatedPositionInMeasure;

    // may be outside of measure, ie when CurrentPositionInMeasure > Track.BEATS_PER_MEASURE + 0.5
    int closestBeatPosition => (int) Math.Round(CurrentPositionInMeasure);

    public HitData TryHitNow ()
    {
        HitData hit = null;
        double hitDistance = Math.Abs(CurrentPositionInMeasure - closestBeatPosition);

        if (closestBeatAttempted)
            hit = new HitData(hitDistance, MissReasonEnum.AlreadyAttemptedBeat);

        closestBeatAttempted = true;

        if (hit == null && Track.CurrentCardAtBeat(ClosestPositionInMeasure) == null)
            hit = new HitData(hitDistance, MissReasonEnum.ClosestBeatIsOff);

        if (hit == null)
            hit = new HitData(hitDistance);

        if (hit.IsSuccessful) ComboCounter++;
        else FailComboAndCard();

        Hit?.Invoke(hit);

        return hit;
    }

    public bool IsDownbeat ()
    {
        return closestBeatPosition == 0;
    }

    public void FailComboAndCard ()
    {
        ComboCounter = 0;
        Track.FailCurrentCard();
    }

    void audioTimeDidUpdate ()
    {
        if (TruncatedPositionInMeasure != beatTicker)
        {
            if (CurrentPositionInMeasure >= beatTicker + 1) beatTicker++; // tick
            else if (TruncatedPositionInMeasure == 0) beatTicker = 0; // loop
            else throw new InvalidOperationException("something fucky with beats");

            Beat?.Invoke();

            if (TruncatedPositionInMeasure == 0) Track.HandleEndOfMeasure();
        }

        if (FractionalPartOfPosition > HitQuality.Miss.BeatDistanceRange().x && !handledEndOfBeat)
        {
            handledEndOfBeat = true;

            // if the player completely skipped this beat when they shouldn't have, fail
            if (Track.CurrentCardAtBeat(TruncatedPositionInMeasure) != null && !closestBeatAttempted)
            {
                Hit?.Invoke(new HitData(FractionalPartOfPosition, MissReasonEnum.NeverAttemptedBeat));
                FailComboAndCard();
            }

            closestBeatAttempted = false;
        }
        else if (FractionalPartOfPosition <= HitQuality.Miss.BeatDistanceRange().x)
        {
            handledEndOfBeat = false;
        }
    }
}
