# Console Blackjack

[![CI](https://github.com/enonymous1/console-blackjack/actions/workflows/ci.yml/badge.svg)](https://github.com/enonymous1/console-blackjack/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)

A small C# console blackjack game. Pick a card game from the menu,
play a hand against the dealer, or shuffle and look at the deck.

## Quick start

Prerequisite: the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```sh
dotnet run --project src/Blackjack
```

The menu is the original 2021 interface, kept intact:

```text
1. Play Blackjack
2. Shuffle and Show Deck
3. Exit
```

Example — option 2, from a real piped run (the shuffled order varies
every run):

```text
 2. Shuffle and Show Deck
5♠   6♣   9♣   Q♣   3♣   A♠   7♠   J♣   Q♠   J♥   A♣   K♠   4♦
A♥   9♦   3♥   4♣   5♥   3♠   K♦   K♣   6♦   10♣  10♠  5♦   A♦
2♥   8♣   3♦   7♣   Q♦   9♥   10♥  2♠   7♦   5♣   Q♥   2♣   6♥
8♥   J♦   10♦  J♠   9♠   4♥   8♦   2♦   4♠   7♥   8♠   6♠   K♥
```

## Rules

Cards are dealt alternating player, dealer, player, dealer. Number
cards count at face value, face cards count 10, and an ace counts 11
when that does not bust the hand, otherwise 1. Hit (`h`) to take
another card, stand (`s`) to stop. Hitting to 21 stands automatically.
The dealer draws until reaching 17 and stands on all 17s, including
soft 17. Blackjack on the first two cards wins immediately unless the
dealer has it too, which is a push.

Hearts and diamonds print in red on a colour terminal. Piped output
prints plain text with no screen clearing, so the game also works
from scripts: end-of-input exits cleanly instead of looping.

## Layout

```text
src/Blackjack.Core/   Cards, deck, hand values, and game rules.
                      No console I/O, so it is all unit-testable.
src/Blackjack/        Console menu and display only.
tests/Blackjack.Tests/ xUnit tests, including rigged-deck games.
```

The core API is public and small: `Card` (an immutable record with
`PipValue`, `IsRed`, and `A♠`-style `ToString`), `Deck` (52 cards,
Fisher-Yates `Shuffle`, `Deal`), `Hand` (`Value`, `IsBust`,
`IsBlackjack`, `IsSoft`), and `BlackjackGame` (`Hit`, `Stand`,
`Outcome`). `Deal` throws on an empty deck instead of silently
rebuilding it — the caller owns reshuffling.

## Development

```sh
dotnet build console-blackjack.sln --configuration Release
dotnet test console-blackjack.sln --configuration Release --no-build
dotnet format console-blackjack.sln --verify-no-changes
```

See [CHANGELOG.md](CHANGELOG.md) for what changed from the 2021
original, including the two bugs that kept it from ever displaying
a deck.
