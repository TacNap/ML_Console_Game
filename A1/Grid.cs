public class Grid
{
    // Fields
    public int GRID_WIDTH { get; private set; }
    public int GRID_HEIGHT { get; private set; }

    // probably an array here

    // Constructor
    public Grid()
    {
        this.GRID_WIDTH = 7;
        this.GRID_HEIGHT = 6;
    }

    // Methods
    public void DrawGrid()
    {
        // Print the Grid
        Console.WriteLine(" 1 2 3 4 5 6");
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                Console.Write("| ");
            }
            Console.WriteLine();
        }
    }
}