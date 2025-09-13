/// <summary>
/// Responsible for parsing input and sending output to the terminal
/// </summary>
public class IOHandler
{
    // Methods

    /// <summary>
    /// Prints text in a different colour
    /// </summary>
    public void PrintHeading(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Prints text in a different colour
    /// </summary>
    public void PrintError(string text)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(text);
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Prints text in a different colour
    /// </summary>
    public void PrintGreen(string text)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(text);
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Prints banner during gameplay
    /// </summary>
    public void PrintBanner()
    {
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║               LineUp                  ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
    }

    /// <summary>
    /// Prints banner when loading 
    /// </summary>
    public void PrintLoadBanner()
    {
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║              Loading                  ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
    }

    /// <summary>
    /// Lists the commands available from the main menu
    /// </summary>
    public void PrintMenuCommands()
    {

        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║           Welcome to LineUp           ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        PrintHeading("Please enter one of the following commands:\n");
        PrintHeading("  /new\n");
        PrintHeading("  /load\n");
        PrintHeading("  /help\n");
        PrintHeading("  /grid\n");
        PrintHeading("  /testing\n");
        PrintHeading("  /quit\n");
    }

    /// <summary>
    /// Prints help section when /help is called from main menu
    /// </summary>
    public void PrintMenuHelp()
    {
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║                Help                   ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        Console.WriteLine("In this game, you'll place discs in an attempt to connect four (or more) consecutive discs, and win the game.");
        Console.WriteLine("Discs can be consecutive vertically, horizontally, or diagonally.");
        Console.WriteLine("You can use the following commands from the menu:");
        PrintHeading("/new: ");
        Console.WriteLine("start a new game");
        PrintHeading("/load: ");
        Console.WriteLine("continue a previous game");
        PrintHeading("/grid: ");
        Console.WriteLine("change the dimensions of the playable grid");
        PrintHeading("/testing: ");
        Console.WriteLine("input a sequence of moves and render the result");
        PrintHeading("/quit: ");
        Console.WriteLine("guess!");
        PrintHeading("Press Enter to return...\n");
        Console.Write("> ");
        Console.ReadLine();
        Console.Clear();

    }

    /// <summary>
    ///  Prints help menu when /help is called during gameplay
    /// </summary>
    /// <param name="WinLength">The number of discs that are required to be aligned to get a win</param>
    public void PrintGameHelp(int WinLength)
    {
        Console.Clear();
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║                Help                   ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        Console.Write("In order to win, you'll need to align ");
        PrintHeading($"{WinLength}");
        Console.Write(" discs in a row vertically, horizontally or diagonally.\n");
        Console.WriteLine("To place a disc, enter the disc type followed by the column number, for example:");
        PrintHeading("o1 ");
        Console.WriteLine("will place an Ordinary disc in the first column");
        PrintHeading("b7 ");
        Console.WriteLine("will place a Boring disc in the seventh column");
        Console.WriteLine("\nYou can also make use of the following commands during gameplay:");
        PrintHeading("/help ");
        Console.WriteLine("to access this menu");
        PrintHeading("/save ");
        Console.WriteLine("to save the game in its current state");
        PrintHeading("/quit ");
        Console.WriteLine("to return to the menu, without saving");
        PrintHeading("\nPress Enter to return...\n");
        Console.Write("> ");
        Console.ReadLine();
        Console.Clear();
    }

    /// <summary>
    /// Prints instructions during Testing mode 
    /// </summary>
    public void PrintTestingMode()
    {
        Console.Clear();
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║               Testing                 ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        Console.WriteLine("You can use this feature to input a sequence of moves and render the final result.");
        Console.WriteLine("Input a single string of moves [disc,column], separated commas (,). For example:");
        PrintHeading("o1,o2,o3,e2,o1,b1\n");
        Console.WriteLine("To test with uniquely sized grids, use the '/grid' command before using Testing mode");
        // Get Sequence

    }

    /// <summary>
    /// Prints the winner of the game.
    /// If both players are winners, prints 'tie'
    /// </summary>
    /// <param name="PlayerOneWin">Does player 1 have a winning sequence?</param>
    /// <param name="PlayerTwoWin">Does player 2 have a winning sequence?</param>
    public void PrintWinner(bool PlayerOneWin, bool PlayerTwoWin)
    {
        string winner;
        if (PlayerOneWin && PlayerTwoWin)
        {
            winner = "It's a Tie !";
        }
        else if (PlayerOneWin)
        {
            winner = "Player One Wins !";
        }
        else
        {
            winner = "Player Two Wins !";
        }
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading($"║           {winner,17}           ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        PrintHeading("Press Enter to exit...\n");
        Console.Write("> ");
        Console.ReadLine();
        Console.Clear();
    }

    /// <summary>
    /// Get input from the player when using /grid from main menu
    /// and validate it
    /// </summary>
    /// <returns>a tuple containing the new grid width and height</returns>
    public (int width, int height) GetInputGridSize()
    {
        string input;
        int rows, cols;
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║                 Grid                  ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        Console.WriteLine("You are now changing the size of the grid.");
        Console.WriteLine("A grid may be square, but cannot have more rows than columns.");
        Console.Write("The smallest grid is ");
        PrintHeading("6x7");
        Console.Write(" Rows x Columns.\n");
        Console.Write("The largest grid is ");
        PrintHeading("40x40");
        Console.Write(" Rows x Columns.\n\n");


        // Determine the number of columns - we do this first in order to set an upper boundary for the rows
        while (true)
        {
            Console.WriteLine("Enter the number of columns [7+]:");
            Console.Write("> ");
            input = Console.ReadLine();
            try
            {
                cols = Int32.Parse(input);
                if (cols < 7 || cols > 40)
                {
                    PrintError("Must be between 7 and 40 (inclusive)");
                    continue;
                }
                break;
            }
            catch (FormatException)
            {
                PrintError("Must be a number between 7 and 40 (inclusive)");
            }
            catch (OverflowException)
            {
                PrintError("Must be a number between 7 and 40 (inclusive)");
            }
        }

        // Determine the number of rows 
        while (true)
        {
            Console.WriteLine($"Enter the number of rows [6..{cols}]:");
            Console.Write("> ");
            input = Console.ReadLine();
            try
            {
                rows = Int32.Parse(input);
                if (rows < 6 || rows > cols)
                {
                    PrintError($"Must have between 6 and {cols} rows");
                    continue;
                }
                break;
            }
            catch (FormatException)
            {
                PrintError($"Must be a number between 6 and {cols}");
            }
            catch (OverflowException)
            {
                PrintError($"Must be a number between 6 and {cols}");
            }
        }

        return (rows, cols);
    }

    /// <summary>
    /// Get basic input from the main menu
    /// </summary>
    /// <returns>raw string</returns>
    public string GetInputMenu()
    {
        Console.Write("> ");
        string input = Console.ReadLine();
        return input.ToLower();
    }

    /// <summary>
    /// Used to get input regarding gamemode (HvC or HvH)
    /// Validates the string
    /// </summary>
    public string GetPlayerCount()
    {
        string input;
        while (true)
        {
            Console.Write("Do you wish to play against AI? {");
            PrintHeading("Y");
            Console.Write("/");
            PrintHeading("N");
            Console.Write("}\n");
            Console.Write("> ");

            input = Console.ReadLine();
            input = input.ToLower().Trim();
            if (input != "y" && input != "n")
            {
                PrintError("Invalid Input - must be [Y] or [N]");
                continue;
            }
            break;
        }
        
        return input;
    }

    // Basic get and validating of terminal input
    // Returns input string, lowercase and trimmed
    public string GetPlayerInput()
    {
        Console.WriteLine("Enter a move or command:");
        Console.Write("> ");
        return Console.ReadLine().ToLower().Trim();
    }
}