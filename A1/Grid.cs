using System.Collections.Generic;
using System;

public class Grid
{
    // Fields
    public int GRID_WIDTH { get; private set; }
    public int GRID_HEIGHT { get; private set; }
    
    public Disc[,] Board { get; private set; }

    public int WinLength { get; private set; }

    public int TurnCounter { get; private set; }

    private IOHandler IOHandler;



    // Constructor
    public Grid()
    {
        this.GRID_HEIGHT = 6;
        this.GRID_WIDTH = 7;
        Board = new Disc[GRID_HEIGHT, GRID_WIDTH];
        this.WinLength = (int)Math.Floor(GRID_HEIGHT * GRID_WIDTH * 0.1);
        this.TurnCounter = 1;
        this.IOHandler = new IOHandler();
    }

    // Methods

    // Add Disc to a chosen column.
    // Successful if top row of chosen column is empty
    public void SetGridSize(int height, int width)
    {
        GRID_HEIGHT = height;
        GRID_WIDTH = width;
        WinLength = (int)Math.Floor(GRID_HEIGHT * GRID_WIDTH * 0.1);
    }

    public void SetTurnCounter(int num)
    {
        if (num < 0)
        {
            // throw exception
            return;
        }
        TurnCounter = num;
    }

    public void IncrementTurnCounter()
    {
        TurnCounter++;
    }

    public bool AddDisc(int col, Disc disc)
    {
        if (Board[0, col] != null) // if collumn is full
        {
            return false;
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
                Board[i - 1, col] = disc; // place a disc above the lowest disc
                return true;
            }
        }

        // If column is empty, add disc to the bottom
        Board[GRID_HEIGHT - 1, col] = disc;
        return true;
    }

    public bool AIFindWinningMove(Dictionary<string,int> P2Discs)
    {   
        // temporary bool 
        bool IsPlayerOneWin = false;

        Disc disc = new OrdinaryDisc(false);
        int winningColumn = -1;
        bool IsWinFound = false;
        // Establish a 'checkpoint' of the current grid.
        // We'll revert back to this during the function call. 
        Disc[,] Checkpoint = (Disc[,])Board.Clone();

        // For each disc type
        foreach (var discEntry in P2Discs)
        {
            // Skip checking if AI doesn't have any of these discs remaining
            if (discEntry.Value < 1)
            {
                continue;
            }
            // Create a disc to test with
            if (discEntry.Key == "Ordinary")
            {
                disc = new OrdinaryDisc(false);
            }
            else if (discEntry.Key == "Boring")
            {
                disc = new BoringDisc(false);
            }
            else
            {
                disc = new ExplosiveDisc(false);
            }

            // For each column
            for (int col = 0; col < GRID_WIDTH; col++)
            {
                // Add the disc 
                if (AddDisc(col, disc))
                {
                    // Apply its effects
                    if (disc is BoringDisc b)
                    {
                        ApplyEffects(col, b);
                    }
                    else if (disc is ExplosiveDisc e)
                    {
                        ApplyEffects(col, e);
                    }
                    // If this produces a winning move, set flag and break
                    // At the moment, this would be true even if P1 wins. 
                    if (CheckWinCondition(ref IsPlayerOneWin)) // I'll need to make a separate win check that doesn't print.
                    {
                        IsWinFound = true;
                        winningColumn = col;
                        Board = (Disc[,])Checkpoint.Clone();
                        break;
                    }
                }

                // Revert to checkpoint after applying effects.
                Board = (Disc[,])Checkpoint.Clone();
            }
            if (IsWinFound) // Must break an additional time, in order to exit both loops
            {
                break;
            }
        }

        if (IsWinFound)
        {
            Board = (Disc[,])Checkpoint.Clone();
            AddDisc(winningColumn, disc);
            if (disc is BoringDisc b)
            {
                ApplyEffects(winningColumn, b);
            }
            else if (disc is ExplosiveDisc e)
            {
                ApplyEffects(winningColumn, e);
            }
            return true;
        }
        return false;

    }

    // Applies gravity to a column.
    // Collects all discs in the column and places into a List
    // Then re-places the discs from bottom up.
    // Intended for use with Explosive Discs.
    public void ApplyGravity(int col)
    {
        // Make a list of all discs in the column
        List<Disc> discs = new List<Disc>();

        for (int row = GRID_HEIGHT - 1; row >= 0; row--)
        {
            if (Board[row, col] != null)
            {
                discs.Add(Board[row, col]);
                Board[row, col] = null;
            }
        }

        // Re-place discs, bottom up
        for (int i = 0; i < discs.Count; i++)
        {
            Board[GRID_HEIGHT - 1 - i, col] = discs[i];
        }
    }


    // Explosive Disc Behaviour Logic
    public void ApplyEffects(int col, ExplosiveDisc disc)
    {
        // Find the row number of the explosive disc
        int depth = -1;
        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            if (Board[row, col] != null)
            {
                depth = row;
                break;
            }
        }

        int killCount = 0;
        // Iterate a 3x3 radius and remove discs from the board
        for (int rrow = -1; rrow < 2; rrow++)
        {
            if (depth + rrow < 0 || depth + rrow >= GRID_HEIGHT)
            {
                continue; // Out of bounds
            }
            for (int rcol = -1; rcol < 2; rcol++)
            {
                if (col + rcol < 0 || col + rcol >= GRID_WIDTH)
                {
                    continue; // Out of bounds
                }

                if (Board[depth + rrow, col + rcol] != null)
                {
                    Board[depth + rrow, col + rcol] = null;
                    killCount++;
                }

            }
        }

        for (int rcol = -1; rcol < 2; rcol++)
        {
            if (col + rcol >= 0 && col + rcol < GRID_WIDTH)
            {
                ApplyGravity(col + rcol);
            }
        }

        IOHandler.PrintHeading($"Explosive disc destroyed {killCount-1} disc/s!\n");
        
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
    public void DrawGrid(bool banner = true)
    {
        if(banner)IOHandler.PrintBanner();
        // Print Column Numbers
        Console.Write(" ");
        for (int col = 1; col <= GRID_WIDTH; col++)
        {
            Console.Write($"{col, 4}");
        }
        Console.WriteLine();

        // Print grid barriers and disc amounts
        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            Console.Write($"{GRID_HEIGHT - row, 2}");
            for (int col = 0; col < GRID_WIDTH; col++)
            {
                // If there's no disc here, print whitespace
                string symbol = Board[row, col] == null ? " " : Board[row, col].Symbol;
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
        // if disc is special
        // apply effects
        // and draw grid again
        if (disc is BoringDisc b)
        {
            ApplyEffects(col, b);
            DrawGrid(false);
        }
        else if (disc is ExplosiveDisc e)
        {
            ApplyEffects(col, e);
            DrawGrid(false);
        }

    }

    // Determines if either player has won in the current grid state
    // make me a bool later.
    // print using IOHandler if there's a win
    // Needs to account for ties, too 
    public bool CheckWinCondition(ref bool IsPlayerOneWin)
    {
        int P1HorizontalCounter;
        int P2HorizontalCounter;

        // Check Horizontal
        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            P1HorizontalCounter = 0;
            P2HorizontalCounter = 0;
            for (int col = 0; col < GRID_WIDTH; col++)
            {
                if (Board[row, col]?.IsPlayerOne == true) // If this disc belongs to Player One
                {
                    P1HorizontalCounter++;
                    P2HorizontalCounter = 0;
                }
                else if (Board[row, col]?.IsPlayerOne == false)
                {
                    P1HorizontalCounter = 0;
                    P2HorizontalCounter++;
                }
                else // If this space is empty, reset both counters
                {
                    P1HorizontalCounter = 0;
                    P2HorizontalCounter = 0;
                }
                if (P1HorizontalCounter == WinLength)
                {
                    IsPlayerOneWin = true;
                    return true;
                }
                if (P2HorizontalCounter == WinLength)
                {
                    IsPlayerOneWin = false;
                    return true;
                }
            }
        }

        int P1VerticalCounter;
        int P2VerticalCounter;


        // Check Vertical
        for (int col = 0; col < GRID_WIDTH; col++)
        {
            P1VerticalCounter = 0;
            P2VerticalCounter = 0;
            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                if (Board[row, col]?.IsPlayerOne == true)
                {
                    P1VerticalCounter++;
                    P2VerticalCounter = 0;
                }
                else if (Board[row, col]?.IsPlayerOne == false)
                {
                    P1VerticalCounter = 0;
                    P2VerticalCounter++;
                }
                else
                {
                    P1VerticalCounter = 0;
                    P2VerticalCounter = 0;
                }
                if (P1VerticalCounter == WinLength)
                {
                    IsPlayerOneWin = true;
                    return true;
                }
                if (P2VerticalCounter == WinLength)
                {
                    IsPlayerOneWin = false;
                    return true;
                }
            }
        }
        
        int P1DiagonalCounter;
        int P2DiagonalCounter;

        // Check Diagonal - North-Eastern, left half
        for (int row = WinLength - 1; row < GRID_HEIGHT; row++)
        {
            P1DiagonalCounter = 0;
            P2DiagonalCounter = 0;
            for (int delta = 0; row - delta >= 0 && delta < GRID_WIDTH; delta++)
            {
                if (Board[row - delta, delta]?.IsPlayerOne == true)
                {
                    P1DiagonalCounter++;
                    P2DiagonalCounter = 0;
                }
                else if (Board[row - delta, delta]?.IsPlayerOne == false)
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter++;
                }
                else
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter = 0;
                }
                if (P1DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = true;
                    return true;
                }
                if (P2DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = false;
                    return true;
                }
            }
        }

        // Check Diagonal - North-Eastern, right half
        for (int col = 1; col < GRID_WIDTH; col++)
        {
            P1DiagonalCounter = 0;
            P2DiagonalCounter = 0;
            for (int delta = 0; GRID_HEIGHT - 1 - delta >= 0 && col + delta < GRID_WIDTH; delta++) 
            {
                if (Board[GRID_HEIGHT - 1 - delta, col + delta]?.IsPlayerOne == true)
                {
                    P1DiagonalCounter++;
                    P2DiagonalCounter = 0;
                }
                else if (Board[GRID_HEIGHT - 1 - delta, col + delta]?.IsPlayerOne == false)
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter++;
                }
                else
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter = 0;
                }
                if (P1DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = true;
                    return true;
                }
                if (P2DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = false;
                    return true;
                }
            }
        }

        // Check Diagonal - North-Western, right half 
        for (int row = WinLength - 1; row < GRID_HEIGHT; row++)
        {
            P1DiagonalCounter = 0;
            P2DiagonalCounter = 0;
            for (int delta = 0; row - delta >= 0 && GRID_WIDTH - 1 - delta >= 0; delta++) 
            {
                if (Board[row - delta, GRID_WIDTH - 1 - delta]?.IsPlayerOne == true)
                {
                    P1DiagonalCounter++;
                    P2DiagonalCounter = 0;
                }
                else if (Board[row - delta, GRID_WIDTH - 1 - delta]?.IsPlayerOne == false)
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter++;
                }
                else
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter = 0;
                }
                if (P1DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = true;
                    return true;
                }
                if (P2DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = false;
                    return true;
                }
            }
        }

        // Check Diagonal - North-Western, left half
        for (int col = GRID_WIDTH - 2; col >= 0; col--)
        {
            P1DiagonalCounter = 0;
            P2DiagonalCounter = 0;
            for (int delta = 0; GRID_HEIGHT - 1 - delta >= 0 && col - delta >= 0; delta++) 
            {
                if (Board[GRID_HEIGHT - 1 - delta, col - delta]?.IsPlayerOne == true)
                {
                    P1DiagonalCounter++;
                    P2DiagonalCounter = 0;
                }
                else if (Board[GRID_HEIGHT - 1 - delta, col - delta]?.IsPlayerOne == false)
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter++;
                }
                else
                {
                    P1DiagonalCounter = 0;
                    P2DiagonalCounter = 0;
                }
                if (P1DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = true;
                    return true;
                }
                if (P2DiagonalCounter == WinLength)
                {
                    IsPlayerOneWin = false;
                    return true;
                }
            }
        }
        return false;
    }

    public void ClearGrid()
    {
        Board = new Disc[GRID_HEIGHT, GRID_WIDTH];
    }
}