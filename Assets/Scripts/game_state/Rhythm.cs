using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm
{
    // metric units
	public event Action Quaver, Beat;

    public readonly Track Track = new Track();

    public double Latency;

    int quaverTicker, beatTicker;

    // needs to be updated by external driver
    double _songTime;
    public double AudioTime
    {
        get => _songTime;
        set
        {
            _songTime = value;

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
        }
    }

    public double CurrentQuaverPosition => (AudioTime - Latency) / (secondsPerBeat / Track.QUAVERS_PER_BEAT);
    public double CurrentBeatPosition => (AudioTime - Latency) / secondsPerBeat;

    double secondsPerBeat => 60.0 / Track.BPM;

    public bool TryHitNow ()
    {
        throw new NotImplementedException();
    }

    public bool IsDownbeat ()
    {
        throw new NotImplementedException();
    }
}
