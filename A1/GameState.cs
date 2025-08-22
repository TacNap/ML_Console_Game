public class GameState
{
    // Fields
    private bool GameActive { get; set; } // true if game is currently in progress
    private bool IsAgainstAI { get; set; } // true if game will be between player and AI

    private bool IsPlayerTurn { get; set; } // used to alternate player turns

    private static readonly string[] DiscTypes = { "o", "b", "e" }; // defines which characters can be used in the terminal

    private Grid Grid { get; set; } // Holds game pieces

    private IOHandler IOHandler { get; set; } // Handles input

    // Constructor
    public GameState()
    {
        Grid = new Grid();
        IOHandler = new IOHandler();
    }

    // Methods

    // Handles the input provided from the menu
    public void ParseMenuInput(string input)
    {
        if (input == "/new") // Start new game
        {
            // Determine number of players
            input = IOHandler.GetPlayerCount();
            IsAgainstAI = input == "y" ? true : false;
            GameActive = true;
            GameLoop();
        }
        else if (input == "/load") // Load game from file
        {
            Console.Clear();
            Console.WriteLine("To be implemented...\n");
        }
        else if (input == "/help") // Print game information
        {
            Console.Clear();
            Console.WriteLine("To be implemented...\n");
        }
        else if (input == "/grid") // Change grid size
        {
            Console.Clear();
            Console.WriteLine("Not implemented yet!\n");
        }
        else if (input == "/quit") // Quit program
        {
            Console.WriteLine("Bye bye!");
            GameActive = true; // required to break loop
            return;
        }
        else // Error
        {
            Console.Clear();
            IOHandler.PrintError("Unrecognised command. Try again...\n");
        }
    }

    // Handles the input provided during a game
    // Assumes input begins with "/"
    // Can be similar in functionality to ParseMenuInput
    private void ParseCommand(string input)
    {
        Console.WriteLine("Command gets processed here :)");
    }

    // Used to create a Disc, depending on which turn is active
    public Disc CreateDisc(int type, bool turn)
    {
        if (type == 1)
        {
            return new OrdinaryDisc(turn);
        }
        else if (type == 2)
        {
            return new BoringDisc(turn);
        }
        else if (type == 3)
        {
            return new ExplosiveDisc(turn);
        }
        return new OrdinaryDisc(turn); // Throw error here 
    }


    // Responsible for validating input,
    // adding discs,
    // rendering their effects,
    // and drawing the grid
    // so.. a bit too much at the moment 
    private bool TryParseMove(string input)
    {

        int col, discType;

        // Validate Input
        // Must be 2 characters
        if (input.Length != 2)
        {
            IOHandler.PrintError("Invalid Move");
            return false;
        }

        // Extract disc Type 
        string discStr = input[0].ToString();
        if (!DiscTypes.Contains(discStr))
        {
            IOHandler.PrintError("Invalid Move - Invalid Disc");
            return false;
        }

        // Extract disc Column 
        if (int.TryParse(input[1].ToString(), out col))
        {
            if (col < 1 || col > Grid.GRID_WIDTH)
            {
                IOHandler.PrintError("Invalid Move - Invalid Column");
                return false;
            }
        }
        else
        {
            IOHandler.PrintError("Invalid Move - Invalid Column");
            return false;
        }

        // Convert for SelectDisc parameters
        if (discStr == "b")
        {
            discType = 2;
        }
        else if (discStr == "e")
        {
            discType = 3;
        }
        else
        {
            discType = 1;
        }
        col -= 1;

        // Create the Disc
        Disc disc = CreateDisc(discType, IsPlayerTurn);
        if (Grid.AddDisc(col, disc))
        {
            Console.Clear();
            Grid.RenderGrid(col, disc);
            return true;
        }
        else
        {
            Console.Clear();
            Grid.RenderGrid(); // Call without parameters, so effects aren't rendered
            IOHandler.PrintError("Invalid Move - Column is full");
            return false;
        }
    }

    // The main loop that runs during a game
    public void GameLoop()
    {
        Console.Clear();
        Grid.ClearGrid();
        Grid.DrawGrid();
        string input;

        for (int i = 0; i < 10; i++)
        {
            input = IOHandler.GetPlayerInput();
            if (input.StartsWith("/"))
            {
                ParseCommand(input);
            }
            else
            {
                if (TryParseMove(input))
                {
                    IsPlayerTurn = !IsPlayerTurn;
                }
            }
        }
        IOHandler.PrintHeading("Game Over!");
    }

    // Main entry point.
    // Provides initial menu before launching into a game 
    public void MenuStart()
    {
        Console.Clear();
        while (!GameActive)
        {
            IOHandler.PrintMenuCommands();
            ParseMenuInput(IOHandler.GetInputMenu());
        }

    }

    // Only used to test parts of the program.
    // Not intended to be examined 
    public void GameTest()
    {
        // Add Disc testing
        if (false)
        {
            Disc pDisc = CreateDisc(1, true);
            Disc cDisc = CreateDisc(1, false);
            Disc sDisc = CreateDisc(2, true);

            Console.WriteLine("Initial Grid");
            Grid.DrawGrid();
            Console.WriteLine("Fill Column 2");
            Grid.AddDisc(1, pDisc);
            Grid.AddDisc(1, cDisc);
            Grid.AddDisc(1, pDisc);
            Grid.AddDisc(1, cDisc);
            Grid.AddDisc(1, pDisc);
            Grid.AddDisc(1, cDisc);
            Grid.DrawGrid();

            Console.WriteLine($"Add to full column [False] : {Grid.AddDisc(1, pDisc)}");
            Grid.DrawGrid();

            Console.WriteLine($"Add special to full column [False] : {Grid.AddDisc(1, sDisc)}");
            Grid.DrawGrid();
        }

        // Get Move Input Testing
        if (false)
        {
        }

        // Game Loop Testing
        if (false)
        {
            GameLoop();
        }

        // Menu Commands Testing
        if (true)
        {
            MenuStart();
        }
    }
}