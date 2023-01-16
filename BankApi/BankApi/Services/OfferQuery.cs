﻿using BankAPI.Database;
using BankAPI.Models.Offers;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public sealed class OfferQuery
{
    private readonly OffersDbContext _context;

    public OfferQuery(OffersDbContext context)
    {
        _context = context;
    }

    public async Task<Offer?> GetByIdAsync(Guid offerId)
    {
        var entity = await _context.Offers.FirstOrDefaultAsync(e => e.Id == offerId);
        return entity is null ? null : Offer.FromEntity(entity);
    }
}