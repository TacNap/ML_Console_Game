public class NonLetterException : Exception
{
    // Parameterless constructor
    public NonLetterException()
    : base("Invalid user input encountered.") { }

    // Constructor with a custom message
    public NonLetterException(string message)
    : base(message) { }

    // Constructor with additional inner exception
    public NonLetterException(string message, Exception inner)
    : base(message, inner) { }
}