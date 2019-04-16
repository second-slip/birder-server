using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ApplicationDbContext _context;
        private readonly ISystemClock _systemClock;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageController(ApplicationDbContext context,
                                ISystemClock systemClock,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _systemClock = systemClock;
            _userManager = userManager;
        }

        [HttpPost, Route("UpdateProfile")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ManageProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = user.UserName;
            if (model.Username != userName)
            {
                if (await _userManager.FindByNameAsync(model.Username) != null)
                {
                    ModelState.AddModelError("Username", $"Username '{model.Username}' is already taken.");

                    return View(model);
                }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Username);
                if (!setUserNameResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting username for user with ID '{user.Id}'.");
                }
            }


            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            //var phoneNumber = user.PhoneNumber;
            //if (model.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            //    }
            //}

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
            //        throw new ApplicationException($"Unexpected error occurred processing the profile photo for user with ID '{user.Id}'.");
            //    }
            //}

            await _userManager.UpdateAsync(user);

            //StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

    }
}