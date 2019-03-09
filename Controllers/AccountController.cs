using System;
using System.Threading.Tasks;
using Birder.Data;
using Birder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost, Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model) // , string returnUrl = null)
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

            // check passwords are equal... ?

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
}