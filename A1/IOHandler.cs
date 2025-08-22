/// <summary>
/// Responsible for parsing input and sending output to the terminal
/// </summary>
public class IOHandler
{
    // Fields
    public string[] Commands { get; }

    public string[] Discs { get; }

    // Constructor
    public IOHandler()
    {
        this.Commands = [
            "/new",
            "/load",
            "/help",
            "/grid",
            "/quit"
        ];

        this.Discs = [
            "o",
            "b",
            "e"
        ];
    }
    // Methods
    public static (int col, int type) ParseMoveInput(string input)
    {
        return (0, 0);
        // if starts with '/', process as command

        // else, if .length == 2, process as move

        // else, throw error
    }

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

    public string GetInputMenu()
    {
        Console.Write("> ");
        string input = Console.ReadLine();
        return input.ToLower();
    }

    // Rename this bc it's confusing
    public string GetInputPlayers()
    {
        Console.Write("Against AI? {");
        PrintHeading("Y");
        Console.Write("/");
        PrintHeading("N");
        Console.Write("}\n");
        Console.Write("> ");

        string input = Console.ReadLine();
        return input.ToLower();
    }

    // Basic get and validating of terminal input
    // Returns input string, lowercase and trimmed
    public string GetPlayerInput()
    {
        Console.WriteLine("Enter a move or command:\n > ");
        return Console.ReadLine().ToLower().Trim();
        
    }

    // Later, this will list the discs available, the commands, and an example input for move
    // Get input from player while mid game
    // Currently only accepts Move input
    // Might switch this back later to just return a valid input string
    public (int col, int type) GetInputGame()
    {
        Console.WriteLine("Enter a move: ");

        string input;
        int col;
        string discStr;
        int discInt;

        // Repeatedly get input until it's valid
        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();

            if (input.Length != 2)
            {
                PrintError("Invalid Move");
                continue;
            }

            discStr = input[0].ToString().ToLower();
            if (!Discs.Contains(discStr)) // Must be valid disc
            {
                PrintError("Invalid Move - Invalid Disc");
                continue;
            }

            if (int.TryParse(input[1].ToString(), out col)) // Col must be integer
            {
                // This uses a hardcoded '8' at the moment.
                // It should connect to Grid's Fields somehow. May need to be passed at constructor
                if (col > 0 && col < 8)
                {
                    break;
                }
                else
                {
                    PrintError("Invalid Move - Invalid Column");
                }
            }
            else
            {
                PrintError("Invalid Move - Invalid Column");
            }
        }

        // Convert str representation to type for AddDisc()

        if (discStr == "b")
        {
            discInt = 2;
        }
        else if (discStr == "e")
        {
            discInt = 3;
        }
        else
        {
            discInt = 1;
        }
        return (col - 1, discInt); // -1 for indexing
    }


}