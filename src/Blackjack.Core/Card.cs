using System.Globalization;

namespace Blackjack.Core;

/// <summary>A single playing card. Immutable and value-equal: two cards
/// with the same face and suit are equal.</summary>
public sealed record Card(CardFace Face, CardSuit Suit)
{
    /// <summary>Blackjack pip value with an ace counted as 1.
    /// Face cards count as 10. <see cref="Hand"/> upgrades an ace to 11
    /// when it does not bust the hand. Throws for an unknown face
    /// instead of silently scoring it at its numeric value: enum
    /// values are not validated by the compiler, so a cast such as
    /// (CardFace)99 must fail loudly rather than score 99 points.</summary>
    public int PipValue => Face switch
    {
        CardFace.Ace => 1,
        >= CardFace.Two and <= CardFace.Ten => (int)Face,
        CardFace.Jack or CardFace.Queen or CardFace.King => 10,
        _ => throw new ArgumentOutOfRangeException(nameof(Face), Face, "Unknown card face."),
    };

    public bool IsRed => Suit is CardSuit.Hearts or CardSuit.Diamonds;

    public string Rank => Face switch
    {
        CardFace.Ace => "A",
        >= CardFace.Two and <= CardFace.Ten => ((int)Face).ToString(CultureInfo.InvariantCulture),
        CardFace.Jack => "J",
        CardFace.Queen => "Q",
        CardFace.King => "K",
        _ => throw new ArgumentOutOfRangeException(nameof(Face), Face, "Unknown card face."),
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
