namespace Blackjack.Core;

/// <summary>A deck of cards, dealt from the front of the list.
/// A new deck holds the 52 standard cards, in suit order, unshuffled:
/// call <see cref="Shuffle"/> before dealing.</summary>
public sealed class Deck
{
    private readonly List<Card> _cards;

    public Deck()
        : this(CreateStandardDeck())
    {
    }

    public Deck(IEnumerable<Card> cards)
    {
        ArgumentNullException.ThrowIfNull(cards);
        _cards = cards.ToList();
    }

    public int Count => _cards.Count;

    /// <summary>A copy of the cards still in the deck, in deal order.</summary>
    public IReadOnlyList<Card> Cards => _cards.ToArray();

    public static IReadOnlyList<Card> CreateStandardDeck()
    {
        var cards = new List<Card>(52);
        foreach (CardSuit suit in Enum.GetValues<CardSuit>())
        {
            foreach (CardFace face in Enum.GetValues<CardFace>())
            {
                cards.Add(new Card(face, suit));
            }
        }

        return cards;
    }

    /// <summary>Fisher-Yates shuffle using the shared random source.</summary>
    public void Shuffle() => Shuffle(Random.Shared);

    /// <summary>Fisher-Yates shuffle. Passing a seeded <see cref="Random"/>
    /// makes the order reproducible, which is what the tests rely on.</summary>
    public void Shuffle(Random random)
    {
        ArgumentNullException.ThrowIfNull(random);

        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    /// <summary>Deals the top card. Throws when the deck is empty instead
    /// of silently rebuilding it: the caller owns reshuffling.</summary>
    public Card Deal()
    {
        if (_cards.Count == 0)
        {
            throw new InvalidOperationException("Cannot deal from an empty deck.");
        }

        Card card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}
