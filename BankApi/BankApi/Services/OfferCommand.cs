using BankAPI.Database;
using BankAPI.Models.Inquiries;
using BankAPI.Models.Offers;
using BankAPI.Results;

namespace BankAPI.Services;

public sealed class OfferCommand
{
    private readonly OffersDbContext _context;

    public OfferCommand(OffersDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Offer>> CreateOfferAsync(Inquiry inquiry)
    {
        var percentage = DetermineOfferPercentageForInquiry(inquiry);
        if (percentage is null) return new RejectedException();

        var offer = Offer.FromInquiryWithPercentage(inquiry, percentage.Value);
        _context.Offers.Add(offer.ToEntity());
        await _context.SaveChangesAsync();

        return offer;
    }

    private static decimal? DetermineOfferPercentageForInquiry(Inquiry inquiry)
    {
        var percentage = inquiry.NeededAmount switch
        {
            <= 10_000 => 20,
            > 10_000 and <= 200_000 => 20 - 10 * (inquiry.NeededAmount - 10_000) / (200_000 - 10_000),
            > 200_000 => 10
        };

        var monthlyInstallment = inquiry.NeededAmount * (1 + percentage / 100) / inquiry.NumberOfInstallments;
        var adjustedIncome = inquiry.PersonalDetails.JobType.IsKnown()
            ? inquiry.PersonalDetails.Income
            : inquiry.PersonalDetails.Income / 2;
        return monthlyInstallment > 0.3m * adjustedIncome ? null : percentage;
    }
}