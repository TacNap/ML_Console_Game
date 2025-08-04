public class Grid
{
    // Fields
    public int GRID_WIDTH { get; private set; }
    public int GRID_HEIGHT { get; private set; }

    public Disc[,] Board { get; private set; }



    // Constructor
    public Grid()
    {
        this.GRID_WIDTH = 7;
        this.GRID_HEIGHT = 6;
        Board = new Disc[GRID_HEIGHT, GRID_WIDTH];
    }

    // Methods

    // Add Disc to a chosen column.
    // Successful if top row of chosen column is empty
    public void AddDisc(int col, bool isPlayerOne)
    {
        // This loops through each row in the column to find a disc
        // Then creates a new disc above it
        for (int i = 0; i < GRID_HEIGHT; i++)
        {
            if (Board[i, col-1] == null)
            {
                continue;
            }
            else
            {
                Board[i - 1, col-1] = new Disc(1, isPlayerOne);
                return;
            }
        }
        Board[GRID_HEIGHT - 1, col - 1] = new Disc(1, isPlayerOne);
    }
    public void DrawGrid()
    {
        // Print the Grid
        Console.WriteLine(" 1 2 3 4 5 6");
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                Console.Write("|");
                if (Board[y, x] == null)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(Board[y, x].Symbol);
                }
            }
            Console.WriteLine("");
        }
    }
}