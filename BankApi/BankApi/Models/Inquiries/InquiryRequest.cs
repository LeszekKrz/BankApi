using System.ComponentModel.DataAnnotations;
using BankAPI.Models.GovernmentId;
using BankAPI.Models.JobTypes;
using Newtonsoft.Json;

namespace BankAPI.Models.Inquiries;

public sealed class InquiryRequest
{
    [Required]
    [Range(1, 1_000_000)]
    [JsonProperty("amount")]
    public decimal NeededAmount { get; init; }

    [Required]
    [Range(1, 12 * 100)]
    [JsonProperty("installments")]
    public int NumberOfInstallments { get; init; }

    [Required]
    [JsonProperty("firstName")]
    public string FirstName { get; init; } = null!;

    [Required]
    [JsonProperty("lastName")]
    public string LastName { get; init; } = null!;

    [Required]
    [JsonProperty("governmentId")]
    public GovernmentIdRequest GovernmentId { get; init; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    [JsonProperty("income")]
    public int Income { get; init; }

    [Required]
    [JsonProperty("jobType")]
    public JobTypeRequest JobType { get; init; } = null!;
}