using BankAPI.Database;
using BankAPI.Models.Applications;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public sealed class ApplicationQuery
{
    private readonly OffersDbContext _context;

    public ApplicationQuery(OffersDbContext context)
    {
        _context = context;
    }

    public async Task<Application?> GetByOfferIdAsync(Guid offerId)
    {
        var entity = await _context.Applications.Include(e => e.Offer).FirstOrDefaultAsync(e => e.OfferId == offerId);
        return entity is null ? null : Application.FromEntity(entity);
    }

    public async Task<IReadOnlyList<Application>> GetAllAsync(DateTimeOffset startTime, int limit,
        bool pendingOnly = false)
    {
        var query = _context.Applications.Include(e => e.Offer).
            Where(e => e.CreationTimestamp >= startTime.ToUnixTimeMilliseconds());
        if (pendingOnly) query = query.Where(e => e.Status == ApplicationStatus.Pending);

        return await query.OrderBy(e => e.CreationTimestamp).
            ThenBy(e => e.OfferId.ToString()).
            Take(limit).
            Select(e => Application.FromEntity(e)).
            ToListAsync();
    }
}