using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Authentication;

public sealed class RegistrationResponse
{
    [Required]
    public string Username { get; init; } = null!;
}