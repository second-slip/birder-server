using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Birder.Services;

public interface IAuthenticationTokenService
{
    AuthenticationResultDto CreateToken(List<Claim> claims);
}

public class AuthenticationTokenService : IAuthenticationTokenService
{
    private readonly IConfiguration _configuration;
    private readonly ISystemClockService _systemClock;

    public AuthenticationTokenService(IConfiguration configuration, ISystemClockService systemClock)
    {
        _configuration = configuration;
        _systemClock = systemClock;
    }

    public AuthenticationResultDto CreateToken(List<Claim> claims)
    {
        if (!claims.Any()) {
            throw new ArgumentException($"The argument is null or empty", nameof(claims));
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var baseUrl = string.Concat(_configuration["Scheme"], _configuration["Domain"]);

        var tokenOptions = new JwtSecurityToken(
            issuer: baseUrl,
            audience: baseUrl,
            claims: claims,
            expires: _systemClock.GetNow.AddDays(2),
            signingCredentials: signinCredentials);

        var viewModel = new AuthenticationResultDto()
        {
            FailureReason = AuthenticationFailureReason.None,
            AuthenticationToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
        };

        return viewModel;
    }
}
