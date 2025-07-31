public class BankAccount
{
    private string accountNumber;
    public string AccountNumber
    { get { return accountNumber; } private set { accountNumber = value; } }

    // Not sure if this is implemented correctly. 
    // Are the set methods meant to be mentioned here?
    private decimal balance;
    public decimal Balance { get { return balance; } }

    private string accountHolderName;
    public string AccountHolderName { get { return accountHolderName; } set { accountHolderName = value; } }

    // Constructor - init all and set balance to 0
    public BankAccount(string AccountNumber, string AccountHolderName)
    {
        this.AccountNumber = AccountNumber;
        this.balance = 0;
        this.AccountHolderName = AccountHolderName;
    }

    public void Deposit(decimal amount)
    {
        this.balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (!(this.Balance - amount <= 0))
        {
            this.balance -= amount;
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"Account Number: {this.AccountNumber}, Account Holder: {this.AccountHolderName}, Balance: ${this.Balance}";
    }

    public bool Transfer(BankAccount recipient, decimal amount)
    {
        if (this.Balance - amount >= 0)
        {
            this.Withdraw(amount);
            recipient.Deposit(amount);
            return true;
        }
        return false;
    }
}