using Blackjack.Core;
using Xunit;

namespace Blackjack.Tests;

public class HandTests
{
    [Theory]
    [InlineData(new[] { CardFace.Ace, CardFace.King }, 21, true)]
    [InlineData(new[] { CardFace.Ace, CardFace.Six }, 17, true)]
    [InlineData(new[] { CardFace.Ace, CardFace.Ace, CardFace.Nine }, 21, true)]
    [InlineData(new[] { CardFace.Ace, CardFace.Five, CardFace.Ace }, 17, true)]
    [InlineData(new[] { CardFace.King, CardFace.Queen, CardFace.Two }, 22, false)]
    [InlineData(new[] { CardFace.Five, CardFace.Five, CardFace.Five, CardFace.Six }, 21, false)]
    [InlineData(new[] { CardFace.Ace, CardFace.Ace, CardFace.Ace, CardFace.Eight }, 21, true)]
    [InlineData(new[] { CardFace.Ten, CardFace.Seven }, 17, false)]
    public void Value_CountsAcesAsElevenWhenItDoesNotBust(CardFace[] faces, int expected, bool expectedSoft)
    {
        var hand = new Hand();
        foreach (CardFace face in faces)
        {
            hand.AddCard(new Card(face, CardSuit.Spades));
        }

        Assert.Equal(expected, hand.Value);
        Assert.Equal(expectedSoft, hand.IsSoft);
    }

    [Fact]
    public void EmptyHand_HasValueZero()
    {
        Assert.Equal(0, new Hand().Value);
        Assert.False(new Hand().IsBust);
    }

    [Fact]
    public void IsBlackjack_OnlyTwoCardsTotallingTwentyOne()
    {
        var blackjack = new Hand();
        blackjack.AddCard(new Card(CardFace.Ace, CardSuit.Spades));
        blackjack.AddCard(new Card(CardFace.King, CardSuit.Spades));
        Assert.True(blackjack.IsBlackjack);

        var twentyOne = new Hand();
        twentyOne.AddCard(new Card(CardFace.Seven, CardSuit.Spades));
        twentyOne.AddCard(new Card(CardFace.Seven, CardSuit.Spades));
        twentyOne.AddCard(new Card(CardFace.Seven, CardSuit.Spades));
        Assert.False(twentyOne.IsBlackjack);
    }

    [Fact]
    public void AddCard_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Hand().AddCard(null!));
    }

    [Fact]
    public void Clear_EmptiesTheHand()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardFace.Ace, CardSuit.Spades));
        hand.Clear();

        Assert.Equal(0, hand.Count);
        Assert.Equal(0, hand.Value);
    }

    [Fact]
    public void ToString_JoinsCardsWithSpaces()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardFace.Ace, CardSuit.Spades));
        hand.AddCard(new Card(CardFace.Ten, CardSuit.Hearts));

        Assert.Equal("A♠ 10♥", hand.ToString());
    }
}
