using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Birder.Data;
using Birder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost, Route("login")] //[HttpPost("[action]")]
        public IActionResult Login([FromBody]LoginViewModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.UserName == "a@b.com" && user.Password == "test")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:55722",
                    audience: "http://localhost:55722",
                    claims: claims,  // new List<Claim>(),
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new { Token = tokenString });
                //return Ok(uvm);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model) // , string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DefaultLocationLatitude = 54.972237,
                DefaultLocationLongitude = -2.4608560000000352,
                // ProfileImage = "https://birderstorage.blob.core.windows.net/profile/default.png",
                RegistrationDate = DateTime.Now // _systemClock.Now
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                //_logger.LogInformation("User created a new account with password.");

                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                // await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                // //await _signInManager.SignInAsync(user, isPersistent: false);
                // _logger.LogInformation("User created a new account with password.");
                //return RedirectToLocal(returnUrl);
                //return RedirectToPage("/ConfirmYourEmail");
                // return RedirectToAction("Welcome","Home");
                return Ok();
            }
                //AddErrors(result);
            

            // If we got this far, something failed, redisplay form
            return BadRequest();
            // return View(model);
        }
    }

    public class RegisterViewModel
    {
        // [Required]
        // [Display(Name = "Username")]
        public string UserName { get; set; }

        // [Required]
        // [EmailAddress]
        // [Display(Name = "Email")]
        public string Email { get; set; }

        // [Required]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        // [DataType(DataType.Password)]
        // [Display(Name = "Password")]
        public string Password { get; set; }

        // [DataType(DataType.Password)]
        // [Display(Name = "Confirm password")]
        // [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }




    //Too: Move to separate file
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}