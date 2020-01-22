using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using crass;

public class Track
{
    public event Action CardsBatchUpdated, CardAdded, CardRemoved;

    public const int CARDS_UNTIL_DEAD = 16;
    public const int BEATS_PER_MEASURE = 4; // also the subdivisions in every card

    public const int CARDS_PER_DIFFICULTY_INCREASE = 8; // every time we clear this many cards, increase the BPM and the card spawn rate

    // a bstep is a made up metrical unit for scaling purposes (since a change of a single BPM is too subtle to be noticed)
    public const int BPM_PER_BSTEP = 5;
    public const int MIN_BSTEPS = 12;
    public const int MAX_BSTEPS = 40;
    public const int STARTING_BSTEPS = 16;

    public const float MAX_CARD_SPAWN_RATE = 4;
    public const float MIN_CARD_SPAWN_RATE = 0.5f;
    public const float STARTING_CARD_SPAWN_RATE = 0.5f;

    List<RhythmCard> cards = new List<RhythmCard>();
    RhythmCardGenerator generator;

    float _cardDelta;
    public float CardDelta
    {
        get => _cardDelta;
        set => _cardDelta = Mathf.Clamp(value, -CARDS_UNTIL_DEAD, CARDS_UNTIL_DEAD);
    }

    int _bSteps = STARTING_BSTEPS;
    public int BSteps
    {
        get => _bSteps;
        set => _bSteps = Mathf.Clamp(value, MIN_BSTEPS, MAX_BSTEPS);
    }

    float _cardsPerSpawn = STARTING_CARD_SPAWN_RATE;
    public float CardsPerSpawn
    {
        get => _cardsPerSpawn;
        set => _cardsPerSpawn = Mathf.Clamp(value, MIN_CARD_SPAWN_RATE, MAX_CARD_SPAWN_RATE);
    }

    public bool FailedCurrentCard { get; private set; }

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

    public void HandleEndOfMeasure ()
    {
        int changeCounter = (int) Mathf.Abs(CardDelta);
        RhythmCard failedCard = null;

        if (FailedCurrentCard)
        {
            failedCard = removeCard();
        }
        else if (Cards.Count != 0)
        {
            removeCard();
            CardsCleared++;
        }

        while (changeCounter > 0)
        {
            if (CardDelta > 0)
            {
                addCard(generator.GetNext());
            }
            else
            {
                if (cards.Count == 0) break;

                removeCard();
                CardsCleared++;
            }

            changeCounter--;
        }

        if (failedCard != null) addCard(failedCard);

        CardDelta -= (int) CardDelta;

        CardDelta += CardsPerSpawn;

        CardsBatchUpdated?.Invoke();

        FailedLastCard = FailedCurrentCard;
        FailedCurrentCard = false;
    }

    public void FailCurrentCard ()
    {
        FailedCurrentCard = true;
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

    void addCard (RhythmCard card)
    {
        cards.Add(card);
        CardAdded?.Invoke();
    }

    RhythmCard removeCard ()
    {
        var card = cards[0];

        cards.RemoveAt(0);
        CardRemoved?.Invoke();

        return card;
    }
}
