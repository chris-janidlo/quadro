using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using crass;

public class Track
{
    public const int MEASURES = 8;
    public const int BEATS_PER_MEASURE = 4; // also cards per measure
    public const int QUAVERS_PER_BEAT = 4; // also the subdivisions in every card

    public const int CARDS_PER_DIFFICULTY_INCREASE = 16; // every time we clear this many cards, increase the BPM and the card spawn rate

    // a bstep is a made up metrical unit for scaling purposes (since a change of a single BPM is too subtle to be noticed)
    public const int BPM_PER_BSTEP = 5;
    public const int MIN_BSTEPS = 8;
    public const int MAX_BSTEPS = 28;
    public const int STARTING_BSTEPS = 12;

    public const int MAX_BEATS_PER_CARD = 16;
    public const int STARTING_BEATS_PER_CARD = 4;

    List<RhythmCard> cards = new List<RhythmCard>();
    BagRandomizer<RhythmCard> cardBag;

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

    public int CardsCleared { get; private set; }
    public bool Dead { get; private set; }

    public int BPM => BSteps * BPM_PER_BSTEP;
    public ReadOnlyCollection<RhythmCard> Cards => cards.AsReadOnly();

    public bool InDanger => cards.Count > BEATS_PER_MEASURE * MEASURES;

    public Track ()
    {
        // there are 2^n permutations of any pattern with n values that are either on or off, like a beat pattern. subtract 1 because we don't include the all-off pattern
        int uniqueCardPermutations = (1 << QUAVERS_PER_BEAT) - 1;

        // start at 1 to avoid the all-off pattern
        List<RhythmCard> allPossibleCards = Enumerable.Range(1, uniqueCardPermutations).Select(i => indexToCard(i)).ToList();

        cardBag = new BagRandomizer<RhythmCard> { Items = allPossibleCards };
    }

    public void SpawnCards (int numCards)
    {
        if (numCards != 0)
        {
            FailedLastCard = false;
        }

        for (int i = 0; i < numCards; i++)
        {
            cards.Add(cardBag.GetNext());
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

            CardsCleared++;
            if (CardsCleared % CARDS_PER_DIFFICULTY_INCREASE == 0)
            {
                BSteps++;
                CardSpawnRate--;
            }
        }
    }

    public void FailCard ()
    {
        if (cards.Count == 0) return;

        RhythmCard card = cards[0];
        cards.RemoveAt(0);
        cards.Add(card);

        FailedLastCard = true;
    }

    RhythmCard indexToCard (int index)
    {
        string paddedBinary = Convert.ToString(index, 2).PadLeft(QUAVERS_PER_BEAT, '0');
        return new RhythmCard(paddedBinary.Select(c => c == '1').ToArray());
    }
}
