namespace BankAPI.Results;

public sealed class RejectedException : ResultException
{
    public RejectedException() : base("Inquiry was rejected")
    {
    }
}