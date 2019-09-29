using AutoMapper;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ManageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUrlService _urlService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        //***************************
        // ToDo: Add logging
        //***************************

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

        [HttpGet, Route("GetUserProfile")]
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
            catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "GetUserProfileAsync");
                return BadRequest("There was an error getting the user");
            }
        }

        [HttpPost, Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfileAsync(ManageProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest(ModelState);
                }

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
                }

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
                        var url = _urlService.ConfirmEmailUrl(model.UserName, code);
                        await _emailSender.SendEmailAsync(model.Email, "Confirm your email", "Please confirm your account by clicking <a href=\"" + url + "\">here</a>");
                    }
                }

                var update = await _userManager.UpdateAsync(user);
                
                if(!update.Succeeded)
                    throw new ApplicationException($"Unexpected error occurred setting the location for user with ID '{user.Id}'.");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "GetUserProfileAsync");
                return BadRequest("There was an error updating the user");
            }
        }

        [HttpPost, Route("SetAvatar")]
        //public async Task<IActionResult> SetAvatar(string urlOrFilepathOrWhatever)
        public IActionResult SetAvatar(string urlOrFilepathOrWhatever)
        {
            return Ok();
            //if (model.ProfileImage != null)
            //{
            //    try
            //    {
            //        string filepath = string.Concat(user.UserName, Path.GetExtension(model.Avatar.FileName.ToString()));
            //        var imageArray = await _stream.GetByteArray(model.Avatar);
            //        imageArray = _stream.ResizePhoto(imageArray, 64, 64);
            //        var imageUpload = _imageService.StoreProfileImage(filepath, imageArray, "profile");

            //        imageUpload.Wait();
            //        if (imageUpload.IsCompletedSuccessfully == true)
            //        {
            //            user.Avatar = imageUpload.Result;
            //        }
            //    }
            //    catch
            //    {
            //        ModelState.AddModelError("ProfileImage", $"Unexpected error occurred processing the profile photo for user with ID '{user.Id}'.");
            //return BadRequest(ModelState);
            //    }
        }

        [HttpPost, Route("SetLocation")]
        public async Task<IActionResult> SetLocationAsync(SetLocationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "SetLocationAsync");
                    return NotFound("User not found");
                }

                var coordinates = user.DefaultLocationLatitude + "," + user.DefaultLocationLongitude;
                var e = string.Concat(user.DefaultLocationLatitude, ",", user.DefaultLocationLongitude);
                ///////////////////////////////////////////////////////////
                var equal = string.Equals(coordinates, e);
                //////////////////////////////////////////////////////////////
                
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


        [HttpPost, Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok(model);
        }
    }
}