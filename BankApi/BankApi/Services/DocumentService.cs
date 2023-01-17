using BankAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Services;

public sealed class DocumentService
{
    public Document GetOfferContract(Guid offerId)
    {
        return Document.Create($"contract-{offerId.ToString()}", $"This is a contract for offer {offerId.ToString()}");
    }

    public async Task<ValidationProblemDetails> ValidateDocumentAsync(Guid offerId, Document file)
    {
        const string expectedType = "text/plain";
        var validationDetails = new ValidationProblemDetails();
        if (file.Type != expectedType)
            validationDetails.Errors.Add("Content type",
                new[] { $"File has type {file.Type}, but expected type is {expectedType}" });
        var content = await new StreamReader(file.Content).ReadToEndAsync();
        var expectedContent = await new StreamReader(GetOfferContract(offerId).Content).ReadToEndAsync();
        if (!content.StartsWith(expectedContent))
            validationDetails.Errors.Add("File content",
                new[] { "File content does not start with text returned in an original document" });

        return validationDetails;
    }
}