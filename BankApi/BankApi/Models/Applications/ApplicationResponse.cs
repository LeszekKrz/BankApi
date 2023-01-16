using System.ComponentModel.DataAnnotations;
using BankAPI.Models.Offers;
using Newtonsoft.Json;

namespace BankAPI.Models.Applications;

public sealed class ApplicationResponse
{
    [Required]
    [JsonProperty("offer")]
    public OfferResponse Offer { get; init; } = null!;

    [Required]
    [JsonProperty("creationDate")]
    public DateTime CreationDate { get; init; }

    [JsonProperty("decisionDate")]
    public DateTime? DecisionDate { get; init; }

    [Required]
    [JsonProperty("status")]
    public string Status { get; init; } = null!;
}