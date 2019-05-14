using Birder.Data.Model;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISystemClock _systemClock;
        private readonly IEmailSender _emailSender;
        private readonly IUrlService _urlService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ISystemClock systemClock
                               , IUrlService urlService
                               , IEmailSender emailSender
                               , ILogger<AccountController> logger
                               , UserManager<ApplicationUser> userManager)
        {
            _systemClock = systemClock;
            _emailSender = emailSender;
            _userManager = userManager;
            _urlService = urlService;
            _logger = logger;
        }

        [HttpPost, Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log modelstate errors
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    DefaultLocationLatitude = 54.972237,
                    DefaultLocationLongitude = -2.4608560000000352,
                    ProfileImage = "https://img.icons8.com/color/96/000000/user.png", // "https://birderstorage.blob.core.windows.net/profile/default.png",
                    RegistrationDate = _systemClock.GetNow
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    // var callbackUrl = Url.Page(
                    //     "#",
                    //     pageHandler: null,
                    //     values: new { userId = newUser.Id, code = code },
                    //     protocol: Request.Scheme);

                    var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { username = newUser.UserName, code = code }));

                    await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return Ok();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in new user registration.");
                return BadRequest();
            }
        }

        [HttpGet, Route("ConfirmEmail", Name = "ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string username, string code)
        {

            if (username == null || code == null)
            {
                return BadRequest(); // error with email confirmation
            }

            var user = await _userManager.FindByNameAsync(username); // FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Redirect("/confirmed-email");

            // return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost, Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return Ok(); // user does not exist or is not confirmed
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = _urlService.ResetPasswordUrl(code);

                await _emailSender.SendEmailAsync(model.Email, "Reset Your Password",
                    "You can reset your password by clicking <a href=\"" + url + "\">here</a>");
                return Ok(url);
            }
            return BadRequest();
        }





        [HttpGet, Route("IsUsernameAvailable")]
        [AllowAnonymous]
        public async Task<ActionResult<Boolean>> IsUsernameAvailable(string userName)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
            {
                ModelState.AddModelError("Username", $"Username '{userName}' is already taken.");
                return BadRequest(ModelState);
            }

            return Ok(true);
        }


        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public class ResetPasswordViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
    }
}