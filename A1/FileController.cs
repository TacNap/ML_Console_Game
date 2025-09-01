using System.Text.Json;
public class FileController
{
    public FileController()
    {

    }

    public void DiscSerialization(Disc data)
    {
        string jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText("Objects/disc.json", jsonString);
        Console.WriteLine(jsonString);
    }

    public void GridSerialization(Grid data)
    {
        string jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText("Objects/grid.json", jsonString);
        Console.WriteLine(jsonString);
    }

}