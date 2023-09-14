using System.Net.Mime;

namespace Birder.Controllers;

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

    [HttpPost, Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> PostRegisterAsync(RegisterViewModel model)
    {
        try
        {
            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DefaultLocationLatitude = 54.972237,
                DefaultLocationLongitude = -2.4608560000000352,
                Avatar = "https://img.icons8.com/color/96/000000/user.png",
                RegistrationDate = _systemClock.GetNow
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var url = _urlService.GetConfirmEmailUrl(newUser.UserName, code);
                var templateData = new { username = newUser.UserName, url = url };
                var msg = _emailSender.CreateMailMessage(SendGridTemplateId.ConfirmEmail, newUser.Email, templateData);
                await _emailSender.SendMessageAsync(msg);

                return Ok(new { success = true }); //ToDo: Is this adequate?  Created reponse?
            }

            ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
            _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(PostRegisterAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet, Route("ConfirmEmail", Name = "ConfirmEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> GetConfirmEmailAsync(string username, string code)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, $"action argument invalid: {nameof(username)}");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, $"action argument invalid: {nameof(code)}");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, $"user with username '{username}' not found");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Redirect("/confirmed-email");
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(GetConfirmEmailAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost, Route("resend-email-confirmation")]
    [AllowAnonymous]
    public async Task<IActionResult> PostResendConfirmEmailMessageAsync(UserEmailDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, $"user with email '{model.Email}' not found");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (user.EmailConfirmed)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, $"user with email '{model.Email}' is already confirmed");
                return Ok(new { success = true });
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = _urlService.GetConfirmEmailUrl(user.UserName, code);
            var templateData = new { username = user.UserName, url = url };
            var msg = _emailSender.CreateMailMessage(SendGridTemplateId.ConfirmEmail, user.Email, templateData);
            await _emailSender.SendMessageAsync(msg);

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(PostResendConfirmEmailMessageAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost, Route("request-password-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> PostRequestPasswordResetAsync(UserEmailDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, $"user with email '{model.Email}' not found");
                return Ok(new { success = true }); // user does not exist, but don't reveal that the user does not exist
            }

            if (user.EmailConfirmed == false)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, $"user with email '{model.Email}' has not confirmed their email");
                return Ok(new { success = true }); // email is not confirmed
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = _urlService.GetResetPasswordUrl(code);
            var templateData = new { username = user.UserName, url = url };
            var msg = _emailSender.CreateMailMessage(SendGridTemplateId.PasswordResetRequest, model.Email, templateData);
            await _emailSender.SendMessageAsync(msg);

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(PostRequestPasswordResetAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost, Route("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> PostResetPasswordAsync(ResetPasswordViewModel model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return Ok(new { success = true }); // Don't reveal that the user does not exist
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { success = true });
            }

            ModelStateErrorsExtensions.AddIdentityErrors(ModelState, result);
            _logger.LogError(LoggingEvents.UpdateItemNotFound, "Invalid model state:" + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(PostResetPasswordAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet, Route("check-username")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIsUsernameTakenAsync(string username)
    {
        if (!RegexHelpers.IsValidUsername(username))
        {
            _logger.LogError($"invalid username paramater supplied at action: {nameof(GetIsUsernameTakenAsync)}");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            if (await _userManager.FindByNameAsync(username) != null)
            {
                return Ok(new { usernameTaken = true });
            }

            return Ok(new { usernameTaken = false });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(GetIsUsernameTakenAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet, Route("check-email")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIsEmailTakenAsync(string email)
    {
        if (!RegexHelpers.IsValidEmail(email))
        {
            _logger.LogError($"invalid email paramater supplied at action: {nameof(GetIsEmailTakenAsync)}");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return Ok(new { emailTaken = true });
            }

            return Ok(new { emailTaken = false });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an unexpected error occurred at method: {nameof(GetIsEmailTakenAsync)}.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}