using Microsoft.EntityFrameworkCore;

namespace BankAPI.Database;

public class OffersDbContext : DbContext
{
    public OffersDbContext(DbContextOptions<OffersDbContext> options) : base(options)
    {
    }

    public DbSet<OfferEntity> Offers => Set<OfferEntity>();

    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
}