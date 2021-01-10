using Birder.Data.Model;
using Birder.Helpers;
using Birder.Services;
using Birder.Templates;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                    var url = _urlService.GetConfirmEmailUrl(newUser.UserName, code);
                    var templateData = new ConfirmEmailDto { Email = newUser.Email, Username = newUser.UserName, Url = url };
                    await _emailSender.SendEmailConfirmationEmailAsync(templateData);
                    return Ok(); //ToDo: Is this adequate?  Created reponse?
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
        public async Task<IActionResult> GetConfirmEmailAsync(string username, string code)
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
                    _logger.LogError(LoggingEvents.GetItemNotFound, $"User with username '{username}' not found");
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
        public async Task<IActionResult> PostResendConfirmEmailMessageAsync(UserEmailDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "User Not found");
                    return NotFound("User not found");
                }

                if (user.EmailConfirmed)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "User email is already confirmed");
                    return Ok();
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var url = _urlService.GetConfirmEmailUrl(user.UserName, code);

                var templateData = new ConfirmEmailDto { Email = user.Email, Username = user.UserName, Url = url };

                await _emailSender.SendEmailConfirmationEmailAsync(templateData);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in resend email confirmation.");
                return BadRequest("An error occurred");
            }
        }

        [HttpPost, Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> PostForgotPasswordAsync(UserEmailDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("An error occurred");
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, $"User with email '{model.Email}' was not found at forgot password");
                    return Ok(); // user does not exist, but don't reveal that the user does not exist
                }

                if (user.EmailConfirmed == false)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, $"Forgot password request when email '{model.Email}' is not confirmed");
                    return Ok(); // email is not confirmed
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = _urlService.GetResetPasswordUrl(code);

                var templateData = new ResetPasswordEmailDto { Email = model.Email, Url = url, Username = user.UserName };
                await _emailSender.SendResetPasswordEmailAsync(templateData);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in forgot password.");
                return BadRequest("An error occurred");
            }
        }

        [HttpPost, Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> PostResetPasswordAsync(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest(ModelState);
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(); // Don't reveal that the user does not exist
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }

                ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                return BadRequest("An error occurred");
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in forgot password.");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("IsUsernameAvailable")]
        [AllowAnonymous]
        public async Task<IActionResult> GetIsUsernameAvailableAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "An error occurred in is username available.");
                    throw new ArgumentException(username, "null or empty argument was passed to GetIsUsernameAvailableAsync()");
                }

                if (await _userManager.FindByNameAsync(username) != null)
                {
                    return Ok(false); //$"Username '{username}' is already taken..."
                }

                return Ok(true); //$"Username '{username}' is available..."
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in is username available.");
                return BadRequest("An unexpected error occurred");
            }
        }
    }
}