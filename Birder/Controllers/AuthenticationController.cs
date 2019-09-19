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
        private readonly ILogger _logger;
        private readonly ISystemClockService _systemClock;
        private readonly IConfiguration _config;

        public AuthenticationController(UserManager<ApplicationUser> userManager
                                        , SignInManager<ApplicationUser> signInManager
                                        , ILogger<AuthenticationController> logger
                                        , ISystemClockService systemClock
                                        , IConfiguration config)
        {
            _systemClock = systemClock;
            _logger = logger;
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(LoggingEvents.InvalidModelState, "LoginViewModel ModelState is invalid");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.UserName);

            if (user != null)
            {

                if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError("EmailNotConfirmed", "You cannot login until you confirm your email.");
                    return BadRequest(ModelState);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

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

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                    var tokenOptions = new JwtSecurityToken(
                        issuer: _config["Tokens:Issuer"],
                        audience: "http://localhost:55722",
                        claims: claims,
                        expires: _systemClock.GetNow.AddDays(2),
                        signingCredentials: signinCredentials
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new { Token = tokenString });
                    //return Ok(tokenString);
                }
            }
            
            _logger.LogError(LoggingEvents.GetItemNotFound, "Login failed: User not found");
            return BadRequest(ModelState);
        }
    }
}