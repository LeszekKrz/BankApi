using System.Security.Claims;
using BankAPI.Models;
using BankAPI.Models.Applications;
using BankAPI.Results;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly ApplicationCommand _applicationCommand;
    private readonly ApplicationQuery _applicationQuery;
    private readonly OfferQuery _offerQuery;

    public ApplicationController(ApplicationCommand applicationCommand, ApplicationQuery applicationQuery,
        OfferQuery offerQuery)
    {
        _applicationCommand = applicationCommand;
        _applicationQuery = applicationQuery;
        _offerQuery = offerQuery;
    }

    [HttpPost]
    [Route("apply/{offerId:guid}")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApplicationResponse>> Post(Guid offerId, IFormFile file)
    {
        var ownershipResult = await _offerQuery.CheckOwnerAsync(offerId, GetUsername());
        if (ownershipResult == OwnershipTestResult.ResourceDoesNotExist)
            return NotFound(new NotFoundResponse
            {
                ResourceName = "Offer",
                Id = offerId.ToString()
            });
        if (ownershipResult == OwnershipTestResult.Unauthorized)
            return Unauthorized();

        var result = await _applicationCommand.CreateApplicationAsync(offerId, Document.FromFormFile(file));
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
    [Route("applications/{id:guid}")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApplicationResponse>> Get(Guid id)
    {
        var ownershipResult = await _offerQuery.CheckOwnerAsync(id, GetUsername());
        if (ownershipResult == OwnershipTestResult.ResourceDoesNotExist)
            return NotFound(new NotFoundResponse
            {
                ResourceName = "Offer",
                Id = id.ToString()
            });
        if (ownershipResult == OwnershipTestResult.Unauthorized)
            return Unauthorized();

        var result = await _applicationQuery.GetByOfferIdAsync(id);
        return result is null
            ? NotFound(new NotFoundResponse
            {
                ResourceName = "Application",
                Id = id.ToString()
            })
            : result.ToResponse();
    }

    [HttpPost]
    [Authorize(Roles = TokenService.AdminRoleName)]
    [Route("applications/{id:guid}/review")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApplicationResponse>> Post(Guid id, ReviewRequest request)
    {
        var result = await _applicationCommand.ReviewApplicationAsync(id, request.Accepted);
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
    [Authorize(Roles = TokenService.AdminRoleName)]
    [Route("applications")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<ApplicationResponse>>> Get(
        [FromQuery] ApplicationCollectionRequest request)
    {
        var applications = await _applicationQuery.GetAllAsync(request.StartTime ?? DateTimeOffset.MinValue,
            request.MaxResponseSize,
            request.PendingOnly);
        return applications.Select(a => a.ToResponse()).ToList();
    }

    private string GetUsername()
    {
        return User.FindFirstValue(ClaimTypes.Name)
               ?? throw new InvalidStateException("JwtToken", "Token does not contain Name claim");
    }
}