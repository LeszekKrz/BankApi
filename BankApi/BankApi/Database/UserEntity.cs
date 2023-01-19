using System.ComponentModel.DataAnnotations;

namespace BankAPI.Database;

public sealed class UserEntity
{
    [Key]
    [Required]
    public string Username { get; init; } = null!;

    [Required]
    public string HashedPassword { get; init; } = null!;

    [Required]
    public bool IsAdmin { get; init; }
}