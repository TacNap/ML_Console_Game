using System;

class Program
{
    // DisplayTwoCards
    static void DisplayTwoCards() {
        Console.WriteLine("\nNew Game!");
        // Generate random values
        Random rnd = new Random();
        int val = rnd.Next(1,14);
        int suit = rnd.Next(1,5);

        Card card1 = new Card(val, suit);

        // Generate new, unique random values
        while(val == card1.GetValue()) {
            val = rnd.Next(1,14);
        }
        while(suit == card1.GetSuit()) {
            suit = rnd.Next(1,5);
        }

        Card card2 = new Card(val, suit);

        if (card1.GetValue() > card2.GetValue()) {
            Console.WriteLine($"{card1.GetString()} wins over {card2.GetString()}");
        } else {
            Console.WriteLine($"{card2.GetString()} wins over {card1.GetString()}");
        }

    }
    
    // WarCardGame
    static void WarCardGame() {
        // Generate a new deck
        Card[] deck = new Card[52];
        bool[] grave = new bool[52]; // Defaults to false
        for(int i = 0; i < 13; i++) {
            deck[i] = new Card(i+1, 1);
            deck[i+13] = new Card(i+1, 2);
            deck[i+26] = new Card(i+1, 3);
            deck[i+39] = new Card(i+1, 4);
        }

        
        
    }

    static void Main(string[] args)
    {
        // ###### Exercise A
        Point point1 = new Point(3, 4);
        Point point2 = new Point(6, 8);
        Console.WriteLine($"Point 1: ({point1.GetX()}, {point1.GetY()})");
        Console.WriteLine($"Point 2: ({point2.GetX()}, {point2.GetY()})");
        Console.WriteLine($"Distance between Point 1 and Point 2: {point1.DistanceTo(point2)}");

        point1.Set(10, 15);
        Console.WriteLine($"Point 1 after setting: ({point1.GetX()}, {point1.GetY()})");

        // ###### Exercise B / C
        // Create a new bank account
        BankAccount account = new BankAccount("123456", "John Doe");

        // Display initial account details
        Console.WriteLine($"Account Number: {account.AccountNumber}, Account Holder: {account.AccountHolderName}, Balance: {account.Balance:C}");

        // Deposit money
        account.Deposit(100);
        Console.WriteLine($"After deposit, Balance: {account.Balance:C}");

        // Withdraw money
        Console.WriteLine(account.Withdraw(50) ? "Withdrawal successful." : "Withdrawal failed.");
        Console.WriteLine($"After withdrawal, Balance: {account.Balance:C}");

        // ToString()
        Console.WriteLine(account.ToString());

        // Transfer
        BankAccount recipient = new BankAccount("654321", "Jason Momoa");
        Console.WriteLine(account.Transfer(recipient, 10) ? "Transfer successful." : "Transfer failed.");
        Console.WriteLine($"After transfer, Balances: John Doe: {account.Balance:C}. Jason Momoa: {recipient.Balance:C}");

        // ###### Exercise D
        Console.WriteLine("#### Die ####\n\n");
        Die d6 = new Die(1);
        Console.WriteLine($"Value before rolling: {d6.Value}");
        d6.Roll();
        Console.WriteLine($"Value after rolling: {d6.Value}");

        Console.WriteLine("#### FiveDice ####\n\n");

        FiveDice gameMaster = new FiveDice();
        gameMaster.GameRound();

        // ###### Exercise E
        DisplayTwoCards();

        WarCardGame();



    }
}