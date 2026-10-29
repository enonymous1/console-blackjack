using Blackjack.Core;
using Xunit;

namespace Blackjack.Tests;

public class GameTests
{
    // Deal order is player, dealer, player, dealer, then hit/draw cards.
    private static BlackjackGame Rigged(params Card[] cards) => new(new Deck(cards));

    private static Card C(CardFace face, CardSuit suit = CardSuit.Spades) => new(face, suit);

    [Fact]
    public void PlayerBlackjack_WinsImmediately()
    {
        var game = Rigged(C(CardFace.Ace), C(CardFace.Five), C(CardFace.King), C(CardFace.Six));

        Assert.True(game.IsOver);
        Assert.True(game.Player.IsBlackjack);
        Assert.Equal(GameOutcome.PlayerWins, game.Outcome);
    }

    [Fact]
    public void DealerBlackjack_WinsImmediately()
    {
        var game = Rigged(C(CardFace.Five), C(CardFace.Ace), C(CardFace.Six), C(CardFace.King));

        Assert.True(game.IsOver);
        Assert.Equal(GameOutcome.DealerWins, game.Outcome);
    }

    [Fact]
    public void BothBlackjack_IsAPush()
    {
        var game = Rigged(C(CardFace.Ace), C(CardFace.Ace), C(CardFace.King), C(CardFace.King));

        Assert.True(game.IsOver);
        Assert.Equal(GameOutcome.Push, game.Outcome);
    }

    [Fact]
    public void Hit_Busting_LosesImmediately()
    {
        var game = Rigged(
            C(CardFace.Ten), C(CardFace.Five),
            C(CardFace.Nine), C(CardFace.Six),
            C(CardFace.Five));

        game.Hit();

        Assert.True(game.IsOver);
        Assert.True(game.Player.IsBust);
        Assert.Equal(GameOutcome.DealerWins, game.Outcome);
    }

    [Fact]
    public void Hit_ToTwentyOne_StandsAutomatically()
    {
        var game = Rigged(
            C(CardFace.Ten), C(CardFace.Ten),
            C(CardFace.Five), C(CardFace.Seven),
            C(CardFace.Six));

        game.Hit();

        Assert.True(game.IsOver);
        Assert.Equal(21, game.Player.Value);
        Assert.Equal(GameOutcome.PlayerWins, game.Outcome);
    }

    [Fact]
    public void Stand_DealerDrawsToSeventeenAndBusts()
    {
        var game = Rigged(
            C(CardFace.Ten), C(CardFace.Six),
            C(CardFace.Seven), C(CardFace.Ten),
            C(CardFace.Ten));

        game.Stand();

        Assert.True(game.Dealer.IsBust);
        Assert.Equal(GameOutcome.PlayerWins, game.Outcome);
    }

    [Fact]
    public void Stand_DealerStandsOnSeventeen()
    {
        var game = Rigged(
            C(CardFace.Ten), C(CardFace.Ten),
            C(CardFace.Seven), C(CardFace.Seven),
            C(CardFace.Ten));

        game.Stand();

        Assert.Equal(17, game.Dealer.Value);
        Assert.Equal(2, game.Dealer.Count);
        Assert.Equal(GameOutcome.Push, game.Outcome);
    }

    [Fact]
    public void Stand_DealerStandsOnSoftSeventeen()
    {
        var game = Rigged(
            C(CardFace.Ten), C(CardFace.Ace),
            C(CardFace.Eight), C(CardFace.Six),
            C(CardFace.Ten));

        game.Stand();

        Assert.Equal(17, game.Dealer.Value);
        Assert.Equal(2, game.Dealer.Count);
        Assert.Equal(GameOutcome.PlayerWins, game.Outcome);
    }

    [Fact]
    public void ActionsAfterGameOver_Throw()
    {
        var game = Rigged(C(CardFace.Ace), C(CardFace.Five), C(CardFace.King), C(CardFace.Six));

        Assert.Throws<InvalidOperationException>(() => game.Hit());
        Assert.Throws<InvalidOperationException>(() => game.Stand());
    }

    [Fact]
    public void NullDeck_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new BlackjackGame(null!));
    }
}
