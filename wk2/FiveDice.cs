public class FiveDice
{
    private Die[] dicePlayer = new Die[5];

    private Die[] diceComputer = new Die[5];

    // Constructor
    public FiveDice()
    {
        for (int i = 0; i < 5; i++)
        {
            dicePlayer[i] = new Die(1);
            diceComputer[i] = new Die(1);
        }

    }

    // Roll Method
    public void GameRound()
    {
        Console.WriteLine("New Round Start");

        // Roll all Dice
        for (int i = 0; i < 5; i++)
        {
            dicePlayer[i].Roll();
            diceComputer[i].Roll();
        }

        // Determine Scores
        string playerScore = DetermineHand(dicePlayer);
        string computerScore = DetermineHand(diceComputer);

        Console.WriteLine($"The Player got {playerScore}. The Computer got {computerScore}");

    }

    // Determine Hand Method
    private static string DetermineHand(Die[] bag)
    {
        // Array to hold the occurences of each number
        int[] counts = new int[7];
        foreach (Die die in bag)
        {
            counts[die.Value]++;
        }

        // Sort the array
        Array.Sort(counts);

        if (counts[6] > 2)
        {
            return $"{counts[6]} of a kind!";
        }
        else if (counts[6] == 2)
        {
            return "a pair!";
        }
        else
        {
            return "no score";
        }
    }

    // Print Bag - for testing
    private void PrintBags()
    {
        Console.WriteLine("Player Bag Values:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(dicePlayer[i].Value);
        }
        
        Console.WriteLine("Computer Bag Values:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(diceComputer[i].Value);
        }
        
    }



}