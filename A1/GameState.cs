public class GameState
{
    // Fields
    private bool GameActive { get; set; } // true if game is currently in progress
    private bool Computer { get; set; } // true if game will be between player and AI

    private bool Turn { get; set; } // used to alternate player turns

    private Grid Grid { get; set; } // Holds game pieces

    private IOHandler IOHandler { get; set; } // Handles input

    // Constructor
    public GameState()
    {
        Grid = new Grid();
        IOHandler = new IOHandler();
    }

    // Methods

    public void GameLoop()
    {
        Console.Clear();
        Grid.ClearGrid();
        IOHandler.PrintHeading("LineUp\n\n");
        Grid.DrawGrid();
        
        // While GameActive() - win condition will set to false
        for (int i = 0; i < 5; i++)
        {
            // Get and validate input from terminal
            (int col, int type) = IOHandler.GetInputGame();
            Grid.AddDisc(col, type, Turn);

            // Render Grid
            Console.Clear();
            Grid.DrawGrid();

            // Switch Player Turn
            Turn = !Turn;
        }
        IOHandler.PrintHeading("Game Over!");
    }

    
    public void GameStart()
    {
        Console.Clear();
        while (!GameActive)
        {
            IOHandler.PrintHeading("### Welcome to LineUp ###\n");
            string input = IOHandler.GetInputMenu();

            if (input == "new") // Start new game
            {
                // Determine number of players
                input = IOHandler.GetInputPlayers();
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
                IOHandler.PrintError("Unrecognised command. Try again...\n");
            }

        }

    }

    public void GameTest()
    {
        // Add Disc testing
        if (false)
        {
            Grid.DrawGrid();

            Grid.AddDisc(1, 1, true);
            Grid.AddDisc(1, 1, false);
            Grid.AddDisc(1, 1, true);
            Grid.AddDisc(1, 1, false);
            Grid.AddDisc(1, 1, true);
            Grid.AddDisc(1, 1, false);
            Grid.AddDisc(1, 1, true); // Column is full
            Grid.DrawGrid();

            Grid.DrawGrid();

            // Clear Grid
            Grid.ClearGrid();
            Grid.AddDisc(1, 1, true);
            Grid.AddDisc(1, 2, true);
            Grid.AddDisc(1, 3, true);
            Grid.AddDisc(1, 4, true);

            Grid.AddDisc(2, 1, false);
            Grid.AddDisc(2, 2, false);
            Grid.AddDisc(2, 3, false);
            Grid.AddDisc(2, 4, false);
            Grid.DrawGrid();
        }

        // Get Move Input Testing
        if (false)
        {
            Grid.ClearGrid();
            for (int i = 0; i < 5; i++)
            {
                (int col, int type) = IOHandler.GetInputGame();
                Grid.AddDisc(col, type, (i % 2 == 0 ? true : false));
            }
            Grid.DrawGrid();
        }

        // Game Loop Testing
        if (false)
        {
            GameLoop();
        }
        


        

        
    }
}