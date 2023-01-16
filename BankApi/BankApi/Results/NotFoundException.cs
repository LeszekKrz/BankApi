using System.ComponentModel.DataAnnotations;

namespace BankAPI.Results;

public sealed class NotFoundException : ResultException
{
    public NotFoundException(string objectType, object id) : base($"{objectType} with ID {id} was not found")
    {
        Data["Type"] = objectType;
        Data["Id"] = id;
    }

    public NotFoundResponse ToResponse()
    {
        return new()
        {
            ResourceName = (string)Data["Type"]!,
            Id = (string)Data["Id"]!
        };
    }
}

public sealed class NotFoundResponse
{
    [Required]
    public string ResourceName { get; init; } = null!;

    [Required]
    public string Id { get; init; } = null!;
}