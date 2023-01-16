using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BankAPI.Models.Applications;

public sealed class ApplicationCollectionRequest
{
    [Required]
    [JsonProperty("pendingOnly")]
    public bool PendingOnly { get; init; }

    [JsonProperty("maxResponseSize")]
    [Range(1, 1000)]
    public int MaxResponseSize { get; init; } = 100;

    [JsonProperty("start")]
    public DateTimeOffset? StartTime { get; init; }
}