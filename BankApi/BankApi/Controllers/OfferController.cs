using System.Security.Claims;
using BankAPI.Models.Inquiries;
using BankAPI.Models.Offers;
using BankAPI.Results;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
public sealed class OfferController : ControllerBase
{
    private readonly OfferCommand _command;
    private readonly DocumentService _documentService;
    private readonly OfferQuery _query;

    public OfferController(OfferCommand command, OfferQuery query, DocumentService documentService)
    {
        _command = command;
        _query = query;
        _documentService = documentService;
    }

    [HttpPost]
    [Route("inquire")]
    [ProducesResponseType(typeof(OfferResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OfferResponse>> Post(InquiryRequest request)
    {
        var inquiry = Inquiry.FromRequest(request);
        var validationResult = inquiry.PerformAdditionalValidation();
        if (validationResult.Errors.Any())
            return BadRequest(BadRequestResponse.FromValidationProblemDetails(validationResult));

        var result = await _command.CreateOfferAsync(inquiry, GetUsername());
        if (result.Exception is not null)
            return result.Exception switch
            {
                RejectedException => UnprocessableEntity(),
                var _ => StatusCode(StatusCodes.Status500InternalServerError)
            };

        return result.Value.ToResponse();
    }

    [HttpGet]
    [Route("offers/{offerId:guid}/document")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Get(Guid offerId)
    {
        var result = await _query.CheckOwnerAsync(offerId, GetUsername());
        if (result == OwnershipTestResult.ResourceDoesNotExist)
            return NotFound(new NotFoundResponse
            {
                ResourceName = "Offer",
                Id = offerId.ToString()
            });
        if (result == OwnershipTestResult.Unauthorized)
            return Unauthorized();

        var document = _documentService.GetOfferContract(offerId);
        return File(document.Content, document.Type, document.FileName);
    }

    private string GetUsername()
    {
        return User.FindFirstValue(ClaimTypes.Name)
               ?? throw new InvalidStateException("JwtToken", "Token does not contain Name claim");
    }
}