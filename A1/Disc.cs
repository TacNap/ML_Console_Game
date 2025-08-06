public class Disc
{
    // Fields
    public string Symbol { get; set; }
    public int Type { get; set; }

    public bool IsPlayerOne { get; set; }

    // These need to be strings, otherwise it gets messy with conversions mid-method
    private string[] DiscsP1 = {
        "@",
        "B",
        "E"
    };
    private string[] DiscsP2 = {
        "#",
        "b",
        "e"
    };

    // Constructor
    public Disc(int type = 1, bool isPlayerOne = true)
    {
        this.Type = type;
        this.IsPlayerOne = isPlayerOne;
        if (isPlayerOne)
        {
            this.Symbol = DiscsP1[type-1];
        }
        else
        {
            this.Symbol = DiscsP2[type-1];
        }
        
    }

}

 