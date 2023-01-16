namespace BankAPI.Results;

public abstract class ResultException : ApplicationException
{
    protected ResultException(string message) : base(message)
    {
    }
}