public abstract class Disc
{
    public abstract string Symbol { get; set; } // Symbol to render to console

    public bool IsPlayerOne { get; set; } // True if belongs to Player 1

    public abstract string P1 { get; } // character specific for P1

    public abstract string P2 { get; } // character specific for P2
}