using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using crass;

public class Track
{
    public event Action HandledEndOfMeasure, HandledMiddleOfMeasure;
    public event Action CardsBatchUpdated, CardAdded, CardRemoved;

    public const int CARDS_UNTIL_DEAD = 16;
    public const int BEATS_PER_MEASURE = 4; // also the subdivisions in every card

    public const int CARDS_PER_DIFFICULTY_INCREASE = 8; // every time we clear this many cards, increase the BPM and the card spawn rate

    // a bstep is a made up metrical unit for scaling purposes (since a change of a single BPM is too subtle to be noticed)
    public const int BPM_PER_BSTEP = 10;
    public const int MIN_BSTEPS = 4;
    public const int MAX_BSTEPS = 20;
    public const int STARTING_BSTEPS = 8;

    public const float CARD_SPAWN_INCREASE_PER_DIFFICULTY_UP = 0.5f;
    public const float MAX_CARD_SPAWN_RATE = 8;
    public const float MIN_CARD_SPAWN_RATE = 0.5f;
    public const float STARTING_CARD_SPAWN_RATE = 0.5f;

    List<RhythmCard> cards = new List<RhythmCard>();
    RhythmCardGenerator generator;

    float _cardDelta = STARTING_CARD_SPAWN_RATE;
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

    float _cardSpawnRate = STARTING_CARD_SPAWN_RATE;
    public float CardSpawnRate
    {
        get => _cardSpawnRate;
        set => _cardSpawnRate = Mathf.Clamp(value, MIN_CARD_SPAWN_RATE, MAX_CARD_SPAWN_RATE);
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

        if (Cards.Count != 0)
        {
            if (FailedCurrentCard)
            {
                failedCard = removeCard(false);
            }
            else
            {
                removeCard();
            }
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
            }

            changeCounter--;
        }

        if (failedCard != null) addCard(failedCard);

        CardDelta -= (int) CardDelta;

        CardsBatchUpdated?.Invoke();

        FailedLastCard = FailedCurrentCard;
        FailedCurrentCard = false;

        HandledEndOfMeasure?.Invoke();
    }

	public void HandleMiddleOfMeasure ()
	{
		CardDelta += CardSpawnRate;
        HandledMiddleOfMeasure?.Invoke();
	}

	public void FailCurrentCard ()
    {
        FailedCurrentCard = true;
    }

    public NoteSymbol? CurrentCardAtBeat (int positionWithinMeasure)
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

    RhythmCard removeCard (bool countsTowardsClear = true)
    {
        var card = cards[0];

        cards.RemoveAt(0);

        if (countsTowardsClear)
        {
            CardsCleared++;
            if (CardsCleared % CARDS_PER_DIFFICULTY_INCREASE == 0)
            {
                BSteps++;
                CardSpawnRate += CARD_SPAWN_INCREASE_PER_DIFFICULTY_UP;
            }
        }

        CardRemoved?.Invoke();

        return card;
    }
}
