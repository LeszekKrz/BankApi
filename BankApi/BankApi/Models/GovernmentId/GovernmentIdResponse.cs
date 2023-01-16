using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BankAPI.Models.GovernmentId;

public sealed class GovernmentIdResponse
{
    [Required]
    [JsonProperty("id")]
    public int TypeId { get; init; }

    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string Value { get; init; } = null!;
}