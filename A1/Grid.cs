public class Grid
{
    // Fields
    public int GRID_WIDTH { get; private set; }
    public int GRID_HEIGHT { get; private set; }

    public Disc[,] Board = new Disc[6, 7];

    

    // Constructor
    public Grid()
    {
        this.GRID_WIDTH = 7;
        this.GRID_HEIGHT = 6;
    }

    // Methods
    public void DrawGrid()
    {
        Board[0, 1] = new Disc();
        Board[2, 1] = new Disc();
        Board[3, 1] = new Disc();
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