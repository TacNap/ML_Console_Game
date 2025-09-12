/// <summary>
/// Responsible for parsing input and sending output to the terminal
/// </summary>
public class IOHandler
{
    // Fields
    public string[] Commands { get; }


    // Constructor
    public IOHandler()
    {
    }
    // Methods

    public void PrintHeading(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(text);
        Console.ResetColor();
    }
    public void PrintError(string text)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(text);
        Console.ResetColor();
        Console.WriteLine();
    }

    public void PrintGreen(string text)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(text);
        Console.ResetColor();
        Console.WriteLine();
    }

    public void PrintBanner()
    {
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║               LineUp                  ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
    }

    public void PrintLoadBanner()
    {
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║              Loading                  ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
    }
    // List commands available from the main menu
    // This could be re-factored to use a dictionary, so each command has a matching desc?
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

    // Prints help information to the terminal - relevant to menu commands
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
        Console.Read();

    }

    // Prints help information to the terminal - relevant to core game loop
    public void PrintGameHelp()
    {
        Console.Clear();
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading("║                Help                   ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
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

    public void PrintWinner(bool IsPlayerOne)
    {
        string winner = IsPlayerOne ? "Player One" : "Player Two";
        PrintHeading("╔═══════════════════════════════════════╗\n");
        PrintHeading($"║           {winner} Wins !           ║\n");
        PrintHeading("╚═══════════════════════════════════════╝\n");
        PrintHeading("Press Enter to exit...\n");
        Console.Write("> ");
        Console.Read();
    }

    public (int width, int height) GetInputGridSize()
    {
        string input;
        int rows, cols;
        Console.WriteLine("You are now changing the size of the grid.");
        Console.WriteLine("The grid may not be smaller than 6 rows by 7 columns,");
        Console.WriteLine("and may not have more rows than columns");

        // Determine the number of columns
        while (true)
        {
            Console.WriteLine("Enter the number of columns [7+]:");
            Console.Write("> ");
            input = Console.ReadLine();
            try
            {
                cols = Int32.Parse(input);
                if (cols < 7)
                {
                    Console.WriteLine("Must have 7 or more columns");
                    continue;
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Must be a number between [..]");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Must be a number between [..]");
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
                    Console.WriteLine($"Must have between 6 and {cols} rows");
                    continue;
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Must be a number between [..]");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Must be a number between [..]");
            }
        }

        return (rows, cols);
    }
    public string GetInputMenu()
    {
        Console.Write("> ");
        string input = Console.ReadLine();
        return input.ToLower();
    }

    // Used to determine if player will verse another player, or AI
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