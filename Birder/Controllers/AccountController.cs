using Birder.Data.Model;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISystemClockService _systemClock;
        private readonly IEmailSender _emailSender;
        private readonly IUrlService _urlService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ISystemClockService systemClock
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
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
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
                    Avatar = "https://img.icons8.com/color/96/000000/user.png", // "https://birderstorage.blob.core.windows.net/profile/default.png",
                    RegistrationDate = _systemClock.GetNow
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    //var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { username = newUser.UserName, code = code }));
                    var url = _urlService.ConfirmEmailUrl(newUser.UserName, code);

                    await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email", "Please confirm your account by clicking <a href=\"" + url + "\">here</a>");

                    return Ok();
                }

                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(error.Code, error.Description);
                //}
                ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);

                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
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

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(error.Code, error.Description);
                //}
                ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                return BadRequest(ModelState);
            }

            return Redirect("/confirmed-email");
        }

        [HttpPost, Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestConfirmEmailMessage(UserEmailDto model)
        {


            return Ok();
        }

        [HttpPost, Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(UserEmailDto model)
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

            _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
            return BadRequest();
        }

        [HttpPost, Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                //return RedirectToAction(nameof(ResetPasswordConfirmation));
                return Ok();
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
                //return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            //AddErrors(result);
            //return View();
            ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(error.Code, error.Description);
            //}

            _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
            return BadRequest(ModelState);
        }

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}

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
    }
}