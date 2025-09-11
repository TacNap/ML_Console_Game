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
        else if (input == "/testing")
        {
            Testing();
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
            Console.Clear();
            IOHandler.PrintGreen("Successfully quit game");
            IsGameActive = false;
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
        if (input.Length < 2 || input.Length > 3)
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
        if (int.TryParse(input.Substring(1), out col))
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
                Grid.IncrementTurnCounter();
                Grid.RenderGrid(col, disc);
                WithdrawDisc(discType, IsPlayerTurn);
                return true;
            }
            
        }
            
        
    }



    private void AIMakeMove()
    {
        Disc disc; // Disc to be placed
        int discType;
        Random rand = new Random();

        // Determine Disc Type
        int roll = rand.Next(1, 11);
        if (roll < 7)
        {
            disc = new OrdinaryDisc(false);
            discType = 1;
        }
        else if (roll == 7 || roll == 8)
        {
            disc = new BoringDisc(false);
            discType = 2;
        }
        else
        {
            disc = new ExplosiveDisc(false);
            discType = 3;
        }

        // Determine Column Placement
        int col;
        while (true)
        {
            col = rand.Next(1, Grid.GRID_WIDTH);
            if (Grid.AddDisc(col, disc))
            {
                break;
            }
        }

        // Apply Effects and Render Grid
        Console.Clear();
        Grid.IncrementTurnCounter();
        Grid.RenderGrid(col, disc);
        WithdrawDisc(discType, false);

    }
    // Load game state from file
    private void Load()
    {
        try
        {
            // Unable to pass properties by reference.
            // So, i had to use a local variable here to set it during deserialization:
            bool playerTurn = IsPlayerTurn;
            bool isAgainstAI = IsAgainstAI;

            string[] saveFiles;
            string path;
            
            try
            {
                saveFiles = Directory.GetFiles("Saves");

            }
            catch (DirectoryNotFoundException e)
            {
                IOHandler.PrintError($"You're missing a Saves folder! {e.Message}");
                return;
            }
            catch (Exception e)
            {
                IOHandler.PrintError($"Error: {e.Message}");
                return;
            }

            Console.WriteLine("Please input the number of the file you'd like to load:");
            for (int i = 0; i < saveFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(saveFiles[i])}");
            }
            string input = Console.ReadLine();
            try
            {
                int num = Int32.Parse(input);
                if (num < 1 || num > saveFiles.Length)
                {
                    IOHandler.PrintError($"Error: Must be between 1 and {saveFiles.Length}");
                    return;
                }
                path = saveFiles[num - 1];

            }
            catch (Exception e)
            {
                IOHandler.PrintError($"Error: {e.Message}");
                return;
            }
            
            // Get input to determine which one to load
            // Deserialization
            Grid = FileController.GridDeserialization(path, P1Discs, P2Discs, ref playerTurn, ref isAgainstAI);
            IsPlayerTurn = playerTurn;
            IsAgainstAI = isAgainstAI;

            IsGameActive = true;
            Console.Clear();
            IOHandler.PrintGreen("Successfully loaded game");
            GameLoop(false);
        }
        catch (Exception e)
        {
            Console.WriteLine($"sumting went wrong: {e.Message}");
        }
    }

    // Save gamestate to file
    private void Save()
    {
        try
        {
            // Add a time stamp to the filename
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folder = "Saves";
            string fileName = "Game " + timestamp + ".csv";
            // string fileName = timestamp
            // if !exists folder, make it 
            if (!Path.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception e)
                {
                    IOHandler.PrintError($"Error: unable to make new directory, {e.Message}");
                }
            }
            FileController.GridSerialization(Path.Combine(folder, fileName), Grid, P1Discs, P2Discs, IsPlayerTurn, IsAgainstAI);
            IOHandler.PrintGreen($"Successfully saved to {Path.Combine(folder, fileName)}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"sumting went wrong: {e.Message}");
        }
    }

    // Allow the user to input a sequence of moves and render the result
    private void Testing()
    {
        // Temporary Bool 
        bool IsPlayerOneWin = false;

        Console.Clear();
        IOHandler.PrintGreen("Testing");
        Console.WriteLine("You can use this feature to input a sequence of moves and render the final result.");
        Console.WriteLine("Input a single string of moves [disc,column], separated commas (,). For example:");
        IOHandler.PrintHeading("o1,o2,o3,e2,o1,b1\n");
        // Get Sequence
        Console.WriteLine("\nEnter your sequence:");
        string input = Console.ReadLine();
        string[] moves = input.Split(',');

        if (moves[0] == "")
        {
            IOHandler.PrintError("Error: Sequence must contain atleast one move");
        }

        // Execute Sequence
        Grid.ClearGrid();
        foreach (string move in moves)
        {
            if (!TryParseMove(move.Trim().ToLower()))
            {
                break;
            }
            if (Grid.CheckWinCondition(ref IsPlayerOneWin))
            {
                IOHandler.PrintWinner(IsPlayerOneWin);
                break;
            }
            IsPlayerTurn = !IsPlayerTurn;
        }
        Grid.DrawGrid();
        Console.WriteLine("Press enter to continue...");
        Console.ReadLine();
        Console.Clear();

    }
    // Alters the size of the playable grid
    private void ChangeGridSize()
    {
        (int height, int width) = IOHandler.GetInputGridSize();
        Grid.SetGridSize(height, width);
        // Change disc amount for each player 
        P1Discs["Ordinary"] = (height * width / 2) - 4;
        P2Discs["Ordinary"] = (height * width / 2) - 4;
        P1Discs["Boring"] = 2;
        P2Discs["Boring"] = 2;
        P1Discs["Explosive"] = 2;
        P2Discs["Explosive"] = 2;
    }


    private void ResetGame()
    {
        Grid.ClearGrid();
        Grid.SetTurnCounter(1);
        P1Discs["Ordinary"] = (Grid.GRID_HEIGHT * Grid.GRID_WIDTH / 2) - 4;
        P2Discs["Ordinary"] = (Grid.GRID_HEIGHT * Grid.GRID_WIDTH / 2) - 4;
        P1Discs["Boring"] = 2;
        P2Discs["Boring"] = 2;
        P1Discs["Explosive"] = 2;
        P2Discs["Explosive"] = 2;
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
        // Temporary Bool while i fix other stuff 
        bool IsPlayerOneWin = false;


        if (IsNewGame)
        {
            Console.Clear();
            ResetGame();
        }
        Grid.DrawGrid();
        string input;

        while (IsGameActive)
        {
            // Console Printing
            string mode = IsAgainstAI ? "HvC" : "HvH";
            if (IsPlayerTurn) // Player 1 turn
            {
                Console.WriteLine($"    {mode}    ");
                Console.WriteLine($"Turn: {Grid.TurnCounter}");
                Console.WriteLine("# Player 1 Turn #");
                Console.WriteLine($"Ordinary Discs: {P1Discs["Ordinary"]}");
                Console.WriteLine($"Boring Discs: {P1Discs["Boring"]}");
                Console.WriteLine($"Explosive Discs: {P1Discs["Explosive"]}");
            }
            else if (!IsAgainstAI) // Player 2 HvH turn
            {
                Console.WriteLine($"    {mode}    ");
                Console.WriteLine($"Turn: {Grid.TurnCounter}");
                Console.WriteLine("# Player 2 Turn #");
                Console.WriteLine($"Ordinary Discs: {P2Discs["Ordinary"]}");
                Console.WriteLine($"Boring Discs: {P2Discs["Boring"]}");
                Console.WriteLine($"Explosive Discs: {P2Discs["Explosive"]}");
            }
            else // AI Turn
            {
                Console.WriteLine("! AI Turn - Testing !");
                // I could make local variables here and pass to Grid?
                if (!Grid.AIFindWinningMove(P2Discs))
                {
                    AIMakeMove();
                }
                else // If AI finds a Winning Move 
                {
                    Grid.DrawGrid();
                    IOHandler.PrintWinner(false);
                    IsGameActive = false;
                    continue;
                }
                IsPlayerTurn = !IsPlayerTurn;
                continue;
            }


            // Get Player Input and Process Move 
            input = IOHandler.GetPlayerInput();
            if (input.StartsWith("/"))
            {
                ParseCommand(input);
            }
            else
            {
                if (TryParseMove(input))
                {
                    if (Grid.CheckWinCondition(ref IsPlayerOneWin))
                    {
                        IsGameActive = false;
                        IOHandler.PrintWinner(IsPlayerOneWin);
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

        }

        // From start test
        if (true)
        {
            MenuStart();
        }
    }
}