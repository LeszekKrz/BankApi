using System.ComponentModel.DataAnnotations;

namespace BankAPI.Configuration;

public sealed class JwtTokenConfiguration
{
    public const string SectionName = "JwtToken";

    [Required]
    public string Issuer { get; init; } = null!;

    [Required]
    public string Audience { get; init; } = null!;

    [Required]
    [Range(1, 24 * 7)]
    public int ExpiresInHours { get; init; }

    [Required]
    public string Key { get; init; } = null!;
}