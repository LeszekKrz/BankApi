using BankAPI.Models;
using BankAPI.Models.Applications;
using BankAPI.Results;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly ApplicationCommand _command;
    private readonly ApplicationQuery _query;

    public ApplicationController(ApplicationCommand command, ApplicationQuery query)
    {
        _command = command;
        _query = query;
    }

    [HttpPost]
    [Route("apply/{offerId:guid}")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationResponse>> Post(Guid offerId, IFormFile file)
    {
        var result = await _command.CreateApplicationAsync(offerId, Document.FromFormFile(file));
        if (result.Exception is not null)
            return result.Exception switch
            {
                NotFoundException e => NotFound(e.ToResponse()),
                BadRequestException e => BadRequest(e.ToResponse()),
                var _ => StatusCode(StatusCodes.Status500InternalServerError)
            };

        return result.Value.ToResponse();
    }

    [HttpGet]
    [Route("application/{id:guid}")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationResponse>> Get(Guid id)
    {
        var result = await _query.GetByOfferIdAsync(id);
        return result is null
            ? NotFound(new NotFoundResponse
            {
                Id = id.ToString(),
                ResourceName = "Offer"
            })
            : result.ToResponse();
    }

    [HttpPost]
    [Route("application/{id:guid}/review")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationResponse>> Post(Guid id, ReviewRequest request)
    {
        var result = await _command.ReviewApplicationAsync(id, request.Accepted);
        if (result.Exception is not null)
            return result.Exception switch
            {
                NotFoundException e => NotFound(e.ToResponse()),
                BadRequestException e => BadRequest(e.ToResponse()),
                var _ => StatusCode(StatusCodes.Status500InternalServerError)
            };

        return result.Value.ToResponse();
    }

    [HttpGet]
    [Route("applications")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ApplicationResponse>>> Get(
        [FromQuery] ApplicationCollectionRequest request)
    {
        var applications = await _query.GetAllAsync(request.StartTime ?? DateTimeOffset.MinValue,
            request.MaxResponseSize,
            request.PendingOnly);
        return applications.Select(a => a.ToResponse()).ToList();
    }
}