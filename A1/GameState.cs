public class GameState
{
    // Fields
    private bool GameActive { get; set; } // true if game is currently in progress
    private bool Computer { get; set; } // true if game will be between player and AI

    private Grid Grid { get; set; }

    // Constructor
    public GameState()
    {
        Grid = new Grid();
    }

    // Methods

    public void GameLoop()
    {
        Console.Clear();
        PrintHeading("LineUp\n\n");
        Grid.DrawGrid();

        // Get player input
        // This will be moved to an InputHandler method later

        Console.WriteLine("> ");
        string input = Console.ReadLine();




        // split the input into two parts

        // if [0].ToLower() in _datastructure_
        // and [1].IsInt() and (>0, <7)
        // AddDisc(input)
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
    }
    public string InputHandler(int opt)
    {
        if (opt == 1)
        {
            Console.Write("> ");
            PrintHeading("new");
            Console.Write(" game\n");
            Console.Write("> ");
            PrintHeading("load");
            Console.Write(" game\n");
            Console.Write("> ");
            PrintHeading("help\n");
            
            
            Console.Write("> ");
        }
        else if (opt == 2)
        {
            Console.Write("Against AI? {");
            PrintHeading("Y");
            Console.Write("/");
            PrintHeading("N");
            Console.Write("}\n");
            Console.Write("> ");
        }

        string input = Console.ReadLine();
        return input.ToLower();
    }
    public void GameStart()
    {
        Console.Clear();
        while (!GameActive)
        {
            PrintHeading("### Welcome to LineUp ###\n");
            string input = InputHandler(1);

            if (input == "new") // Start new game
            {
                // Determine number of players
                input = InputHandler(2);
                Computer = input == "y" ? true : false;
                GameActive = true;
                GameLoop();
            }
            else if (input == "load")
            {
                Console.WriteLine("To be implemented...\n");
            }
            else if (input == "help")
            {
                Console.WriteLine("To be implemented...\n");
            }
            else
            {
                PrintError("Unrecognised command. Try again...\n");
            }

        }

    }
}