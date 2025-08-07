public class BoringDisc : Disc
{
    public override string Symbol { get; set; }

    public override string P1 { get; } = "B";

    public override string P2 { get; } = "b";
    public BoringDisc(bool isPlayerOne = true)
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