using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BankAPI.Models.Applications;

public sealed class ReviewRequest
{
    [Required]
    [JsonProperty("accept")]
    public bool Accepted { get; init; }
}