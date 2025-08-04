public class Disc
{
    // Fields
    public char Symbol { get; set; }
    public string Type { get; set; }

    public bool IsPlayerOne { get; set; }

    // Constructor
    public Disc(char symbol = '@', string type = "Ordinary", bool isPlayerOne = true)
    {
        this.Symbol = symbol;
        this.Type = type;
        this.IsPlayerOne = isPlayerOne;
    }

}

 // private char[] DiscsP1 = {
    //     ' ',
    //     '@',
    //     'B',
    //     'M',
    //     'E'
    // };
    // private char[] DiscsP2 = {
    //     ' ',
    //     '#',
    //     'b',
    //     'm',
    //     'e'
    // };