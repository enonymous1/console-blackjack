namespace Blackjack.Core;

/// <summary>One round of blackjack. The game takes a deck as dealt:
/// shuffle it before constructing the game. Cards are dealt
/// alternating player, dealer, player, dealer. The dealer draws
/// until reaching 17 and stands on all 17s, including soft 17.</summary>
public sealed class BlackjackGame
{
    private readonly Deck _deck;

    public BlackjackGame()
        : this(CreateShuffledDeck())
    {
    }

    private static Deck CreateShuffledDeck()
    {
        var deck = new Deck();
        deck.Shuffle();
        return deck;
    }

    public BlackjackGame(Deck deck)
    {
        ArgumentNullException.ThrowIfNull(deck);
        _deck = deck;

        Player.AddCard(_deck.Deal());
        Dealer.AddCard(_deck.Deal());
        Player.AddCard(_deck.Deal());
        Dealer.AddCard(_deck.Deal());

        if (Player.IsBlackjack || Dealer.IsBlackjack)
        {
            Finish();
        }
    }

    public Hand Player { get; } = new();

    public Hand Dealer { get; } = new();

    public bool IsOver { get; private set; }

    public GameOutcome? Outcome { get; private set; }

    public void Hit()
    {
        EnsurePlaying();
        Player.AddCard(_deck.Deal());
        if (Player.IsBust)
        {
            Finish();
        }
        else if (Player.Value == 21)
        {
            Stand();
        }
    }

    public void Stand()
    {
        EnsurePlaying();

        if (!Player.IsBust)
        {
            while (Dealer.Value < 17)
            {
                Dealer.AddCard(_deck.Deal());
            }
        }

        Finish();
    }

    private void Finish()
    {
        IsOver = true;
        Outcome = Player.IsBust || Dealer.IsBust
            ? Player.IsBust ? GameOutcome.DealerWins : GameOutcome.PlayerWins
            : Player.Value == Dealer.Value ? GameOutcome.Push
            : Player.Value > Dealer.Value ? GameOutcome.PlayerWins
            : GameOutcome.DealerWins;
    }

    private void EnsurePlaying()
    {
        if (IsOver)
        {
            throw new InvalidOperationException("The game is already over.");
        }
    }
}
