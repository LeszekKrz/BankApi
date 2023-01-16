using BankAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Services;

public sealed class DocumentService
{
    public Document GetOfferContract(Guid offerId)
    {
        return Document.Create($"contract-{offerId.ToString()}", $"This is a contract for offer {offerId.ToString()}");
    }

    public ValidationProblemDetails ValidateDocument(Guid offerId, Document file)
    {
        const string expectedType = "text/plain";
        var validationDetails = new ValidationProblemDetails();
        if (file.Type != expectedType)
            validationDetails.Errors.Add("Content type",
                new[] { $"File has type {file.Type}, but expected type is {expectedType}" });
        if (!file.FileName.Contains(offerId.ToString()))
            validationDetails.Errors.Add("File name",
                new[] { $"File name {file.FileName} does not contain offer ID: {offerId.ToString()}" });

        return validationDetails;
    }
}