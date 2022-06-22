using AutoMapper;
using Birder.Data.Model;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ManageController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUrlService _urlService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public ManageController(IMapper mapper
                          , IEmailSender emailSender
                          , IUrlService urlService
                          , ILogger<ManageController> logger
                          , UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _logger = logger;
        _urlService = urlService;
        _emailSender = emailSender;
        _userManager = userManager;
    }

    [HttpGet] //, Route("GetUserProfile")]
    public async Task<IActionResult> GetUserProfileAsync()
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "GetUserProfileAsync");
                return NotFound("User not found");
            }

            return Ok(_mapper.Map<ApplicationUser, ManageProfileViewModel>(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, ex, "GetUserProfileAsync");
            return BadRequest("There was an error getting the user");
        }
    }

    [HttpPost, Route("Profile")]
    public async Task<IActionResult> UpdateProfileAsync(ManageProfileViewModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "GetUserProfileAsync");
                return NotFound("User not found");
            }

            var userName = user.UserName;
            if (model.UserName != userName)
            {
                if (await _userManager.FindByNameAsync(model.UserName) != null)
                {
                    ModelState.AddModelError("Username", $"Username '{model.UserName}' is already taken.");
                    return BadRequest(ModelState);
                }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    ModelState.AddModelError("Username", $"Unexpected error occurred setting username for user with ID '{user.Id}'.");
                    return BadRequest(ModelState);
                }

                // Save Avatar with new username ========> use user id instead 
                // 
                //var file = await _fileClient.GetFile(StorageContainers.Avatar, userName);
                //if (file != null)
                //{
                //    await _fileClient.SaveFile(StorageContainers.Avatar, model.UserName, file);
                //    await file.DisposeAsync();
                //    await _fileClient.DeleteFile(StorageContainers.Avatar, userName);
                //}

                //var avatarUrl = await _fileClient.GetFileUrl(StorageContainers.Avatar, model.UserName);

                //if (string.IsNullOrEmpty(avatarUrl))
                //{
                //    avatarUrl = "https://img.icons8.com/color/96/000000/user.png";
                //}

                //user.Avatar = avatarUrl;
            }

            var viewModel = new EmailConfirmationRequiredDto();

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    ModelState.AddModelError("Email", $"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                    return BadRequest(ModelState);
                }
                else
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = _urlService.GetConfirmEmailUrl(model.UserName, code);
                    var templateData = new { username = user.UserName, url = url };
                    await _emailSender.SendTemplateEmail("d-fc1571171e23463bb311870984664506", model.Email, templateData);
                    viewModel.IsEmailConfirmationRequired = true;
                }
            }

            var update = await _userManager.UpdateAsync(user);

            if (!update.Succeeded)
                throw new ApplicationException($"Unexpected error occurred setting the location for user with ID '{user.Id}'.");

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "GetUserProfileAsync");
            return BadRequest("There was an error updating the user");
        }
    }

    // ToDo: MOVE TO SEPARATE CONTROLLER (or handle in client side?)

    //[HttpPost, Route("UploadAvatar")]
    //public async Task<IActionResult> PostAvatarAsync([FromForm(Name = "file")] IFormFile file)
    //{
    //    try
    //    {
    //        if (file == null)
    //        {
    //            _logger.LogError(LoggingEvents.UpdateItem, "IFormFile argument is null at PostAvatar()");
    //            return BadRequest("An error occurred");
    //        }

    //        string[] supportedTypes = new[] { "jpg", "jpeg", "png", "bmp" };
    //        var fileExt = Path.GetExtension(file.FileName).Substring(1);
    //        if (!supportedTypes.Contains(fileExt))
    //        {
    //            string message = $"IFormFile is not a supported image type. Type: {fileExt}";
    //            _logger.LogError(LoggingEvents.UpdateItem, message);
    //            return BadRequest(message);
    //        }

    //        var user = await _userManager.FindByNameAsync(User.Identity.Name);
    //        if (user == null)
    //        {
    //            _logger.LogError(LoggingEvents.GetItemNotFound, $"User with id '{User.Identity.Name}' not found");
    //            return NotFound("User not found");
    //        }

    //        await _fileClient.DeleteFile(StorageContainers.Avatar, user.UserName);

    //        using (var fileStream = file.OpenReadStream())
    //        {
    //            await _fileClient.SaveFile(StorageContainers.Avatar, user.UserName, fileStream);
    //        }

    //        var avatarUrl = await _fileClient.GetFileUrl(StorageContainers.Avatar, user.UserName);

    //        if (string.IsNullOrEmpty(avatarUrl))
    //        {
    //            avatarUrl = "https://img.icons8.com/color/96/000000/user.png";
    //        }

    //        if (user.Avatar != avatarUrl)
    //        {
    //            user.Avatar = avatarUrl;
    //            var setAvatar = await _userManager.UpdateAsync(user);
    //            if (!setAvatar.Succeeded)
    //                throw new ApplicationException($"Unexpected error occurred setting the avatar for user with ID '{user.Id}'.");
    //        }

    //        return Ok();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "PostAvatar()");
    //        return BadRequest("An unexpected error occurred");
    //    }
    //}

    [HttpPost, Route("Location")]
    public async Task<IActionResult> SetLocationAsync(SetLocationViewModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "SetLocationAsync");
                return NotFound("User not found");
            }

            var coordinates = string.Concat(user.DefaultLocationLatitude, ",", user.DefaultLocationLongitude);

            if (string.Concat(model.DefaultLocationLatitude, ",", model.DefaultLocationLongitude) != coordinates)
            {
                user.DefaultLocationLatitude = model.DefaultLocationLatitude;
                user.DefaultLocationLongitude = model.DefaultLocationLongitude;

                var setCoordinates = await _userManager.UpdateAsync(user);
                if (!setCoordinates.Succeeded)
                    throw new ApplicationException($"Unexpected error occurred setting the location for user with ID '{user.Id}'.");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "SetLocation");
            return BadRequest("There was an error updating the user");
        }
    }

    [HttpPost, Route("Password")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "ChangePassword");
                return BadRequest("There was an error updating the user");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
                throw new ApplicationException($"Unexpected error occurred changing the password for user with ID '{user.Id}'.");

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "ChangePassword");
            return BadRequest("There was an error updating the user");
        }
    }
}

public class EmailConfirmationRequiredDto
{
    public bool IsEmailConfirmationRequired { get; set; }
}