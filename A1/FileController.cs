using System.Text.Json;
public class FileController
{
    public FileController()
    {

    }

    public void SaveGrid(string path, Grid grid)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            // grid metadata
            writer.WriteLine($"{grid.GRID_WIDTH}");
            writer.WriteLine($"{grid.GRID_HEIGHT}");
            writer.WriteLine($"{grid.WinLength}");
            


            for (int row = 0; row < grid.GRID_HEIGHT; row++)
            {
                for (int col = 0; col < grid.GRID_WIDTH; col++)
                {
                    if (grid.Board[row, col] == null)
                    {
                        writer.Write("null");
                    }
                    else
                    {
                        writer.Write("o"); // needs to account for boring disc, too
                        writer.Write(grid.Board[row, col].IsPlayerOne ? "1" : "0");
                    }
                    if (col < grid.GRID_WIDTH)
                    {
                        writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }
    }

    // public Grid GridDeserialization(string path)
    // {
    // }

}