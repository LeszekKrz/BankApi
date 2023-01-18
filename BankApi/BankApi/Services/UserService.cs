using BankAPI.Database;
using BankAPI.Models;
using BankAPI.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public sealed class UserService
{
    private readonly OffersDbContext _context;

    public UserService(OffersDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetUserByCredentialsAsync(string username, string password)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(e => e.Username == username);
        if (entity is null) return new NotFoundException("User", username);

        if (!IsPasswordCorrect(entity.Username, password, entity.HashedPassword))
            return new WrongCredentialsException(username, password);

        return User.FromEntity(entity);
    }

    public async Task<Result<User>> GetUserByUsernameAsync(string username)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(e => e.Username == username);
        if (entity is null) return new NotFoundException("User", username);
        return User.FromEntity(entity);
    }

    public async Task<Result<User>> CreateUserAsync(string username, string password)
    {
        if (await _context.Users.AnyAsync(e => e.Username == username))
            return new ObjectAlreadyExistsException("User", username);

        var newEntity = new UserEntity
        {
            Username = username,
            HashedPassword = HashPassword(username, password),
            IsAdmin = false
        };
        _context.Users.Add(newEntity);
        await _context.SaveChangesAsync();

        return User.FromEntity(newEntity);
    }

    public static string HashPassword(string username, string password)
    {
        var hasher = new PasswordHasher<string>();
        return hasher.HashPassword(username, password);
    }

    private static bool IsPasswordCorrect(string username, string suppliedPassword, string expectedHash)
    {
        var hasher = new PasswordHasher<string>();
        var result = hasher.VerifyHashedPassword(username, expectedHash, suppliedPassword);
        return result != PasswordVerificationResult.Failed;
    }

    public static bool IsRegistrationKeyValid(string key)
    {
        var expectedKey = Environment.GetEnvironmentVariable("REGISTRATION_KEY")
                          ?? throw new InvalidOperationException(
                              "Environment variable REGISTRATION_KEY is not defined");
        return expectedKey == key;
    }
}