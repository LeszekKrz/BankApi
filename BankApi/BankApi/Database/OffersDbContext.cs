using System.Security.Authentication;
using BankAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Database;

public class OffersDbContext : DbContext
{
    public OffersDbContext(DbContextOptions<OffersDbContext> options) : base(options)
    {
    }

    public DbSet<OfferEntity> Offers => Set<OfferEntity>();

    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public async Task EnsureAdminUserIsCreated()
    {
        var adminEnv = Environment.GetEnvironmentVariable("SEED_ADMIN_CREDENTIALS")
                       ?? throw new InvalidCredentialException(
                           "Environment variable SEED_ADMIN_CREDENTIALS is not defined");
        var split = adminEnv.Split(":", 2);
        if (split.Length < 2)
            throw new InvalidCredentialException("Environment variable SEED_ADMIN_CREDENTIALS is in invalid format");
        var adminUsername = split[0];
        var adminPassword = split[1];

        if (await Users.AnyAsync(e => e.Username == adminUsername)) return;

        Users.Add(new UserEntity
        {
            Username = adminUsername,
            HashedPassword = UserService.HashPassword(adminUsername, adminPassword),
            IsAdmin = true
        });
        await SaveChangesAsync();
    }
}