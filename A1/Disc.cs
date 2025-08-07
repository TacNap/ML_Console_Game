public abstract class Disc
{
    public abstract string Symbol { get; set; }

    public bool IsPlayerOne { get; set; }

    public abstract string P1 { get; }

    public abstract string P2 { get; }
}