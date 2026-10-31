using Blackjack.Core;

namespace Blackjack;

internal static class Program
{
    private static int Main()
    {
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Run();
            return 0;
        }
        catch (Exception ex)
        {
            // Top-level boundary only: game logic throws specific
            // exceptions (e.g. dealing from an empty deck); the player
            // gets a readable message and a non-zero exit code, not a
            // stack trace. The full trace is still available in a debugger.
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    private static void Run()
    {
        string[] menu = ["1. Play Blackjack", "2. Shuffle and Show Deck", "3. Exit"];
        const string prompt = "\nChoice: ";

        while (true)
        {
            ClearScreen();
            int? menuChoice = ReadChoice(prompt, menu);
            if (menuChoice is null)
            {
                return;
            }

            switch (menuChoice)
            {
                case 1:
                    PlayBlackjack();
                    break;
                case 2:
                    ShowDeck();
                    break;
                case 3:
                    Console.WriteLine("3. Exit");
                    return;
            }
        }
    }

    private static void PlayBlackjack()
    {
        var deck = new Deck();
        deck.Shuffle();
        var game = new BlackjackGame(deck);

        while (!game.IsOver)
        {
            ClearScreen();
            ShowHands(game, hideDealerCard: true);
            Console.WriteLine("\nHit or stand? (h/s)");

            string? input = Console.ReadLine();
            if (input is null)
            {
                return;
            }

            switch (input.Trim().ToLowerInvariant())
            {
                case "h":
                case "hit":
                    game.Hit();
                    break;
                case "s":
                case "stand":
                    game.Stand();
                    break;
                default:
                    Console.WriteLine("error: invalid input detected");
                    Console.WriteLine("please enter h or s");
                    PressAnyKey();
                    break;
            }
        }

        ClearScreen();
        ShowHands(game, hideDealerCard: false);
        Console.WriteLine();
        Console.WriteLine(game.Outcome switch
        {
            GameOutcome.PlayerWins when game.Player.IsBlackjack => "Blackjack! You win!",
            GameOutcome.PlayerWins when game.Dealer.IsBust => "Dealer busts. You win!",
            GameOutcome.PlayerWins => "You win!",
            GameOutcome.DealerWins when game.Player.IsBust => "Bust! Dealer wins.",
            GameOutcome.DealerWins when game.Dealer.IsBlackjack => "Dealer has blackjack. Dealer wins.",
            GameOutcome.DealerWins => "Dealer wins.",
            GameOutcome.Push => "Push. Nobody wins.",
            _ => string.Empty,
        });
        PressAnyKey();
    }

    private static void ShowHands(BlackjackGame game, bool hideDealerCard)
    {
        // Snapshot once: Hand.Cards copies the list on every access,
        // so indexing it in a loop would allocate once per card.
        IReadOnlyList<Card> dealerCards = game.Dealer.Cards;
        Console.Write("Dealer: ");
        if (hideDealerCard && dealerCards.Count > 1)
        {
            WriteCard(dealerCards[0]);
            Console.Write(" ??");
        }
        else
        {
            WriteHand(game.Dealer);
            Console.Write($" ({game.Dealer.Value})");
        }

        Console.WriteLine();
        Console.Write("You:    ");
        WriteHand(game.Player);
        Console.WriteLine($" ({game.Player.Value})");
    }

    private static void ShowDeck()
    {
        var deck = new Deck();
        deck.Shuffle();

        // Snapshot once: Deck.Cards copies the list on every access.
        IReadOnlyList<Card> cards = deck.Cards;
        Console.WriteLine("\n 2. Shuffle and Show Deck");
        for (int i = 0; i < cards.Count; i++)
        {
            WriteCard(cards[i]);
            Console.Write("  ");
            if ((i + 1) % 13 == 0)
            {
                Console.WriteLine();
            }
        }

        PressAnyKey();
    }

    private static void WriteHand(Hand hand)
    {
        // Snapshot once: Hand.Cards copies the list on every access.
        IReadOnlyList<Card> cards = hand.Cards;
        for (int i = 0; i < cards.Count; i++)
        {
            if (i > 0)
            {
                Console.Write(" ");
            }

            WriteCard(cards[i]);
        }
    }

    private static void WriteCard(Card card)
    {
        if (Console.IsOutputRedirected)
        {
            Console.Write(card.ToString().PadRight(3));
            return;
        }

        ConsoleColor saved = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = card.IsRed ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.Write(card.ToString().PadRight(3));
        }
        finally
        {
            Console.ForegroundColor = saved;
        }
    }

    private static int? ReadChoice(string prompt, string[] options)
    {
        foreach (string option in options)
        {
            Console.WriteLine(option);
        }

        return ReadInteger(prompt, 1, options.Length);
    }

    /// <summary>Reads an integer in range, re-prompting on bad input.
    /// Returns null on end of input (Ctrl+D / Ctrl+Z) instead of looping
    /// forever, which the original did.</summary>
    private static int? ReadInteger(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();
            if (line is null)
            {
                return null;
            }

            if (int.TryParse(line, out int choice) && choice >= min && choice <= max)
            {
                return choice;
            }

            Console.WriteLine("error: invalid input detected");
            Console.WriteLine("please enter a valid input");
        }
    }

    private static void ClearScreen()
    {
        if (!Console.IsOutputRedirected)
        {
            Console.Clear();
        }
    }

    private static void PressAnyKey()
    {
        if (Console.IsInputRedirected)
        {
            return;
        }

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey(intercept: true);
    }
}
