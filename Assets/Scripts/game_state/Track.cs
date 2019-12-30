using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Track
{
    public const int CARDS_PER_DIFFICULTY_INCREASE = 16; // every time we clear this many cards, increase the BPM and the card spawn rate

    public const int QUAVERS_PER_BEAT = 4;

    // a bstep is a made up metrical unit for scaling purposes (since a change of a single BPM is too subtle to be noticed)
    public const int BPM_PER_BSTEP = 5;
    public const int MIN_BSTEPS = 8;
    public const int MAX_BSTEPS = 28;
    public const int STARTING_BSTEPS = 12;

    public const int MAX_BEATS_PER_CARD = 16;
    public const int STARTING_BEATS_PER_CARD = 4;

    List<RhythmCard> cards = new List<RhythmCard>();
    RhythmCardGenerator generator = new RhythmCardGenerator(QUAVERS_PER_BEAT);

    int _bSteps = STARTING_BSTEPS;
    public int BSteps
    {
        get => _bSteps;
        set => _bSteps = Mathf.Clamp(value, MIN_BSTEPS, MAX_BSTEPS);
    }

    int _cardSpawnRate = STARTING_BEATS_PER_CARD;
    public int CardSpawnRate
    {
        get => _cardSpawnRate;
        set => _cardSpawnRate = Mathf.Clamp(value, 1, MAX_BEATS_PER_CARD);
    }

    public int CardsCleared { get; private set; }

    public int BPM => BSteps * BPM_PER_BSTEP;
    public double SecondsPerBeat => 60.0 / BPM;

    public ReadOnlyCollection<RhythmCard> Cards => cards.AsReadOnly();

    public void SpawnCards (int n)
    {
        throw new NotImplementedException();
    }

    public void ClearCards (int n)
    {
        throw new NotImplementedException();
    }
}
