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
    public async Task<IActionResult> PostRegisterAsync(RegisterViewModel model) // [FromBody] removed
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
                await _emailSender.SendTemplateEmail("d-882e4b133cae40268364c8a929e55ea9", newUser.Email, templateData);
                return Ok(new { success = true }); //ToDo: Is this adequate?  Created reponse?
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
            if (string.IsNullOrEmpty(username) || code == null)
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

    [HttpPost, Route("resend-email-confirmation")]
    [AllowAnonymous]
    public async Task<IActionResult> PostResendConfirmEmailMessageAsync(UserEmailDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "User Not found");
                return NotFound("User not found");
            }

            if (user.EmailConfirmed)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "User email is already confirmed");
                return Ok(new { success = true });
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = _urlService.GetConfirmEmailUrl(user.UserName, code);

            var templateData = new { username = user.UserName, url = url };

            await _emailSender.SendTemplateEmail("d-882e4b133cae40268364c8a929e55ea9", user.Email, templateData);

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in resend email confirmation.");
            return BadRequest("An error occurred");
        }
    }

    [HttpPost, Route("request-password-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> PostRequestPasswordResetAsync(UserEmailDto model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                _logger.LogError($"Email null or empty at {nameof(PostRequestPasswordResetAsync)}");
                return BadRequest("An error occurred");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, $"User not found at forgot password");
                return Ok(new { success = true }); // user does not exist, but don't reveal that the user does not exist
            }

            if (user.EmailConfirmed == false)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, $"User's email is not confirmed");
                return Ok(new { success = true }); // email is not confirmed
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = _urlService.GetResetPasswordUrl(code);

            //    message.SetTemplateId("d-37733c23b2eb4c339a011dfadbd42b91");
            //    message.SetTemplateData(new ConfirmEmailDto { Username = accountDetails.Username, Url = accountDetails.Url });

            //var templateData = new ResetPasswordEmailDto { Email = model.Email, Url = url, Username = user.UserName };
            var templateData = new { username = user.UserName, url = url };
            //await _emailSender.SendResetPasswordEmailAsync(templateData);
            await _emailSender.SendTemplateEmail("d-37733c23b2eb4c339a011dfadbd42b91", model.Email, templateData);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in forgot password.");
            return BadRequest("An error occurred");
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
            return BadRequest("An error occurred");
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in forgot password.");
            return BadRequest("An error occurred");
        }
    }

    [HttpGet, Route("check-username")]
    [AllowAnonymous]
    public async Task<IActionResult> GetIsUsernameTakenAsync(UsernameDto model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                _logger.LogError($"Username null or empty at {nameof(GetIsUsernameTakenAsync)}");
                return BadRequest("An error occurred");
            }

            if (await _userManager.FindByNameAsync(model.Username) != null)
            {
                return Ok(new { usernameTaken = true }); //$"Username '{username}' is already taken..."
            }

            return Ok(new { usernameTaken = false }); //$"Username '{username}' is available..."
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred in is username available.");
            return BadRequest("An unexpected error occurred");
        }
    }

    [HttpGet, Route("check-email")]
    [AllowAnonymous]
    public async Task<IActionResult> GetIsEmailTakenAsync(UserEmailDto model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                _logger.LogError($"Email null or empty at {nameof(GetIsEmailTakenAsync)}");
                return BadRequest("An error occurred");
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return Ok(new { emailTaken = true }); //$"Email is already taken..."
            }

            return Ok(new { emailTaken = false }); //$"Email is not taken..."
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"An error occurred at {nameof(GetIsEmailTakenAsync)}.");
            return BadRequest("An unexpected error occurred");
        }
    }
}