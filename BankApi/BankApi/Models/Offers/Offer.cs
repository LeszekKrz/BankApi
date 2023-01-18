using BankAPI.Database;
using BankAPI.Models.Inquiries;
using BankAPI.Models.JobTypes;

namespace BankAPI.Models.Offers;

public sealed class Offer
{
    public Guid Id { get; private init; }
    public DateTime CreationTime { get; init; }
    public decimal Amount { get; init; }
    public int Installments { get; init; }
    public decimal Percentage { get; init; }
    public PersonalData Recipient { get; init; } = null!;
    public string OwnerUsername { get; init; } = null!;

    public OfferResponse ToResponse()
    {
        return new OfferResponse
        {
            Id = Id,
            CreationTime = CreationTime,
            Amount = Amount,
            NumberOfInstallments = Installments,
            Percentage = Percentage,
            FirstName = Recipient.FirstName,
            LastName = Recipient.LastName,
            GovernmentId = Recipient.Id.ToResponse(),
            JobType = Recipient.JobType.ToResponse(),
            Income = Recipient.Income
        };
    }

    public OfferEntity ToEntity()
    {
        return new()
        {
            Id = Id,
            CreationTimestamp = new DateTimeOffset(CreationTime).ToUnixTimeMilliseconds(),
            Amount = Amount,
            NumberOfInstallments = Installments,
            Percentage = Percentage,
            FirstName = Recipient.FirstName,
            LastName = Recipient.LastName,
            GovernmentIdTypeId = Recipient.Id.GetTypeId(),
            GovernmentIdName = Recipient.Id.Name,
            GovernmentIdValue = Recipient.Id.Value,
            JobTypeId = Recipient.JobType.Id,
            Income = Recipient.Income,
            OwnerUsername = OwnerUsername
        };
    }

    public static Offer FromEntity(OfferEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            CreationTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.CreationTimestamp).UtcDateTime,
            Amount = entity.Amount,
            Installments = entity.NumberOfInstallments,
            Percentage = entity.Percentage,
            Recipient = new PersonalData(
                entity.FirstName,
                entity.LastName,
                GovernmentId.GovernmentId.Create(entity.GovernmentIdTypeId, entity.GovernmentIdName,
                    entity.GovernmentIdValue),
                JobType.GetById(entity.JobTypeId),
                entity.Income
            ),
            OwnerUsername = entity.OwnerUsername
        };
    }

    public static Offer FromInquiryWithPercentage(Inquiry inquiry, decimal percentage, string ownerUsername)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTime.Now,
            Amount = inquiry.NeededAmount,
            Installments = inquiry.NumberOfInstallments,
            Percentage = percentage,
            Recipient = inquiry.PersonalDetails,
            OwnerUsername = ownerUsername
        };
    }
}