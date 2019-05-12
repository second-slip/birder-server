using Birder.Data.Model;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        private readonly ISystemClock _systemClock;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ISystemClock systemClock,
                                IEmailSender emailSender,
                                UserManager<ApplicationUser> userManager)
        {
            _systemClock = systemClock;
            _emailSender = emailSender;
            _userManager = userManager;
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

                    // //await _signInManager.SignInAsync(user, isPersistent: false);
                    // _logger.LogInformation("User created a new account with password.");
                    //return RedirectToLocal(returnUrl);
                    //return RedirectToPage("/ConfirmYourEmail");
                    // return RedirectToAction("Welcome","Home");

                }
                //AddErrors(result);
                // username or email already taken, add to modelstate errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                // If we got this far, something failed, redisplay form
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {

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
                //return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await _userManager.FindByNameAsync(username); // FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
                //throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Redirect("/confirmed-email");

            // return View(result.Succeeded ? "ConfirmEmail" : "Error");
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