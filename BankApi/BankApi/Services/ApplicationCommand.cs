using BankAPI.Database;
using BankAPI.Models;
using BankAPI.Models.Applications;
using BankAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class ApplicationCommand
{
    private readonly ApplicationQuery _applicationQuery;
    private readonly OffersDbContext _context;
    private readonly DocumentService _documentService;
    private readonly OfferQuery _offerQuery;

    public ApplicationCommand(OfferQuery offerQuery, ApplicationQuery applicationQuery, OffersDbContext context,
        DocumentService documentService)
    {
        _offerQuery = offerQuery;
        _applicationQuery = applicationQuery;
        _context = context;
        _documentService = documentService;
    }

    public async Task<Result<Application>> CreateApplicationAsync(Guid offerId, Document document)
    {
        var offer = await _offerQuery.GetByIdAsync(offerId);
        if (offer is null) return new NotFoundException("Offer", offerId);

        var validationDetails = await _documentService.ValidateDocumentAsync(offerId, document);
        if (validationDetails.Errors.Any())
            return new BadRequestException("File", "Provided document is invalid");

        if (await _applicationQuery.GetByOfferIdAsync(offerId) is not null)
            return new BadRequestException("Offer ID", $"Application for offer {offerId} already exists");

        var application = Application.Create(offer);
        _context.Applications.Add(application.ToEntity());
        await _context.SaveChangesAsync();

        return application;
    }

    public async Task<Result<Application>> ReviewApplicationAsync(Guid offerId, bool accept)
    {
        var entity = await _context.Applications.Include(e => e.Offer).FirstOrDefaultAsync(e => e.OfferId == offerId);
        if (entity is null) return new NotFoundException("Application", offerId);

        if (entity.Status != ApplicationStatus.Pending)
            return new BadRequestException("Offer ID", $"Application for offer with ID {offerId} is already reviewed");

        entity.Status = accept ? ApplicationStatus.Accepted : ApplicationStatus.Rejected;
        entity.DecisionTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        await _context.SaveChangesAsync();

        return Application.FromEntity(entity);
    }
}