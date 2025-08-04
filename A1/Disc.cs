public class Disc
{
    // Fields
    public char Symbol { get; set; }
    public int Type { get; set; } // {1: O, 2: B, 3: Mag, 4: E}

    public bool IsPlayerOne { get; set; }
    private char[] DiscsP1 = {
        '@',
        'B',
        'M',
        'E'
    };
    private char[] DiscsP2 = {
        '#',
        'b',
        'm',
        'e'
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

 