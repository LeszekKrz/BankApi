using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Authentication;

public sealed class AuthenticationRequest
{
    [Required]
    public string Username { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;
}