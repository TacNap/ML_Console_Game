using System;

class Program
{
    static void GuessingGameWithExceptionHandling()
    {
        Random random = new Random();
        int target = random.Next(1, 11);
        int prevLow = -1;
        int prevHigh = -1;
        int guess;

        while (true)
        {
            Console.WriteLine("Guess a number between 1 and 10 (inclusive):");
            Console.Write("> ");
            string input = Console.ReadLine();
            try
            {
                guess = int.Parse(input);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Must be a number");
                continue;
            }

            if (guess < target)
            {
                if (prevLow != -1 && guess <= prevLow)
                {
                    Console.WriteLine("u stupid");
                    continue;
                }
                Console.WriteLine("Higher!");
                prevLow = guess;
                continue;
            }
            else if (guess > target)
            {
                if (prevHigh != -1 && guess >= prevHigh)
                {
                    Console.WriteLine("u stupid");
                    continue;
                }
                Console.WriteLine("Lower!");
                prevHigh = guess;
                continue;
            }
            Console.WriteLine("Correct!");
            break;

        }

    }

    static void GuessAWordWithExceptionHandling()
    {
        string target = "magic";
        Console.WriteLine("Guess the word, or letters in the word");
        string input;

        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            if (input == target)
            {
                Console.WriteLine("Correct!");
                break;
            }
            foreach (char s in input)
            {
                if (!Char.IsLetter(s))
                {
                    throw new NonLetterException("Must only include alphabetical characters.");
                }
                if (target.Contains(s))
                {
                    Console.WriteLine($"{s} is in the word!");
                }
                else
                {
                    Console.WriteLine($"{s} is not.");
                }
            }
            
        }
    }

    static void TwoDimensionalArrayExample()
    {
        Random random = new Random(); // to assign variables
        Console.WriteLine("Two Dimensional Array Examples:");

        // Declare the matrix
        int width = 5;
        int height = 4;
        int[,] matrix = new int[height, width];

        // Initialise the matrix values
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                matrix[row, col] = random.Next(100, 1000);
            }
        }


        // Print to a table format
        Console.WriteLine("     |  0  |  1  |  2  |  3  |  4  |");
        for (int row = 0; row < height; row++)
        {
            Console.Write($"  {row}  ");
            for (int col = 0; col < width; col++)
            {
                Console.Write($"| {matrix[row, col]} ");
            }
            Console.Write("|\n");
        }



    }
    static void Main(string[] args)
    {
        //GuessingGameWithExceptionHandling();
        GuessAWordWithExceptionHandling();
        //TwoDimensionalArrayExample();



    }
}