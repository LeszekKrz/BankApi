namespace BankAPI;

public sealed class InvalidStateException : Exception
{
    public InvalidStateException(string type, string message) : base($"{type} object is in invalid state: {message}")
    {
    }
}