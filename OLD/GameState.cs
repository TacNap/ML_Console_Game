public class GameState
{
    // Fields
    private bool State { get; set; } // true if game is currently in progress

    // Constructor
    public GameState()
    {
        this.State = false;
    }

    // Methods
    public void GameStart()
    {
        while (!State)
        {
            Console.WriteLine("Welcome to LineUp");
            Console.WriteLine("1 > new game\n2 > load game\n3 > help");
            Console.Write("> ");
            string input = Console.ReadLine();
            // input to lower
            if (input.ToLower() == "new")
            {
                State = true;
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

        while (State)
        {
            Console.WriteLine("Game Start!");
            Console.ReadLine();
            break;
        }
    }
}