namespace Blackjack.Core;

/// <summary>The cards one player holds, with blackjack hand values:
/// aces count as 11 when that does not bust the hand, otherwise 1.</summary>
public sealed class Hand
{
    private readonly List<Card> _cards = [];

    public IReadOnlyList<Card> Cards => _cards.ToArray();

    public int Count => _cards.Count;

    /// <summary>Best blackjack value: the highest value at or under 21,
    /// or the lowest bust value when every ace-as-11 busts.</summary>
    public int Value => Evaluate().Value;

    public bool IsBust => Value > 21;

    public bool IsBlackjack => _cards.Count == 2 && Value == 21;

    /// <summary>True when an ace is counted as 11 in <see cref="Value"/>.</summary>
    public bool IsSoft => Evaluate().IsSoft;

    public void AddCard(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);
        _cards.Add(card);
    }

    public void Clear() => _cards.Clear();

    public override string ToString() => string.Join(" ", _cards);

    private (int Value, bool IsSoft) Evaluate()
    {
        int total = 0;
        int aces = 0;
        foreach (Card card in _cards)
        {
            total += card.PipValue;
            if (card.Face == CardFace.Ace)
            {
                aces++;
            }
        }

        bool isSoft = false;
        while (aces > 0 && total + 10 <= 21)
        {
            total += 10;
            aces--;
            isSoft = true;
        }

        return (total, isSoft);
    }
}
