using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Birder.Services;

public interface IAuthenticationTokenService
{
    string CreateToken(ApplicationUser user);
}

public class AuthenticationTokenService : IAuthenticationTokenService
{
    private const int ExpirationHours = 48;
    private ConfigOptions Options { get; }

    private readonly ISystemClockService _systemClock;

    public AuthenticationTokenService(IOptions<ConfigOptions> optionsAccessor, ISystemClockService systemClock)
    {
        Options = optionsAccessor.Value;
        _systemClock = systemClock;
    }

    public string CreateToken(ApplicationUser user)
    {
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            CalculateTokenExpiry()
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private DateTime CalculateTokenExpiry()
    {
        var expiration = _systemClock.GetNow.AddHours(ExpirationHours);
        return expiration;
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            issuer: Options.BaseUrl,
            audience: Options.BaseUrl,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
            );

    private List<Claim> CreateClaims(ApplicationUser user)
    {
        try
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("ImageUrl", user.Avatar),
                    new Claim("Lat", user.DefaultLocationLatitude.ToString()),
                    new Claim("Lng", user.DefaultLocationLongitude.ToString())
                };

            return claims;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("error creating claims collection", e);
        }
    }
    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.TokenKey)),
            SecurityAlgorithms.HmacSha256
        );
    }
}