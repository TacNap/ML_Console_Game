public class GameState
{
    // Fields
    private bool GameActive { get; set; } // true if game is currently in progress
    private bool Computer { get; set; } // true if game will be between player and AI

    private Grid Grid { get; set; } // Holds game pieces

    private InputHandler InputHandler { get; set; } // Handles input

    // Constructor
    public GameState()
    {
        Grid = new Grid();
        InputHandler = new InputHandler();
    }

    // Methods

    public void GameLoop()
    {
        Console.Clear();
        InputHandler.PrintHeading("LineUp\n\n");
        Grid.DrawGrid();

        // Get player input
        // This will be moved to an InputHandler method later

        




        // split the input into two parts

        // if [0].ToLower() in _datastructure_
        // and [1].IsInt() and (>0, <7)
        // AddDisc(input)
    }

    
    public void GameStart()
    {
        Console.Clear();
        while (!GameActive)
        {
            InputHandler.PrintHeading("### Welcome to LineUp ###\n");
            string input = InputHandler.ListCommands(1);

            if (input == "new") // Start new game
            {
                // Determine number of players
                input = InputHandler.ListCommands(2);
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
                InputHandler.PrintError("Unrecognised command. Try again...\n");
            }

        }

    }

    public void GameTest()
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
}