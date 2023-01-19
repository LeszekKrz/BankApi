using BankAPI.Database;

namespace BankAPI.Models;

public sealed class User
{
    public string Username { get; init; } = null!;

    public bool IsAdmin { get; init; }

    public static User FromEntity(UserEntity entity)
    {
        return new()
        {
            Username = entity.Username,
            IsAdmin = entity.IsAdmin
        };
    }
}