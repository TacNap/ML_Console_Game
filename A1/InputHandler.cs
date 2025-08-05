public class InputHandler
{
    // Fields
    public string[] Commands { get; }
    // Constructor

    public InputHandler()
    {
        this.Commands = [
            "/new",
            "/load",
            "/help",
            "/quit"
        ];

    }
    // Methods
    public void ParseInput()
    {

    }

    public void PrintHeading(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(text);
        Console.ResetColor();
    }
    public void PrintError(string text)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(text);
        Console.ResetColor();
    }
    
    
    public string ListCommands(int opt)
    {
        if (opt == 1)
        {
            Console.Write("> ");
            PrintHeading("new");
            Console.Write(" game\n");
            Console.Write("> ");
            PrintHeading("load");
            Console.Write(" game\n");
            Console.Write("> ");
            PrintHeading("help\n");


            Console.Write("> ");
        }
        else if (opt == 2)
        {
            Console.Write("Against AI? {");
            PrintHeading("Y");
            Console.Write("/");
            PrintHeading("N");
            Console.Write("}\n");
            Console.Write("> ");
        }

        string input = Console.ReadLine();
        return input.ToLower();
    }
}