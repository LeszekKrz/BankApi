using System.ComponentModel.DataAnnotations;

namespace BankAPI.Database;

public class OfferEntity
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public long CreationTimestamp { get; init; }

    [Required]
    public decimal Amount { get; init; }

    [Required]
    public int NumberOfInstallments { get; init; }

    [Required]
    public decimal Percentage { get; init; }

    [Required]
    public string FirstName { get; init; } = null!;

    [Required]
    public string LastName { get; init; } = null!;

    [Required]
    public int GovernmentIdTypeId { get; init; }

    [Required]
    public string GovernmentIdName { get; init; } = null!;

    [Required]
    public string GovernmentIdValue { get; init; } = null!;

    [Required]
    public int JobTypeId { get; init; }

    [Required]
    public int Income { get; init; }

    // This is needed so that EF Core understands that offers and applications have 1-to-1 relationship
    public ApplicationEntity? Application { get; set; }
}