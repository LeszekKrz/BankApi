using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Authentication;

public sealed class RegistrationRequest
{
    [Required]
    public string Username { get; init; } = null!;

    [Required]
    [MinLength(8)]
    public string Password { get; init; } = null!;

    [Required]
    public string Key { get; init; } = null!;
}