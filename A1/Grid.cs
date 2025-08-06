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
    public void AddDisc(int col, int type = 1, bool isPlayerOne = true)
    {
        col -= 1; // Adjust for indexing
        if (Board[0, col] != null)
        {
            Console.WriteLine("Column is full!");
            return;
        }
        // This loops through each row in the column to find a disc
        // Then creates a new disc above it

        for (int i = 0; i < GRID_HEIGHT; i++)
        {
            if (Board[i, col] == null)
            {
                continue;
            }
            else
            {
                Board[i - 1, col] = new Disc(type, isPlayerOne);
                return;
            }
        }

        // If column is empty, add disc to the bottom
        Board[GRID_HEIGHT - 1, col] = new Disc(type, isPlayerOne);
    }
    public void DrawGrid()
    {
        // Print Column Numbers
        for (int col = 1; col <= GRID_WIDTH; col++)
        {
            Console.Write($"  {col} ");
        }
        Console.WriteLine();

        // Print grid barriers and discs
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                // If there's no disc here, print whitespace
                string render = Board[y, x] == null ? " " : Board[y, x].Symbol; 
                Console.Write($"| {render} ");
            }
            Console.WriteLine("|");
        }
    }

    public void ClearGrid()
    {
        Board = new Disc[GRID_HEIGHT, GRID_WIDTH];
    }
}