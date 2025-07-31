using System;

class Program
{
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

        // ###### Exercise B
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
    }
}