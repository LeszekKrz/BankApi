using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.JobTypes;

public sealed class JobTypeRequest
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; } = null!;
}