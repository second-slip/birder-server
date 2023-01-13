using Microsoft.Extensions.Options;
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
    private readonly ISystemClockService _systemClock;

    public AuthenticationTokenService(ISystemClockService systemClock
    , IOptions<AuthConfigOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
        _systemClock = systemClock;
    }

    public AuthConfigOptions Options { get; }

    public AuthenticationResultDto CreateToken(List<Claim> claims)
    {
        if (claims is null)
        {
            throw new ArgumentException($"The argument is null or empty", nameof(claims));
        }
        if (!claims.Any())
        {
            throw new ArgumentException($"The argument is null or empty", nameof(claims));
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.TokenKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: Options.BaseUrl,
            audience: Options.BaseUrl,
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