using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Authentication;

public sealed class AuthenticationResponse
{
    [Required]
    public string Token { get; init; } = null!;

    [Required]
    public DateTime ExpirationTime { get; init; }
}