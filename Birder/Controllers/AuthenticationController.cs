using Birder.Data.Model;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISystemClockService _systemClock;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public AuthenticationController(UserManager<ApplicationUser> userManager
                                        , SignInManager<ApplicationUser> signInManager
                                        , ILogger<AuthenticationController> logger
                                        , ISystemClockService systemClock
                                        , IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _systemClock = systemClock;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // record and log model errors...
                    _logger.LogError(LoggingEvents.InvalidModelState, "LoginViewModel ModelState is invalid");
                    //return BadRequest(ModelState); //Not a good idea
                    var viewModel = new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other };
                    return BadRequest(viewModel);
                }

                var user = await _userManager.FindByEmailAsync(loginViewModel.UserName);

                if (user == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "Login failed: User not found");
                    var viewModel = new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other };
                    return NotFound(viewModel);
                }

                if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError("EmailNotConfirmed", "You cannot login until you have confirmed your email.");
                    _logger.LogInformation("EmailNotConfirmed", "You cannot login until you confirm your email.");
                    var viewModel = new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.EmailConfirmationRequired };
                    return BadRequest(viewModel);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

                //ToDo: move to separate Service?
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim("ImageUrl", user.Avatar),
                        new Claim("DefaultLatitude", user.DefaultLocationLatitude.ToString()),
                        new Claim("DefaultLongitude", user.DefaultLocationLongitude.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };


                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: _config["BaseUrl"], //_config["TokenIssuer"],
                        audience: _config["BaseUrl"], //_config["TokenAudience"],
                        claims: claims,
                        expires: _systemClock.GetNow.AddDays(2),
                        signingCredentials: signinCredentials);

                    var viewModel = new AuthenticationResultDto()
                    {
                        FailureReason = AuthenticationFailureReason.None,
                        AuthenticationToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
                    };
                    return Ok(viewModel);
                    //return Ok(new { Token = tokenString });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    var viewModel = new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.LockedOut };
                    return BadRequest(viewModel);
                }
                //if (result.RequiresTwoFactor) { }
                else
                {
                    _logger.LogInformation("EmailNotConfirmed", "You cannot login until you confirm your email.");
                    var viewModel = new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other };
                    return BadRequest(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GenerateItems, ex, "An error with user authenitication");
                var viewModel = new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other };
                return BadRequest(viewModel);
            }
        }
    }
}
