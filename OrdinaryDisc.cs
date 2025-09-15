public class OrdinaryDisc : Disc
{
    public override string Symbol { get; set; }

    public override string P1 { get; } = "@";

    public override string P2 { get; } = "#";

    public OrdinaryDisc(bool isPlayerOne = true)
    {
        this.IsPlayerOne = isPlayerOne;
        if (isPlayerOne)
        {
            this.Symbol = P1;
        }
        else
        {
            this.Symbol = P2;
        }
    }
}