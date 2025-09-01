using System.Text.Json;
public class FileController
{
    public FileController()
    {

    }

    public void GridSerialization(Grid data)
    {
        data.ConvertToJaggedArray();
        string jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText("Objects/grid.json", jsonString);
        Console.WriteLine(jsonString);
    }

    public Grid GridDeserialization(string path)
    {
        string jsonStringRead = File.ReadAllText(path);
        Grid newGrid = JsonSerializer.Deserialize<Grid>(jsonStringRead);
        return newGrid;
    }

}