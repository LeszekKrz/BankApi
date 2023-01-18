using System.IdentityModel.Tokens.Jwt;
using BankAPI.Models.Authentication;
using BankAPI.Results;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConflictResult = BankAPI.Results.ConflictResult;

namespace BankAPI.Controllers;

[ApiController]
[Route("users")]
public sealed class UserController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly UserService _userService;

    public UserController(TokenService tokenService, UserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("auth")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthenticationResponse>> GetTokenAsync(AuthenticationRequest request)
    {
        var result = await _tokenService.GetTokenForUserAsync(request.Username, request.Password);
        if (result.Exception is not null)
            return result.Exception switch
            {
                NotFoundException e => NotFound(e.ToResponse()),
                WrongCredentialsException e => Unauthorized(e.Message),
                var _ => StatusCode(StatusCodes.Status500InternalServerError)
            };

        var token = result.Value;
        return new AuthenticationResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpirationTime = token.ValidTo
        };
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ConflictResult), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RegistrationResponse>> CreateUserAsync(RegistrationRequest request)
    {
        if (!UserService.IsRegistrationKeyValid(request.Key)) return Unauthorized("Provided key is invalid");
        var result = await _userService.CreateUserAsync(request.Username, request.Password);
        if (result.Exception is not null)
            return result.Exception switch
            {
                ObjectAlreadyExistsException e => Conflict(e.ToResponse()),
                var _ => StatusCode(StatusCodes.Status500InternalServerError)
            };

        var createdUser = result.Value;
        return new RegistrationResponse { Username = createdUser.Username };
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("exists")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CheckIfUserExistsAsync([FromQuery] string username)
    {
        var result = await _userService.GetUserByUsernameAsync(username);
        if (result.HasValue) return NoContent();
        return result.Exception! switch
        {
            NotFoundException e => NotFound(e.ToResponse()),
            var _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}