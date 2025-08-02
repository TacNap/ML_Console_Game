public class GameState
{
    // Fields
    private bool GameActive { get; set; } // true if game is currently in progress
    private bool Computer { get; set; } // true if game will be between player and AI

    private int GRID_WIDTH { get; }
    private int GRID_HEIGHT { get; }

    // Constructor
    public GameState()
    {
        this.GRID_WIDTH = 7;
        this.GRID_HEIGHT = 6;
    }

    // Methods
    public void GameLoop()
    {
        // Print the Grid
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                Console.Write("| ");
            }
            Console.WriteLine();
        }
    }

    public void GameStart()
    {
        while (!GameActive)
        {
            Console.WriteLine("Welcome to LineUp");
            Console.WriteLine("1 > new game\n2 > load game\n3 > help");
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input.ToLower() == "new") // Start new game
            {
                // Determine number of players
                Console.WriteLine("Against AI? {Y/N}\n> ");
                input = Console.ReadLine();
                Computer = input.ToLower() == "y" ? true : false;
                GameActive = true;
                GameLoop();
            }
            else if (input.ToLower() == "load")
            {
                Console.WriteLine("To be implemented...\n");
            }
            else if (input.ToLower() == "help")
            {
                Console.WriteLine("Help information would go here...\n");
            }
            else
            {
                Console.WriteLine("Unrecognised command. Try again...\n");
            }

        }

    }
}