

public class GameController
{
    // Fields
    private bool IsGameActive { get; set; } // true if game is currently in progress
    private bool IsAgainstAI { get; set; } // true when game mode is HvC
    private bool IsPlayerTurn { get; set; } = true; // used to alternate player turns
    public bool PlayerOneWin = false; // true if CheckWinCondition finds a win for P1
    public bool PlayerTwoWin = false; // true if CheckWinCondition finds a win for P2

    // Discs remaining for P1
    Dictionary<string, int> P1Discs = new Dictionary<string, int>
    {
        ["Ordinary"] = 17,
        ["Boring"] = 2,
        ["Explosive"] = 2
    };

    // Discs remaining for P2
   Dictionary<string, int> P2Discs = new Dictionary<string, int>
    {
        ["Ordinary"] = 17,
        ["Boring"] = 2,
        ["Explosive"] = 2
    };

    private static readonly string[] DiscTypes = { "o", "b", "e" }; // defines which characters can be used in the terminal

    private Grid Grid { get; set; } // Holds game discs

    private IOHandler IOHandler { get; set; } // Handles some input and printing to console

    private FileController FileController { get; set; } // Handles file operations

    // Constructor
    public GameController()
    {
        Grid = new Grid();
        IOHandler = new IOHandler();
        FileController = new FileController();
    }

    // Methods

    /// <summary>
    /// Handles the user input from the main menu
    /// </summary>
    public void ParseMenuInput(string input)
    {
        if (input == "/new") // Start new game
        {
            // Game Mode (HvC or HvH)
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
        else if (input == "/testing") // Testing mode 
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

    /// <summary>
    /// Handles the user input during the game
    /// Assumes input starts with "/",
    /// otherwise this method would not be called
    /// </summary>
    private void ParseCommand(string input)
    {
        if (input == "/save") // Save to file
        {
            Save();
            Grid.DrawGrid();
        }
        else if (input == "/help") // Print game information
        {
            IOHandler.PrintGameHelp(Grid.WinLength);
            Grid.DrawGrid();
        }
        else if (input == "/quit") // Return to menu
        {
            Console.Clear();
            IOHandler.PrintGreen("Quit Game!");
            IsGameActive = false;
            return;
        }
        else // Error
        {
            Console.Clear();
            IOHandler.PrintError("Unrecognised command. Try again...");
            Grid.DrawGrid();
        }
    }

    /// <summary>
    /// Prints player information during the main game loop:
    /// Game mode, turn counter, current turn, discs remaining, etc.  
    /// </summary>
    private void PrintPlayerData()
    {
        string mode = IsAgainstAI ? "HvC" : "HvH";
        Dictionary<string, int> PlayerDiscs = IsPlayerTurn ? P1Discs : P2Discs; // Holds reference to relevent dictionary
        string player = IsPlayerTurn ? "Player 1 Turn" : "Player 2 Turn";
        // holds strings that contain ◉ characters, depending on discs remaining
        string ordinary = new string('◉', Math.Min(PlayerDiscs["Ordinary"], 17)); // max out at 15 
        string boring = new string('◉', PlayerDiscs["Boring"]);
        string explosive = new string('◉', PlayerDiscs["Explosive"]);

        // Printing
        // I apologise to whoever has to read this:
        Console.WriteLine("╔═══════════════════════════════════════╗");
        Console.WriteLine($"║   Turn{Grid.TurnCounter,3}                      {mode}    ║");
        Console.WriteLine($"║             {player,13}             ║");
        Console.WriteLine("╚═══════════════════════════════════════╝");

        // Print Ordinary - only print 15, the add a "+ x" for remaining discs
        Console.Write($"║ Ordinary : ");
        Console.Write($"{ordinary,-17}");
        if (PlayerDiscs["Ordinary"] > 17)
        {
            Console.Write($" +{PlayerDiscs["Ordinary"] - 17,3}     ║");
        }
        else
        {
            Console.Write($"          ║");
        }
        Console.WriteLine();

        // Print Boring
        Console.Write($"║ Boring   : ");
        Console.Write($"{boring,-2}");
        Console.Write("                         ║\n");

        // Print Explosive
        Console.Write($"║ Explosive: ");
        Console.Write($"{explosive,-2}");
        Console.Write("                         ║\n");
        Console.WriteLine("╚═══════════════════════════════════════╝");
    }

    /// <summary>
    /// Used to create new Disc objects during gameplay
    /// </summary>
    /// <param name="type">just a number to define which type of disc</param>
    /// <param name="turn">each Disc belongs to a player, defined by the current turn</param>
    /// <returns>a Disc object</returns>
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
        return new OrdinaryDisc(turn); // Default to ordinary disc to continue gameplay
    }

    /// <summary>
    /// Handles user input from game loop, and tries to place a Disc into the board.
    /// If this method is called, input is not a command.
    /// This method is very bloated... 
    /// </summary>
    /// <returns>true if a disc was successfully placed on the board</returns>
    private bool TryParseMove(string input)
    {

        int col, discType;

        // Validate Input
        // Must be 2 or 3 characters in length
        if (input.Length < 2 || input.Length > 3)
        {
            Console.Clear();
            IOHandler.PrintError("Invalid Move");
            return false;
        }

        // Extract disc Type - first character
        string discStr = input[0].ToString();
        if (!DiscTypes.Contains(discStr))
        {
            Console.Clear();
            IOHandler.PrintError("Invalid Move - Invalid Disc");
            return false;
        }

        // Extract disc Column - remaining characters
        if (int.TryParse(input.Substring(1), out col))
        {
            if (col < 1 || col > Grid.GRID_WIDTH)
            {
                Console.Clear();
                IOHandler.PrintError("Invalid Move - Invalid Column");
                return false;
            }
        }
        else
        {
            Console.Clear();
            IOHandler.PrintError("Invalid Move - Invalid Column");
            return false;
        }

        // Convert first character to a number, so it can be passed to CreateDisc
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

        // Make sure the player has discs available to place
        if (!HasDiscRemaining(discType, IsPlayerTurn))
        {
            Console.Clear();
            IOHandler.PrintError("Invalid Move - No discs of that type remaining");
            return false;
        }
        else
        {
            if (!Grid.AddDisc(col, disc)) // Attempt to place the disc 
            {
                Console.Clear();
                IOHandler.PrintError("Invalid Move - Column is full");
                return false;
            }
            else
            {
                // Successful Placement
                // Effects are applied and rendered here
                Console.Clear();
                Grid.IncrementTurnCounter();
                Grid.RenderGrid(col, disc);
                WithdrawDisc(discType, IsPlayerTurn);
                return true;
            }
        }
    }

    /// <summary>
    /// This method is called if there's no 'winning move' for the AI to play.
    /// It will instead place a random disc in random column.
    /// Weighted to prefer Ordinary discs. 
    /// </summary>
    private void AIMakeMove()
    {
        Disc disc; // Disc to be placed
        int discType;
        Random rand = new Random();

        // Determine random Disc 
        int roll = rand.Next(1, 11);
        if (roll < 9 && HasDiscRemaining(1, false))
        {
            disc = new OrdinaryDisc(false);
            discType = 1;
        }
        else if (HasDiscRemaining(2, false))
        {
            disc = new BoringDisc(false);
            discType = 2;
        }
        else if (HasDiscRemaining(3, false))
        {
            disc = new ExplosiveDisc(false);
            discType = 3;
        }
        else
        {
            return;
        }

        // Determine random Column
        int col;
        while (true)
        {
            col = rand.Next(0, Grid.GRID_WIDTH);
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

    /// <summary>
    /// Load game state from file. 
    /// This method is largely responsible for input handling and output
    /// </summary>
    private void Load()
    {
        try
        {
            string[] saveFiles; // Array of files in 'Saves' folder
            string path; // Path of chosen file to load 

            // Check if there's a "Saves" folder
            try
            {
                saveFiles = Directory.GetFiles("Saves");
                if (saveFiles.Length == 0)
                {
                    Console.Clear();
                    IOHandler.PrintError("Error: No files found in Saves folder!");
                    return;
                }

            }
            catch (DirectoryNotFoundException e)
            {
                Console.Clear();
                IOHandler.PrintError($"You're missing a Saves folder! {e.Message}");
                return;
            }
            catch (Exception e)
            {
                Console.Clear();
                IOHandler.PrintError($"Error: {e.Message}");
                return;
            }


            // Print available files to console
            IOHandler.PrintLoadBanner();
            Console.WriteLine("Please input the number of the file you'd like to load:");
            for (int i = 0; i < saveFiles.Length; i++)
            {
                IOHandler.PrintHeading($"{i + 1}. ");
                Console.WriteLine($"{Path.GetFileName(saveFiles[i])}");
            }
            // Get input for chosen file 
            string input = Console.ReadLine();
            try
            {
                int num = Int32.Parse(input);
                if (num < 1 || num > saveFiles.Length)
                {
                    Console.Clear();
                    IOHandler.PrintError($"Error: Input must be between 1 and {saveFiles.Length}\n");
                    return;
                }
                path = saveFiles[num - 1];

            }
            catch (Exception e)
            {
                Console.Clear();
                IOHandler.PrintError($"Error: {e.Message}\n");
                return;
            }

            // Unable to pass properties by reference.
            // So, i had to use a local variable here to set it during deserialization:
            bool playerTurn = IsPlayerTurn;
            bool isAgainstAI = IsAgainstAI;

            // Deserialization
            ResetGame();
            Grid = FileController.GridDeserialization(path, P1Discs, P2Discs, ref playerTurn, ref isAgainstAI);
            IsPlayerTurn = playerTurn;
            IsAgainstAI = isAgainstAI;
            IsGameActive = true;
            Console.Clear();
            IOHandler.PrintGreen("Game Loaded!");
            GameLoop(false);
        }
        catch (Exception e)
        {
            Console.Clear();
            IOHandler.PrintError($"Error: This file could not be read. {e.Message}");
        }
    }

    /// <summary>
    /// Will always attempt to save the file to a folder 'Saves'
    /// Name is automatic, with a time-stamp appended
    /// </summary>
    private void Save()
    {
        try
        {
            // Generate filename
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = "Game " + timestamp + ".csv";
            string folder = "Saves";

            // Check if Saves folder exists, otherwise make it
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

            // Serialization
            FileController.GridSerialization(Path.Combine(folder, fileName), Grid, P1Discs, P2Discs, IsPlayerTurn, IsAgainstAI);
            Console.Clear();
            IOHandler.PrintGreen($"Game saved to '{Path.Combine(folder, fileName)}'");
        }
        catch (Exception e)
        {
            Console.Clear();
            IOHandler.PrintError($"Error: {e.Message}");
        }
    }

    /// <summary>
    /// Allows the user to input a sequence of moves that are played in succession.
    /// Only the final result is returned - game cannot be played from this point.
    /// </summary>
    private void Testing()
    {
        IOHandler.PrintTestingMode();

        // Get Sequence
        Console.WriteLine("\nEnter your sequence:");
        Console.Write("> ");
        string input = Console.ReadLine();
        string[] moves = input.Split(',');

        if (moves[0] == "")
        {
            IOHandler.PrintError("Error: Sequence must contain atleast one move");
        }

        // Execute Sequence
        ResetGame();
        foreach (string move in moves)
        {
            Console.Clear();
            if (!TryParseMove(move.Trim().ToLower())) // If a move fails, stop here
            {
                Grid.DrawGrid();
                PrintPlayerData();
                break;
            }
                PrintPlayerData();
            if (Grid.CheckWinCondition(ref PlayerOneWin, ref PlayerTwoWin)) // If there's a win, stop here
            {
                IOHandler.PrintWinner(PlayerOneWin, PlayerTwoWin);
                break;
            }
            IsPlayerTurn = !IsPlayerTurn;

        }
        Console.WriteLine("Press enter to continue...");
        Console.ReadLine();
        Console.Clear();

    }

    /// <summary>
    /// Alters the size of the playable grid, and changes WinLength & Disc amounts accordingly
    /// </summary>
    private void ChangeGridSize()
    {
        // Get input
        (int height, int width) = IOHandler.GetInputGridSize();
        // Change the grid
        Grid.SetGridSize(height, width);
        // Change disc amount for each player 
        P1Discs["Ordinary"] = (height * width / 2) - 4;
        P2Discs["Ordinary"] = (height * width / 2) - 4;
        P1Discs["Boring"] = 2;
        P2Discs["Boring"] = 2;
        P1Discs["Explosive"] = 2;
        P2Discs["Explosive"] = 2;

        Console.Clear();
    }


    /// <summary>
    /// Resets the game, for use before starting a second game in a single session, or when loading 
    /// </summary>
    private void ResetGame()
    {
        // Reset turn
        IsPlayerTurn = true;
        PlayerOneWin = false;
        PlayerTwoWin = false;

        // Empty all discs, reset turn counter
        Grid.ClearGrid();
        Grid.SetTurnCounter(1);

        // Reset disc amounts
        P1Discs["Ordinary"] = (Grid.GRID_HEIGHT * Grid.GRID_WIDTH / 2) - 4;
        P2Discs["Ordinary"] = (Grid.GRID_HEIGHT * Grid.GRID_WIDTH / 2) - 4;
        P1Discs["Boring"] = 2;
        P2Discs["Boring"] = 2;
        P1Discs["Explosive"] = 2;
        P2Discs["Explosive"] = 2;
    }

    /// <summary>
    /// Checks if the player has discs remaining of the specified type
    /// </summary>
    /// <param name="discType">just a number that specifies the disc type</param>
    /// <param name="IsPlayerTurn">used to specify which Disc Dictionary to check</param>
    /// <returns>true if there is at least one disc remaining</returns>
    private bool HasDiscRemaining(int discType, bool IsPlayerTurn)
    {
        if (discType == 1) // Ordinary
        {
            return IsPlayerTurn ? P1Discs["Ordinary"] > 0 : P2Discs["Ordinary"] > 0;
        }
        else if (discType == 2) // Boring
        {
            return IsPlayerTurn ? P1Discs["Boring"] > 0 : P2Discs["Boring"] > 0;
        }
        else if (discType == 3) // Explosive
        {
            return IsPlayerTurn ? P1Discs["Explosive"] > 0 : P2Discs["Explosive"] > 0;
        }
        return false;
    }

    /// <summary>
    /// Reduces the player's disc amount by 1.
    /// </summary>
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

    /// <summary>
    /// Used to check if both players have atlest one disc remaining
    /// </summary>
    /// <returns>true if both players have 0 discs</returns>
    private bool IsTieGame()
    {
        if (
            !HasDiscRemaining(1, true) &&
            !HasDiscRemaining(2, true) &&
            !HasDiscRemaining(3, true) &&
            !HasDiscRemaining(1, false) &&
            !HasDiscRemaining(2, false) &&
            !HasDiscRemaining(3, false)
            )
        {
            PlayerOneWin = true;
            PlayerTwoWin = true;
            return true;
        }
        return false;
    }

    // The main loop that runs during a game
    public void GameLoop(bool IsNewGame = true)
    {
        // Fresh start
        if (IsNewGame)
        {
            Console.Clear();
            ResetGame();
        }
        Grid.DrawGrid();

        // Main loop
        while (IsGameActive)
        {
            // Check if players have discs remaining
            if (IsTieGame())
            {
                IsGameActive = false;
                IOHandler.PrintWinner(PlayerOneWin, PlayerTwoWin);
                break;
            }

            // Check if its a player turn or AI turn
            if (IsAgainstAI && !IsPlayerTurn)
            {
                // AI Logic 
                // Check if there's a winning move, otherwise place a random disc
                if (!Grid.AIFindWinningMove(P2Discs, ref PlayerTwoWin))
                {
                    AIMakeMove();
                }
                else
                {
                    // Winning move found. Play it and end game
                    Console.Clear();
                    Grid.DrawGrid();
                    Grid.CheckWinCondition(ref PlayerOneWin, ref PlayerTwoWin);
                    IsGameActive = false;
                    IOHandler.PrintWinner(PlayerOneWin, PlayerTwoWin);
                    break;
                }
                IsPlayerTurn = !IsPlayerTurn;
                continue;
            }
            else
            {
                // Printing to console for human players
                PrintPlayerData();
            }


            // Get Player Input and Process
            string input = IOHandler.GetPlayerInput();
            if (input.StartsWith("/")) // Process command
            {
                ParseCommand(input);
            }
            else // Process move
            {
                if (TryParseMove(input))
                {
                    if (Grid.CheckWinCondition(ref PlayerOneWin, ref PlayerTwoWin))
                    {
                        // Win
                        IsGameActive = false;
                        IOHandler.PrintWinner(PlayerOneWin, PlayerTwoWin);
                    }
                    IsPlayerTurn = !IsPlayerTurn;
                }
                else
                {
                    Grid.DrawGrid();
                }
            }
        }
    }

    /// <summary>
    /// Main entry point to the program. Brings user to Main Menu 
    /// </summary>
    public void MenuStart()
    {
        Console.Clear(); 
        while (!IsGameActive)
        {
            IOHandler.PrintMenuCommands();
            ParseMenuInput(IOHandler.GetInputMenu());
        }
    }
}