# Changelog

## 2.0.0 — 2026-10-29: standalone modernization (squashed export)

Renamed 2026-10-29 from `Blackjack` to `console-blackjack` (repo,
folder, and solution name only; the `Blackjack` / `Blackjack.Core`
namespaces and project names are unchanged).

The original was a 2021 .NET Framework 4.7.2 console app plus class
library (352 lines). It never actually played blackjack, and two bugs
meant it never finished starting a deck display at all. This pass
keeps the same program — a console blackjack menu — and brings it to
current standards:

- **.NET 10.0**, solution file, shared build properties
  (`Directory.Build.props`: nullable + implicit usings on,
  warnings as errors except NU1900).
- **Split into two projects:** `Blackjack.Core` (cards, deck, hand
  values, game rules — all unit-testable, no console I/O) and
  `Blackjack` (console UI only). The original mixed cursor-position
  drawing into the card and hand classes.
- **The game is now playable** (menu option 1 was "Coming Soon!"):
  alternating deal, hit/stand, dealer draws to 17 and stands on all
  17s including soft 17, blackjack / bust / push outcomes.
- **Bugs fixed:**
  - `Deck.CreateDeck` incremented the suit loop variable in the face
    loop, so deck construction never terminated — and would have
    built 3 suits × 12 faces. A new deck now holds exactly 52 unique
    cards, 4 suits × 13 faces.
  - `Deck.Draw` and `Hand.Draw` called themselves instead of drawing
    a card (stack overflow). Drawing is gone from the core: cards
    render as strings (`A♠`, `10♥`) and the console lays them out.
  - `Shuffle` swapped each card with a uniformly random position
    (biased) using a freshly seeded `Random` each call. It is now
    Fisher-Yates, with `Random.Shared` by default and an injectable
    `Random` for reproducible tests.
  - `Deal` silently rebuilt and reshuffled an empty deck. It now
    throws `InvalidOperationException` on an empty deck; the caller
    owns reshuffling.
  - `Card`, `Hand`, the enums, and `ICard` were internal, so the
    library was barely usable outside itself. The core API is now
    public; the single-implementation `ICard` interface and the
    `Factory` ceremony are removed.
  - Menu input looped forever on end-of-input (Ctrl+D / Ctrl+Z).
    EOF now exits cleanly, and `Console.Clear` / `ReadKey` are
    skipped when input or output is redirected, so piped runs work.
  - Showing the deck cleared the screen before the user could see
    it. It now pauses for a keypress first.
  - Hand values: aces count as 11 when that does not bust, otherwise
    1. There was no hand value at all in the original.
- **Tests:** xUnit coverage for the 52-card deck, shuffle
  preservation and reproducibility, hand values including soft
  aces, and game outcomes on rigged decks (blackjacks, bust,
  dealer stands on 17 / soft 17, actions after game over).
- **Repo hygiene:** `.gitignore`, `.gitattributes` (LF),
  MIT LICENSE, GitHub Actions CI (Ubuntu + Windows: build, test,
  format check), and a rewritten README.
- **Unchanged on purpose:** the three-option menu and its wording,
  the UTF-8 suit glyphs, and the red-suit colouring in the console
  display.

## 1.0.0 — 2021-08-22 (original)

.NET Framework 4.7.2 console app and class library: menu, `Deck`,
`Card`, `Hand`, `Factory`, `ICard`. Play Blackjack was unimplemented.
