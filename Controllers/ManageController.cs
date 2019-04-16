using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ManageController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISystemClock _systemClock;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageController(
                                IMapper mapper,
                                ISystemClock systemClock,
                                UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            //_context = context;
            _systemClock = systemClock;
            _userManager = userManager;
        }

        [HttpGet, Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var viewModel = _mapper.Map<ApplicationUser, ManageProfileViewModel>(user);

            return Ok(viewModel);

        }

        [HttpPost, Route("UpdateProfile")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ManageProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("GetUser", $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return BadRequest(ModelState);
            }

            var userName = user.UserName;
            if (model.UserName != userName)
            {
                if (await _userManager.FindByNameAsync(model.UserName) != null)
                {
                    // Can use this in check username...
                    ModelState.AddModelError("Username", $"Username '{model.UserName}' is already taken.");

                    return BadRequest(ModelState);
                }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    //throw new ApplicationException($"Unexpected error occurred setting username for user with ID '{user.Id}'.");
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
            }

            //if (model.ProfileImage != null)
            //{
            //    try
            //    {
            //        string filepath = string.Concat(user.UserName, Path.GetExtension(model.ProfileImage.FileName.ToString()));
            //        var imageArray = await _stream.GetByteArray(model.ProfileImage);
            //        imageArray = _stream.ResizePhoto(imageArray, 64, 64);
            //        var imageUpload = _imageService.StoreProfileImage(filepath, imageArray, "profile");

            //        imageUpload.Wait();
            //        if (imageUpload.IsCompletedSuccessfully == true)
            //        {
            //            user.ProfileImage = imageUpload.Result;
            //        }
            //    }
            //    catch
            //    {
            //        ModelState.AddModelError("ProfileImage", $"Unexpected error occurred processing the profile photo for user with ID '{user.Id}'.");
                    //return BadRequest(ModelState);
            //    }
            //}

            await _userManager.UpdateAsync(user);

            return Ok(model);

            //StatusMessage = "Your profile has been updated";
            //return RedirectToAction(nameof(Index));
        }

    }
}