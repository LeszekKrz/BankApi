using BankAPI.Database;
using BankAPI.Models.Offers;

namespace BankAPI.Models.Applications;

public sealed class Application
{
    public Offer Offer { get; init; } = null!;
    public DateTime CreationDate { get; init; }
    public DateTime? DecisionDate { get; set; }
    public ApplicationStatus Status { get; set; }

    public static Application Create(Offer offer)
    {
        return new Application
        {
            Offer = offer,
            CreationDate = DateTime.Now,
            DecisionDate = null,
            Status = ApplicationStatus.Pending
        };
    }

    public ApplicationEntity ToEntity()
    {
        return new ApplicationEntity
        {
            OfferId = Offer.Id,
            CreationTimestamp = ToTimestamp(CreationDate),
            DecisionTimestamp = DecisionDate is null ? null : ToTimestamp(DecisionDate.Value),
            Status = Status
        };
    }

    public static Application FromEntity(ApplicationEntity entity)
    {
        return new Application
        {
            CreationDate = FromTimestamp(entity.CreationTimestamp),
            DecisionDate = entity.DecisionTimestamp is null ? null : FromTimestamp(entity.DecisionTimestamp.Value),
            Status = entity.Status,
            Offer = Offer.FromEntity(entity.Offer)
        };
    }

    private static long ToTimestamp(DateTime time)
    {
        return new DateTimeOffset(time).ToUnixTimeMilliseconds();
    }

    private static DateTime FromTimestamp(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
    }

    public ApplicationResponse ToResponse()
    {
        return new ApplicationResponse
        {
            CreationDate = CreationDate,
            DecisionDate = DecisionDate,
            Status = Status switch
            {
                ApplicationStatus.Pending => "Pending",
                ApplicationStatus.Accepted => "Accepted",
                ApplicationStatus.Rejected => "Rejected",
                var _ => throw new ArgumentOutOfRangeException()
            },
            Offer = Offer.ToResponse()
        };
    }
}