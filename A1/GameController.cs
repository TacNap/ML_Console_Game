public class GameController
{
    // Fields
    private bool IsGameActive { get; set; } // true if game is currently in progress
    private bool IsAgainstAI { get; set; } // true if game will be between player and AI

    private bool IsPlayerTurn { get; set; } = true; // used to alternate player turns

   Dictionary<string, int> P1Discs = new Dictionary<string, int>
    {
        ["Ordinary"] = 17,
        ["Boring"] = 2,
        ["Explosive"] = 2
    };

   Dictionary<string, int> P2Discs = new Dictionary<string, int>
    {
        ["Ordinary"] = 17,
        ["Boring"] = 2,
        ["Explosive"] = 2
    };


    private static readonly string[] DiscTypes = { "o", "b", "e" }; // defines which characters can be used in the terminal

    private Grid Grid { get; set; } // Holds game pieces

    private IOHandler IOHandler { get; set; } // Handles input

    private FileController FileController { get; set; } // Handles file operations

    // Constructor
    public GameController()
    {
        Grid = new Grid();
        IOHandler = new IOHandler();
        FileController = new FileController();
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
            IsGameActive = true;
            GameLoop();
        }
        else if (input == "/load") // Load game from file
        {
            Console.Clear();
            Load();
        }
        else if (input == "/help") // Print game information
        {
            Console.Clear();
            IOHandler.PrintMenuHelp();
        }
        else if (input == "/grid") // Change grid size
        {
            Console.Clear();
            ChangeGridSize();
        }
        else if (input == "/quit") // Quit program
        {
            Console.WriteLine("Bye bye!");
            IsGameActive = true; // required to break loop
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
        if (input == "/save") // Save to file
        {
            Save();
        }
        else if (input == "/help") // Print game information
        {
            IOHandler.PrintGreen("To be implemented...\n");
        }
        else if (input == "/quit") // Return to menu
        {
            IOHandler.PrintGreen("I don't work yet!");
            return;
        }
        else // Error
        {
            IOHandler.PrintError("Unrecognised command. Try again...\n");
        }
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

        if (!HasDiscRemaining(discType, IsPlayerTurn))
        {
            Console.Clear();
            Grid.RenderGrid(); // Call without parameters, so effects aren't rendered
            IOHandler.PrintError("Invalid Move - No discs of that type remaining");
            return false;
        }
        else
        {
            if (!Grid.AddDisc(col, disc))
            {
                Console.Clear();
                Grid.RenderGrid(); // Call without parameters, so effects aren't rendered
                IOHandler.PrintError("Invalid Move - Column is full");
                return false;
            }
            else
            {
                // Successful Placement
                Console.Clear();
                Grid.RenderGrid(col, disc);
                WithdrawDisc(discType, IsPlayerTurn);
                return true;
            }
            
        }
            
        
    }

    // Load game state from file
    private void Load()
    {
        try
        {
            Grid = FileController.GridDeserialization("Objects/grid.csv");
            IsGameActive = true;
            Console.Clear();
            IOHandler.PrintGreen("Successfully loaded game");
            GameLoop(false);
        }
        catch (Exception e)
        {
            Console.WriteLine("sumting went wrong");
        }
    }

    // Save gamestate to file
    private void Save()
    {
        try
        {
            // Add a time stamp to the filename
            FileController.GridSerialization("Objects/grid.csv", Grid);
            IOHandler.PrintGreen("Successfully saved to 'Objects/grid.csv'");
        }
        catch (Exception e)
        {
            Console.WriteLine("sumting went wrong");
        }
    }

    // Alters the size of the playable grid
    private void ChangeGridSize()
    {
        (int height, int width) = IOHandler.GetInputGridSize();
        Grid.SetGridSize(height, width);
        // Change disc amount for each player 
        P1Discs["Ordinary"] = (height * width / 2) - 4;
        P2Discs["Ordinary"] = (height * width / 2) - 4;
    }

    // Checks the player's disc balance for the given type.
    // Returns true if they have atleast 1 disc of that type remaining
    private bool HasDiscRemaining(int discType, bool IsPlayerTurn)
    {
        if (discType == 1)
        {
            return IsPlayerTurn ? P1Discs["Ordinary"] > 0 : P2Discs["Ordinary"] > 0;
        }
        else if (discType == 2)
        {
            return IsPlayerTurn ? P1Discs["Boring"] > 0 : P2Discs["Boring"] > 0;
        }
        else if (discType == 3)
        {
            return IsPlayerTurn ? P1Discs["Explosive"] > 0 : P2Discs["Explosive"] > 0;
        }
        return false;
    }

    // Reduces the players disc amount by 1
    private void WithdrawDisc(int discType, bool IsPlayerTurn)
    {
        if (discType == 1)
        {
            if (IsPlayerTurn)
            {
                P1Discs["Ordinary"]--;
            }
            else
            {
                P2Discs["Ordinary"]--;
            }

        }
        else if (discType == 2)
        {
            if (IsPlayerTurn)
            {
                P1Discs["Boring"]--;
            }
            else
            {
                P2Discs["Boring"]--;
            }
        }
        else if (discType == 3)
        {
            if (IsPlayerTurn)
            {
                P1Discs["Explosive"]--;
            }
            else
            {
                P2Discs["Explosive"]--;
            }
        }
    }

    
    // The main loop that runs during a game
    public void GameLoop(bool IsNewGame = true)
    {
        if (IsNewGame)
        {
            Console.Clear();
            Grid.ClearGrid();
        }
        Grid.DrawGrid();
        string input;

        while (IsGameActive)
        {
            // Console Printing
            if (IsPlayerTurn)
            {
                Console.WriteLine("# Player 1 Turn #");
                Console.WriteLine($"Ordinary Discs: {P1Discs["Ordinary"]}");
                Console.WriteLine($"Boring Discs: {P1Discs["Boring"]}");
                Console.WriteLine($"Explosive Discs: {P1Discs["Explosive"]}");
            }
            else if (!IsAgainstAI)
            {
                Console.WriteLine("# Player 2 Turn #");
                Console.WriteLine($"Ordinary Discs: {P2Discs["Ordinary"]}");
                Console.WriteLine($"Boring Discs: {P2Discs["Boring"]}");
                Console.WriteLine($"Explosive Discs: {P2Discs["Explosive"]}");
            }
            else // This will be deleted later
            {
                Console.WriteLine("! AI Turn - Testing !");
            }

            input = IOHandler.GetPlayerInput();
            if (input.StartsWith("/"))
            {
                ParseCommand(input);
            }
            else
            {
                if (TryParseMove(input))
                {
                    if (Grid.CheckWinCondition())
                    {
                        IsGameActive = false;
                    }
                    IsPlayerTurn = !IsPlayerTurn;
                }
            }
        }
    }

    // Main entry point.
    // Provides initial menu before launching into a game 
    public void MenuStart()
    {
        Console.Clear();
        while (!IsGameActive)
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

        // Explosive Disc Testing
        if (false)
        {
            Disc oDisc = CreateDisc(1, true);
            Disc eDisc = CreateDisc(3, true);

            Console.WriteLine("Fill will Ordinary Discs");

            
            
            Grid.AddDisc(1, oDisc);
            Grid.AddDisc(1, oDisc);
            Grid.AddDisc(1, oDisc);
            Grid.AddDisc(1, oDisc);
            Grid.AddDisc(1, oDisc);
            Grid.AddDisc(1, oDisc);

            Grid.AddDisc(2, oDisc);
            Grid.AddDisc(2, oDisc);
            Grid.AddDisc(2, oDisc);
            Grid.AddDisc(2, oDisc);
            Grid.AddDisc(2, oDisc);

            
            Grid.DrawGrid();

            Console.WriteLine($"Add explosive disc to column 3 [True] : {Grid.AddDisc(0, eDisc)}");
            Console.WriteLine("Render Effects");
            Grid.RenderGrid(0, eDisc);
        }

        // Get Move Input Testing
        if (false)
        {
        }

        // Game Loop Testing
        if (false)
        {
            IsGameActive = true;
            GameLoop();
        }

        // File Operations Testing
        if (false)
        {
            OrdinaryDisc disc = new OrdinaryDisc(true);
            BoringDisc bdisc = new BoringDisc(false);
            Grid grid = new Grid();
            grid.AddDisc(1, disc);
            grid.AddDisc(1, disc);
            grid.AddDisc(1, disc);
            grid.AddDisc(1, disc);
            grid.AddDisc(2, bdisc);
            grid.DrawGrid();
            FileController.GridSerialization("Objects/grid.csv", grid);

            Grid loadGrid = FileController.GridDeserialization("Objects/grid.csv");
            loadGrid.DrawGrid();
            loadGrid.AddDisc(1, bdisc);
            loadGrid.DrawGrid();

        }

        // From start test
        if (true)
        {
                MenuStart();
        }
    }
}