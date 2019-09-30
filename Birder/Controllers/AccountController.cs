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
        public async Task<IActionResult> PostRegisterAsync([FromBody]RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest(ModelState);
                }

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
                    var url = _urlService.ConfirmEmailUrl(newUser.UserName, code);
                    await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email", "Please confirm your account by clicking <a href=\"" + url + "\">here</a>");
                    return Ok("New user successfully created");
                }

                ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in new user registration.");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("ConfirmEmail", Name = "ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync(string username, string code)
        {
            try
            {
                if (username == null || code == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, $"Null arguments passed to ConfirmEmailAsync: username = {username}; code = {code}.");
                    return BadRequest("An error occurred");
                }

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("An error occurred");
                }

                return Redirect("/confirmed-email");
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in email confirmation.");
                return BadRequest("An error occurred");
            }
        }

        [HttpPost, Route("ResendEmailConfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmEmailMessage(UserEmailDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "User Not found");
                return BadRequest("User Not found");
            }

            if (user.EmailConfirmed)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "User email is already confirmed");
                return Ok();
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = _urlService.ConfirmEmailUrl(user.UserName, code);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", "Please confirm your account by clicking <a href=\"" + url + "\">here</a>");          

            return Ok(url);
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

            _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
            return BadRequest(ModelState);
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
    }
}