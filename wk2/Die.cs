public class Die
{
    // int value {1-6}
    public int Value { get; set; }

    // auto-implemented property for value of die

    // Constructor that requires the value of the die
    public Die(int value)
    {
        Value = value;
    }

}