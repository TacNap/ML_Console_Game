
// make me static bruh 
public class FileController
{
    public FileController()
    {

    }

    public void GridSerialization(string path, Grid grid, Dictionary<string,int> P1Discs, Dictionary<string, int> P2Discs, bool IsPlayerTurn)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            // grid metadata
            writer.WriteLine($"{grid.GRID_HEIGHT}");
            writer.WriteLine($"{grid.GRID_WIDTH}");
            writer.WriteLine($"{grid.TurnCounter}");
            // player disc amounts
            writer.WriteLine($"{P1Discs["Ordinary"].ToString()}");
            writer.WriteLine($"{P1Discs["Boring"].ToString()}");
            writer.WriteLine($"{P1Discs["Explosive"].ToString()}");

            writer.WriteLine($"{P2Discs["Ordinary"].ToString()}");
            writer.WriteLine($"{P2Discs["Boring"].ToString()}");
            writer.WriteLine($"{P2Discs["Explosive"].ToString()}");

            // player turn
            writer.WriteLine($"{IsPlayerTurn}");


            // Iterate through Board
            for (int row = 0; row < grid.GRID_HEIGHT; row++)
            {
                for (int col = 0; col < grid.GRID_WIDTH; col++)
                {
                    // If cell is empty, write null
                    if (grid.Board[row, col] == null)
                    {
                        writer.Write("null");
                    }
                    else
                    {
                        if (grid.Board[row, col] is OrdinaryDisc o) // If ordinary disc
                        {
                            writer.Write("o");
                        }
                        else // Otherwise, assume it's a boring disc
                        {
                            writer.Write("b");
                        }
                        // Determine which player it belongs to
                        writer.Write(grid.Board[row, col].IsPlayerOne ? "1" : "0");
                    }
                    writer.WriteLine();
                }
            }
        }
    }

    public Grid GridDeserialization(string path, Dictionary<string,int> P1Discs, Dictionary<string, int> P2Discs, ref bool IsPlayerTurn)
    {
        Grid returnGrid = new Grid();
        using (StreamReader reader = new StreamReader(path))
        {
            // Get Metadata
            int rows, cols, turn;
            try
            {
                rows = Int32.Parse(reader.ReadLine());
                cols = Int32.Parse(reader.ReadLine());
                turn = Int32.Parse(reader.ReadLine());
                returnGrid.SetGridSize(rows, cols);
                returnGrid.SetTurnCounter(turn);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Grid metadata couldn't be read");
                rows = -1;
                cols = -1;
            }

            // Get player disc amounts 
            try
            {
                P1Discs["Ordinary"] = Int32.Parse(reader.ReadLine());
                P1Discs["Boring"] = Int32.Parse(reader.ReadLine());
                P1Discs["Explosive"] = Int32.Parse(reader.ReadLine());

                P2Discs["Ordinary"] = Int32.Parse(reader.ReadLine());
                P2Discs["Boring"] = Int32.Parse(reader.ReadLine());
                P2Discs["Explosive"] = Int32.Parse(reader.ReadLine());

                IsPlayerTurn = reader.ReadLine() == "True" ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Player metadata couldn't be read");
            }


            string line;
            // Get Discs
            for (int row = 0; row < returnGrid.GRID_HEIGHT; row++)
            {
                for (int col = 0; col < returnGrid.GRID_WIDTH; col++)
                {
                    line = reader.ReadLine();
                    // If line is "null"
                    if (line == "null")
                    {
                        continue;
                    }
                    else
                    {
                        // Determine which player it belongs to
                        bool player = line[1] == '1' ? true : false;
                        if (line[0] == 'o')
                        {
                            returnGrid.Board[row, col] = new OrdinaryDisc(player);
                        }
                        else if (line[0] == 'b')
                        {
                            returnGrid.Board[row, col] = new BoringDisc(player);
                        }
                        else
                        {
                            // unrecognised symbol 
                            // throw exception
                        }


                    }

                }
            }



        }

        return returnGrid;
    }

}