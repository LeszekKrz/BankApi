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

    public OfferController(OfferCommand command, DocumentService documentService)
    {
        _command = command;
        _documentService = documentService;
    }

    [HttpPost]
    [Route("inquire")]
    [ProducesResponseType(typeof(OfferResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<OfferResponse>> Post(InquiryRequest request)
    {
        var inquiry = Inquiry.FromRequest(request);
        var validationResult = inquiry.PerformAdditionalValidation();
        if (validationResult.Errors.Any())
            return BadRequest(BadRequestResponse.FromValidationProblemDetails(validationResult));

        var result = await _command.CreateOfferAsync(inquiry);
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
    public FileResult Get(Guid offerId)
    {
        var document = _documentService.GetOfferContract(offerId);
        return File(document.Content, document.Type, document.FileName);
    }
}