
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuthenticationTokenService _authenticationTokenService;
    private readonly ILogger _logger;

    public AuthenticationController(UserManager<ApplicationUser> userManager
                                    , SignInManager<ApplicationUser> signInManager
                                    , ILogger<AuthenticationController> logger
                                    , IAuthenticationTokenService authenticationTokenService)
    {
        _logger = logger;
        _authenticationTokenService = authenticationTokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticationTokenService = authenticationTokenService;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)  // [FromBody] removed
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.UserName);

            if (user is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "Login failed: User not found");
                return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other });
            }

            if (user.EmailConfirmed == false)
            {
                _logger.LogInformation("EmailNotConfirmed", "You cannot login until you confirm your email.");
                return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.EmailConfirmationRequired });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.LockedOut });
            }

            if (result.Succeeded)
            {
                // todo: move to a static method/helper?
                var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim("ImageUrl", user.Avatar),
                        new Claim("Lat", user.DefaultLocationLatitude.ToString()),
                        new Claim("Lng", user.DefaultLocationLongitude.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                var model = _authenticationTokenService.CreateToken(claims);

                return Ok(model);
            }

            _logger.LogWarning(LoggingEvents.GenerateItems, "Other authentication failure");
            return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "An unexpected error occurred");
            return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other });
        }
    }
}
