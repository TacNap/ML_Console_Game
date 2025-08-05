public class InputHandler
{
    // Fields
    public string[] Commands { get; }

    public string[] Discs { get; }
    // Constructor

    public InputHandler()
    {
        this.Commands = [
            "/new",
            "/load",
            "/help",
            "/quit"
        ];

        this.Discs = [
            "o",
            "m",
            "b",
            "e"
        ];
    }
    // Methods
    public void ParseInput()
    {
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
    public string GetInputMenu()
    {
        PrintHeading("Options:\n");
        foreach (string cmd in Commands)
        {
            PrintHeading($"{cmd}\n");
        }
        Console.Write("> ");
        string input = Console.ReadLine();
        return input.ToLower();
    }

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

    // Later, this will list the discs available, the commands, and an example input for move
    public string GetInputGame()
    {
        Console.WriteLine("Enter a move: ");

        // Validate move 
        // May need to be in a separate method later 
        string input;
        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            if (input.Length != 2)
            {
                PrintError("Invalid Move");
                continue;
            }
            if (!Discs.Contains(input[0].ToString().ToLower())) // If char is NOT IN Discs
            {
                PrintError("Invalid Move - Invalid Disc");
                continue;
            }
            if (int.TryParse(input[1].ToString(), out int result))
            {
                // This uses a hardcoded '8' at the moment.
                // It should connect to Grid's Fields somehow. May need to be passed at constructor
                if (result > 0 && result < 8)
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
        return input.ToLower();
    }
    
    
}