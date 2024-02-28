namespace Minesweeper;

class Minesweeper
{
    static void Main()
    {
        List<MenuOption> options = new()
        {
            new() { Name = "Test", MinefieldWidth = 5, MinefieldHeight = 5, MineCount = 3, Selected = true },
            new() { Name = "Easy", MinefieldWidth = 9, MinefieldHeight = 9, MineCount = 10 },
            new() { Name = "Medium", MinefieldWidth = 16, MinefieldHeight = 16, MineCount = 40 },
            new() { Name = "Hard", MinefieldWidth = 30, MinefieldHeight = 16, MineCount = 99 },
        };
        Minefield? minefield = null;

        // Render loop: Menu screen 
        ConsoleKeyInfo? keyInfo = null;
        do
        {
            MenuOption? selectedOption = RenderMenu(options, keyInfo?.Key);
            if (selectedOption != null)
            {
                minefield = new Minefield(selectedOption.MinefieldHeight, selectedOption.MinefieldWidth, selectedOption.MineCount);
                break;
            }

            keyInfo = Console.ReadKey();
        }
        while (keyInfo?.Key != ConsoleKey.Escape);

        // Render loop: Game screen
        string? inputLine = null;
        GameState state;
        do
        {
            state = RenderGame(minefield, inputLine);

            inputLine = Console.ReadLine();

            if (inputLine?.Trim().ToLower() == "y" && (state == GameState.Win || state == GameState.Lose))
            {
                state = GameState.Ongoing;
                minefield?.Reset();
            }
        }
        while (inputLine?.ToLower() != "q" && state == GameState.Ongoing);
    }

    private static MenuOption? RenderMenu(List<MenuOption> options, ConsoleKey? key = null)
    {
        Console.Clear();

        Console.WriteLine("Welcome to Minesweeper!");
        Console.WriteLine("Please select difficulty (or press ESC to quit):");

        int selectedOptionIndex = options.FindIndex(options => options.Selected);
        if (selectedOptionIndex == -1)
        {
            selectedOptionIndex = 0;
        }

        switch (key)
        {
            case ConsoleKey.DownArrow:
                options[selectedOptionIndex].Selected = false;
                if (selectedOptionIndex == options.Count - 1)
                {
                    selectedOptionIndex = 0;
                }
                else
                {
                    selectedOptionIndex++;
                }
                options[selectedOptionIndex].Selected = true;
                break;
            case ConsoleKey.UpArrow:
                options[selectedOptionIndex].Selected = false;
                if (selectedOptionIndex == 0)
                {
                    selectedOptionIndex = options.Count - 1;
                }
                else
                {
                    selectedOptionIndex--;
                }
                options[selectedOptionIndex].Selected = true;
                break;
            case ConsoleKey.Enter:
                return options[selectedOptionIndex];
        }

        options.ForEach(RenderMenuOption);
        return null;
    }

    private static void RenderMenuOption(MenuOption option)
    {
        string selector = option.Selected ? ">" : " ";
        Console.WriteLine($"{selector} {option.Name} ({option.MinefieldWidth}x{option.MinefieldHeight}, {option.MineCount} mines)");
    }

    private static GameState RenderGame(Minefield? minefield, string? inputLine)
    {
        Console.Clear();

        GameState state = GameState.Ongoing;
        if (minefield != null)
        {
            (int? digX, int? digY) = ParseUserCoordinates(inputLine);
            if (digX != null && digY != null)
            {
                state = minefield.Visit((int)digX, (int)digY);
            }

            RenderGameField(minefield, state, digX, digY);
        }

        Console.WriteLine();
        switch (state)
        {
            case GameState.Ongoing:
                Console.WriteLine("Dig at coordinate \"x y\", or type \"q\" to quit the game.");
                break;
            case GameState.Win:
                Console.WriteLine("Congratulations! You cleared the minefield. Try again? (y/n)");
                break;
            case GameState.Lose:
                Console.WriteLine("Oh no! You found a mine that blew up. Try again? (y/n)");
                break;
        }
        Console.Write("> ");

        return state;
    }

    /// <summary>
    /// Parse user input of form "x y".
    /// </summary>
    private static (int? digX, int? digY) ParseUserCoordinates(string? inputString)
    {
        int? digX = null, digY = null;
        string[]? inputParts = inputString?.Trim().Split(" ");
        if (inputParts?.Length == 2)
        {
            if (int.TryParse(inputParts[0], out int inputX))
            {
                digX = inputX;
            }
            if (int.TryParse(inputParts[1], out int inputY))
            {
                digY = inputY;
            }
        }

        return (digX, digY);
    }

    private static void RenderGameField(Minefield minefield, GameState state, int? digX, int? digY)
    {
        // Render x coordinate labels
        for (int digitSignificance = Helper.GetDigitCount(minefield.Width) - 1; digitSignificance >= 0; digitSignificance--)
        {
            Console.Write(new string(' ', Helper.GetDigitCount(minefield.Height)) + " ");
            for (int x = 0; x < minefield.Width; x++)
            {
                Console.Write(Helper.GetSignificantDigit(x, digitSignificance));
            }
            Console.WriteLine();
        }

        // Render the rest of the minefield
        for (int y = 0; y < minefield.Height; y++)
        {
            string paddedY = Helper.GetPaddedNumber(y, minefield.Height - 1);
            Console.Write($"{paddedY}|");

            for (int x = 0; x < minefield.Width; x++)
            {
                if (state == GameState.Lose && x == digX && y == digY)
                {
                    Console.Write("X");
                }
                else if (minefield.IsCovered(x, y))
                {
                    Console.Write("?");
                }
                else if (minefield.GetAdjacentMineCount(x, y) == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(minefield.GetAdjacentMineCount(x, y));
                }
            }
            Console.WriteLine();
        }
    }
}
