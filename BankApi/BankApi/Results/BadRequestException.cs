using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BankAPI.Results;

public sealed class BadRequestException : ResultException
{
    public BadRequestException(string argumentName, string message) : base($"{argumentName}: {message}")
    {
        Data["ArgumentName"] = argumentName;
        Data["Message"] = message;
    }

    public BadRequestResponse ToResponse()
    {
        return new BadRequestResponse
        {
            Problems = new[]
            {
                new BadRequestResponse.BadRequestItem
                {
                    ArgumentName = (string)Data["ArgumentName"]!,
                    Message = (string)Data["Message"]!
                }
            }
        };
    }
}

public sealed class BadRequestResponse
{
    [Required]
    public IReadOnlyList<BadRequestItem> Problems { get; init; } = null!;

    public static BadRequestResponse FromValidationProblemDetails(ValidationProblemDetails details)
    {
        return new()
        {
            Problems = details.Errors.SelectMany(p => p.Value.Select(e => new BadRequestItem
            {
                ArgumentName = p.Key,
                Message = e
            })).
                ToList()
        };
    }

    public sealed class BadRequestItem
    {
        [Required]
        [JsonProperty("valueName")]
        public string ArgumentName { get; init; } = null!;

        [Required]
        [JsonProperty("message")]
        public string Message { get; init; } = null!;
    }
}