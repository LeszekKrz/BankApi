using System.ComponentModel.DataAnnotations;
using BankAPI.Models.Applications;

namespace BankAPI.Database;

public sealed class ApplicationEntity
{
    [Key]
    public Guid OfferId { get; init; }

    [Required]
    public OfferEntity Offer { get; init; } = null!;

    [Required]
    public long CreationTimestamp { get; init; }

    public long? DecisionTimestamp { get; set; }

    [Required]
    public ApplicationStatus Status { get; set; }
}