using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using crass;

public class Track
{
    public event Action CardAdded, CardRemoved;

    public const int CARDS_UNTIL_DEAD = 32;
    public const int BEATS_PER_MEASURE = 4; // also the subdivisions in every card

    public const int CARDS_PER_DIFFICULTY_INCREASE = 8; // every time we clear this many cards, increase the BPM and the card spawn rate

    // a bstep is a made up metrical unit for scaling purposes (since a change of a single BPM is too subtle to be noticed)
    public const int BPM_PER_BSTEP = 5;
    public const int MIN_BSTEPS = 12;
    public const int MAX_BSTEPS = 40;
    public const int STARTING_BSTEPS = 16;

    public const int MAX_CARDS_PER_SPAWN = 16;
    public const int STARTING_CARDS_PER_SPAWN = 1;

    List<RhythmCard> cards = new List<RhythmCard>();
    RhythmCardGenerator generator;

    int _bSteps = STARTING_BSTEPS;
    public int BSteps
    {
        get => _bSteps;
        set => _bSteps = Mathf.Clamp(value, MIN_BSTEPS, MAX_BSTEPS);
    }

    int _cardsPerSpawn = STARTING_CARDS_PER_SPAWN;
    public int CardsPerSpawn
    {
        get => _cardsPerSpawn;
        set => _cardsPerSpawn = Mathf.Clamp(value, 1, MAX_CARDS_PER_SPAWN);
    }

    bool _failedLastCard;
    public bool FailedLastCard
    {
        get => _failedLastCard;
        private set
        {
            if (value && InDanger)
            {
                Dead = true;
            }

            _failedLastCard = value;
        }
    }

    public RhythmCard NextToSpawn => generator.Peek;

    public int CardsCleared { get; private set; }
    public bool Dead { get; private set; }

    public int BPM => BSteps * BPM_PER_BSTEP;
    public ReadOnlyCollection<RhythmCard> Cards => cards.AsReadOnly();

    public bool InDanger => cards.Count > CARDS_UNTIL_DEAD;

    public Track ()
    {
        generator = new RhythmCardGenerator(BEATS_PER_MEASURE);
    }

    public Track (int randomSeed)
    {
        generator = new RhythmCardGenerator(BEATS_PER_MEASURE, randomSeed);
    }

    public void SpawnCards (int numCards)
    {
        if (numCards != 0)
        {
            FailedLastCard = false;
        }

        for (int i = 0; i < numCards; i++)
        {
            cards.Add(generator.GetNext());
            CardAdded?.Invoke();
        }
    }

    public void ClearCards (int numCards)
    {
        if (numCards != 0)
        {
            FailedLastCard = false;
        }

        for (int i = 0; i < numCards; i++)
        {
            if (cards.Count == 0) break;

            cards.RemoveAt(0);
            CardRemoved?.Invoke();

            CardsCleared++;
            if (CardsCleared % CARDS_PER_DIFFICULTY_INCREASE == 0)
            {
                BSteps++;
                CardsPerSpawn++;
            }
        }
    }

    public RhythmCard RemoveFailedCard ()
    {
        RhythmCard card = cards[0];

        cards.RemoveAt(0);
        CardRemoved?.Invoke();

        return card;
    }

    public void RespawnFailedCard (RhythmCard card)
    {
        cards.Add(card);
        CardAdded?.Invoke();

        FailedLastCard = true;
    }

    public BeatSymbol? CurrentCardAtBeat (int positionWithinMeasure)
    {
        if (positionWithinMeasure < 0 || positionWithinMeasure >= BEATS_PER_MEASURE)
        {
            throw new ArgumentException("beat position must be within 0 and " + BEATS_PER_MEASURE);
        }

        if (Cards.Count == 0) return null;

        return Cards[0][positionWithinMeasure];
    }
}
