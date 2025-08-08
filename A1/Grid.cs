public class Grid
{
    // Fields
    public int GRID_WIDTH { get; private set; }
    public int GRID_HEIGHT { get; private set; }

    public Disc[,] Board { get; private set; }

    private IOHandler IOHandler;



    // Constructor
    public Grid()
    {
        this.GRID_WIDTH = 7;
        this.GRID_HEIGHT = 6;
        Board = new Disc[GRID_HEIGHT, GRID_WIDTH];
        this.IOHandler = new IOHandler();
    }

    // Methods

    // Add Disc to a chosen column.
    // Successful if top row of chosen column is empty
    public void AddDisc(int col, Disc disc)
    {
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
                Board[i - 1, col] = disc;
                return;
            }
        }

        // If column is empty, add disc to the bottom
        Board[GRID_HEIGHT - 1, col] = disc;
    }



    // Explosive Disc Behaviour Logic
    public void ApplyEffects(int col, ExplosiveDisc disc)
    {
        for (int row = 0; row < GRID_HEIGHT - 1; row++)
        {
            // loop a 3x3 grid from this point with double pointers
            // "try" for boundaries
            // otherwise set it to null            
        }

        // then, you'll need to check if there's any discs above the 3x3 radius that were affected
        // and pull them down accordingly.
        // might be easier to keep tabs on them in an array
        // then set to null and just add them in order?

        // THIS WOULD BE WAY EASIER TO IMPLEMENT IF EVERY COLUMN WAS A STACK, NOT JUST AN ARRAY. 
    }

    // Boring Disc Behaviour Logic
    public void ApplyEffects(int col, BoringDisc disc)
    {
        int killCount = 0;

        for (int row = 0; row < GRID_HEIGHT - 1; row++)
        {
            if (Board[row, col] != null)
            {
                killCount++;
                Board[row, col] = null;
            }

        }
        Board[GRID_HEIGHT - 1, col] = new BoringDisc(disc.IsPlayerOne);
        IOHandler.PrintHeading($"Boring disc destroyed {killCount} disc/s!\n");
    }

    // Simply draws the grid in its current state
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
                string symbol = Board[y, x] == null ? " " : Board[y, x].Symbol;
                Console.Write($"| {symbol} ");
            }
            Console.WriteLine("|");
        }
    }

    // Applies disc effects and draws grid respectively
    // Takes the last-placed disc as reference
    public void RenderGrid(int col, Disc disc)
    {
        DrawGrid();
        if (disc is ExplosiveDisc e)
        {
            ApplyEffects(col, e);
            DrawGrid();
        }
        else if (disc is BoringDisc b)
        {
            ApplyEffects(col, b);
            DrawGrid();
        }
        // if disc is special
        // apply effects
        // and draw grid
    }

    public void ClearGrid()
    {
        Board = new Disc[GRID_HEIGHT, GRID_WIDTH];
    }
}