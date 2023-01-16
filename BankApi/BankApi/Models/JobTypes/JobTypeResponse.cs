using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.JobTypes;

public sealed class JobTypeResponse
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; } = null!;
}