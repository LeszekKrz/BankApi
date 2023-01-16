using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BankAPI.Models.Applications;

public sealed class ApplicationCollectionResponse
{
    [Required]
    [JsonProperty("applications")]
    public IReadOnlyList<ApplicationResponse> Applications { get; init; } = null!;

    [Required]
    [JsonProperty("cursor")]
    public string NextRequestCursor { get; init; } = null!;
}