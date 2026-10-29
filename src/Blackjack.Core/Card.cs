namespace Blackjack.Core;

/// <summary>A single playing card. Immutable and value-equal: two cards
/// with the same face and suit are equal.</summary>
public sealed record Card(CardFace Face, CardSuit Suit)
{
    /// <summary>Blackjack pip value with an ace counted as 1.
    /// Face cards count as 10. <see cref="Hand"/> upgrades an ace to 11
    /// when it does not bust the hand.</summary>
    public int PipValue => Face is CardFace.Jack or CardFace.Queen or CardFace.King ? 10 : (int)Face;

    public bool IsRed => Suit is CardSuit.Hearts or CardSuit.Diamonds;

    public string Rank => Face switch
    {
        CardFace.Ace => "A",
        CardFace.Jack => "J",
        CardFace.Queen => "Q",
        CardFace.King => "K",
        _ => ((int)Face).ToString(System.Globalization.CultureInfo.InvariantCulture),
    };

    public string SuitGlyph => Suit switch
    {
        CardSuit.Spades => "♠",
        CardSuit.Hearts => "♥",
        CardSuit.Clubs => "♣",
        CardSuit.Diamonds => "♦",
        _ => throw new InvalidOperationException($"Unknown suit: {Suit}"),
    };

    public override string ToString() => $"{Rank}{SuitGlyph}";
}
