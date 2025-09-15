public class ExplosiveDisc : Disc
{
    public override string Symbol { get; set; }

    public override string P1 { get; } = "E";

    public override string P2 { get; } = "e";

    public ExplosiveDisc(bool isPlayerOne = true)
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