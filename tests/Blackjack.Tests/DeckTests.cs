using Blackjack.Core;
using Xunit;

namespace Blackjack.Tests;

public class DeckTests
{
    [Fact]
    public void NewDeck_HasFiftyTwoUniqueCards_FourOfEachFaceAndThirteenOfEachSuit()
    {
        var deck = new Deck();

        Assert.Equal(52, deck.Count);
        Assert.Equal(52, deck.Cards.Distinct().Count());
        Assert.Equal(4, deck.Cards.Count(c => c.Face == CardFace.Ace));
        Assert.Equal(13, deck.Cards.Count(c => c.Suit == CardSuit.Hearts));
    }

    [Fact]
    public void Shuffle_PreservesTheMultiset_AndChangesTheOrder()
    {
        var deck = new Deck();
        Card[] before = deck.Cards.ToArray();

        deck.Shuffle(new Random(42));

        Assert.Equal(52, deck.Count);
        Assert.Equal(before.OrderBy(c => c.ToString()), deck.Cards.OrderBy(c => c.ToString()));
        Assert.NotEqual(before, deck.Cards.ToArray());
    }

    [Fact]
    public void Shuffle_WithSameSeed_IsReproducible()
    {
        var first = new Deck();
        first.Shuffle(new Random(7));
        var second = new Deck();
        second.Shuffle(new Random(7));

        Assert.Equal(first.Cards, second.Cards);
    }

    [Fact]
    public void Shuffle_NullRandom_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Deck().Shuffle(null!));
    }

    [Fact]
    public void Deal_RemovesTheTopCard_InOrder()
    {
        var deck = new Deck();

        Assert.Equal(new Card(CardFace.Ace, CardSuit.Spades), deck.Deal());
        Assert.Equal(new Card(CardFace.Two, CardSuit.Spades), deck.Deal());
        Assert.Equal(50, deck.Count);
    }

    [Fact]
    public void Deal_EmptyDeck_ThrowsInsteadOfSilentlyRebuilding()
    {
        var deck = new Deck([]);

        Assert.Throws<InvalidOperationException>(() => deck.Deal());
    }

    [Fact]
    public void Cards_ReturnsACopy_MutatingItDoesNotAffectTheDeck()
    {
        var deck = new Deck();
        var snapshot = (Card[])deck.Cards;
        snapshot[0] = new Card(CardFace.King, CardSuit.Hearts);

        Assert.Equal(new Card(CardFace.Ace, CardSuit.Spades), deck.Cards[0]);
        Assert.Equal(52, deck.Count);
    }
}
