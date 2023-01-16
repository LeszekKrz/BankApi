using System.ComponentModel.DataAnnotations;
using BankAPI.Models.GovernmentId;
using BankAPI.Models.JobTypes;
using Newtonsoft.Json;

namespace BankAPI.Models.Offers;

public class OfferResponse
{
    [Required]
    [JsonProperty("id")]
    public Guid Id { get; init; }

    [Required]
    [JsonProperty("creationDate")]
    public DateTime CreationTime { get; init; }

    [Required]
    [JsonProperty("amount")]
    public decimal Amount { get; init; }

    [Required]
    [JsonProperty("installments")]
    public int NumberOfInstallments { get; init; }

    [Required]
    [JsonProperty("percentage")]
    public decimal Percentage { get; init; }

    [Required]
    [JsonProperty("firstName")]
    public string FirstName { get; init; } = null!;

    [Required]
    [JsonProperty("lastName")]
    public string LastName { get; init; } = null!;

    [Required]
    [JsonProperty("governmentId")]
    public GovernmentIdResponse GovernmentId { get; init; } = null!;

    [Required]
    [JsonProperty("income")]
    public int Income { get; init; }

    [Required]
    [JsonProperty("jobType")]
    public JobTypeResponse JobType { get; init; } = null!;
}