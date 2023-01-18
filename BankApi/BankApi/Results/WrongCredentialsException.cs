namespace BankAPI.Results;

public class WrongCredentialsException : ResultException
{
    public WrongCredentialsException(string username, string password) : base(
        $"Provided credentials are wrong: {username} + {password}")
    {
    }
}