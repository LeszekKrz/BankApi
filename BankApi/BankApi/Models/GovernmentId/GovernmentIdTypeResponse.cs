using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BankAPI.Models.GovernmentId;

public class GovernmentIdTypeResponse
{
    [Required]
    [JsonProperty("id")]
    public int TypeId { get; init; }

    [Required]
    public string Name { get; init; } = null!;
}