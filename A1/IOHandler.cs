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
        this.Commands = [ // This can probably be deleted later .
            "/new",
            "/load",
            "/help",
            "/grid",
            "/quit"
        ];
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

    // List commands available from the main menu
    // This could be re-factored to use a dictionary, so each command has a matching desc?
    public void PrintMenuCommands()
    {
        PrintHeading("┌───────────────────────────┐\n");
        PrintHeading("|     Welcome to LineUp     |\n");
        PrintHeading("└───────────────────────────┘\n");
        PrintHeading("Please enter one of the following commands:\n");
        foreach (string cmd in Commands)
        {
            PrintHeading($"{cmd}\n");
        }
    }

    // Prints help information to the terminal - relevant to menu commands
    public void PrintMenuHelp()
    {
        PrintGreen("To be implemented...\n");
    }

    // Prints help information to the terminal - relevant to core game loop
    public void PrintGameHelp()
    {
        PrintGreen("To be implemented...\n");
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
            }
            break;
        }
        
        return input.ToLower();
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