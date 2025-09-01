using System.Text.Json;
public class FileController
{
    public FileController()
    {

    }

    // JSON Serializer doesn't support 2D arrays,
    // So i'll have to convert it first
    public Disc[][] ConvertToJaggedArray(Disc[,] gridData)
    {
        return null;
    }

    public Disc[,] ConvertTo2DArray(Disc[][] gridData)
    {
        return null;
    }

    public void GridSerialization(Grid data)
    {
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