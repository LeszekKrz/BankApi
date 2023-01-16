using BankAPI.Models.JobTypes;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Models.Inquiries;

public sealed class Inquiry
{
    public decimal NeededAmount { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public PersonalData PersonalDetails { get; private init; } = null!;

    public static Inquiry FromRequest(InquiryRequest request)
    {
        return new Inquiry
        {
            NeededAmount = request.NeededAmount,
            NumberOfInstallments = request.NumberOfInstallments,
            PersonalDetails = new PersonalData(
                request.FirstName,
                request.LastName,
                GovernmentId.GovernmentId.FromRequest(request.GovernmentId),
                JobType.FromRequest(request.JobType),
                request.Income
            )
        };
    }

    public ValidationProblemDetails PerformAdditionalValidation()
    {
        var details = new ValidationProblemDetails();
        if (!PersonalDetails.JobType.IsValid())
            details.Errors.Add("Job type", new[]
            {
                $@"Provided job name {PersonalDetails.JobType.Name} does not match provided ID " +
                $@"{PersonalDetails.JobType.Id}. To provide an unlisted job, ID must be set to number corresponding " +
                @"to ""Other"" job type."
            });

        if (!PersonalDetails.Id.IsValid())
            details.Errors.Add("Id",
                new[] { "Provided government ID is not valid, or type ID does not match the name" });

        return details;
    }
}

public sealed record PersonalData(string FirstName, string LastName, GovernmentId.GovernmentId Id, JobType JobType,
    int Income);