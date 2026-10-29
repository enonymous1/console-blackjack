using Blackjack.Core;
using Xunit;

namespace Blackjack.Tests;

public class CardTests
{
    [Fact]
    public void ToString_UsesRankAndSuitGlyph()
    {
        Assert.Equal("A♠", new Card(CardFace.Ace, CardSuit.Spades).ToString());
        Assert.Equal("10♥", new Card(CardFace.Ten, CardSuit.Hearts).ToString());
        Assert.Equal("Q♣", new Card(CardFace.Queen, CardSuit.Clubs).ToString());
        Assert.Equal("K♦", new Card(CardFace.King, CardSuit.Diamonds).ToString());
    }

    [Fact]
    public void PipValue_FaceCardsCountTen_AceCountsOne()
    {
        Assert.Equal(1, new Card(CardFace.Ace, CardSuit.Spades).PipValue);
        Assert.Equal(7, new Card(CardFace.Seven, CardSuit.Spades).PipValue);
        Assert.Equal(10, new Card(CardFace.Jack, CardSuit.Spades).PipValue);
        Assert.Equal(10, new Card(CardFace.Queen, CardSuit.Spades).PipValue);
        Assert.Equal(10, new Card(CardFace.King, CardSuit.Spades).PipValue);
    }

    [Fact]
    public void IsRed_OnlyHeartsAndDiamonds()
    {
        Assert.False(new Card(CardFace.Ace, CardSuit.Spades).IsRed);
        Assert.False(new Card(CardFace.Ace, CardSuit.Clubs).IsRed);
        Assert.True(new Card(CardFace.Ace, CardSuit.Hearts).IsRed);
        Assert.True(new Card(CardFace.Ace, CardSuit.Diamonds).IsRed);
    }

    [Fact]
    public void EqualCards_AreEqual()
    {
        Assert.Equal(new Card(CardFace.Ace, CardSuit.Spades), new Card(CardFace.Ace, CardSuit.Spades));
        Assert.NotEqual(new Card(CardFace.Ace, CardSuit.Spades), new Card(CardFace.Ace, CardSuit.Hearts));
    }
}
