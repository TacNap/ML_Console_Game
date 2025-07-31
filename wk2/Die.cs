public class Die
{
    // Auto implemented Property
    public int Value { get; private set; }

    // Required for dice rolls
    public static Random random = new Random();
    // Constructor
    public Die(int value)
    {
        // If the value supplied is not {1,6}, default to 6
        if (value >= 0 && value <= 6)
        {
            Value = value;
        }
        else
        {
            Value = 6;
        }
    }

    public void Roll()
    {
        this.Value = random.Next(1, 7);
    }
}

///
    // Dice are used in many games. One die can be thrown to randomly show a value from 1 through 6. 
    // Design a Die class that can hold an integer data field for a value (from 1 to 6). 
    // Include an auto-implemented property that holds the value of the die and a constructor that requires a value for the die.
    // Write an application named TwoDice that generates random numbers for the value of two dice and displays their values.
    // Using the Die class, write an application named FiveDice that randomly “throws” five dice for the computer and five dice for a second player. 
    // Display the values and then decide who wins based on the following hierarchy of Die values.
    //     Five of a kind
    //     Four of a kind
    //     Three of a kind
    //     A pair
///