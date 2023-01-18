using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankAPI.Configuration;
using BankAPI.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BankAPI.Services;

public class TokenService
{
    public const string AdminRoleName = "Admin";
    public const string UserRoleName = "User";
    private readonly IOptionsSnapshot<JwtTokenConfiguration> _config;

    private readonly UserService _userService;

    public TokenService(UserService userService, IOptionsSnapshot<JwtTokenConfiguration> config)
    {
        _userService = userService;
        _config = config;
    }

    public async Task<Result<JwtSecurityToken>> GetTokenForUserAsync(string username, string password)
    {
        var result = await _userService.GetUserByCredentialsAsync(username, password);
        if (result.Exception is not null) return result.Exception;

        var user = result.Value;
        return new JwtSecurityToken(
            _config.Value.Issuer,
            _config.Value.Audience,
            new Claim[]
            {
                new(ClaimTypes.Name, user.Username), 
                new(ClaimTypes.Role, UserRoleName)
            }.Concat(user.IsAdmin
                ? new[] { new Claim(ClaimTypes.Role, AdminRoleName) }
                : Array.Empty<Claim>()),
            expires: DateTime.Now.AddHours(_config.Value.ExpiresInHours),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key)),
                SecurityAlgorithms.HmacSha256
            )
        );
    }
}