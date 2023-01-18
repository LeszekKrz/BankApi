using System.ComponentModel.DataAnnotations;

namespace BankAPI.Results;

public sealed class ObjectAlreadyExistsException : ResultException
{
    public ObjectAlreadyExistsException(string objectName, object id) : base(
        $"{objectName} with ID {id} already exists")
    {
        Data["Name"] = objectName;
        Data["Id"] = id;
    }

    public ConflictResult ToResponse()
    {
        return new()
        {
            ObjectName = (string)Data["Name"]!,
            Id = Data["Id"]?.ToString()
        };
    }
}

public sealed class ConflictResult
{
    [Required]
    public string ObjectName { get; init; } = null!;

    public string? Id { get; init; }
}