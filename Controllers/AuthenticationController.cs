using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Birder.Data;
using Birder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IConfiguration _config;

        public AuthenticationController(UserManager<ApplicationUser> userManager
                                        ,SignInManager<ApplicationUser> signInManager)
                                        //,IConfiguration config)
        {
            //_context = context;
            //_config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Hello()
        {
            return Ok();
        }

        [HttpPost, Route("login")] //[HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // if (user == null)
            // {
            //    return BadRequest("Invalid client request");
            // }
            // .FindByNameAsync(loginViewModel.Username);

            var user = await _userManager.FindByEmailAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
                // check for lockout / requires two factor login
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                    //new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    //new Claim(JwtRegisteredClaimNames., "Administrator"),
                    //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:55722",
                        audience: "http://localhost:55722",
                        claims: claims,  // new List<Claim>(),
                        expires: DateTime.Now.AddDays(2),
                        signingCredentials: signinCredentials
                        );


                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new { Token = tokenString });
                }
            }
            return Unauthorized();
            // return BadRequest();
        }
    }





            //if (user.UserName == "a@b.com" && user.Password == "test")
            //{
            //    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            //    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                //var claims = new List<Claim>
                //{
                //    //new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                //    //new Claim(JwtRegisteredClaimNames., "Administrator"),
                //    //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),    
                //};

                //var tokeOptions = new JwtSecurityToken(
                //    issuer: "http://localhost:55722",
                //    audience: "http://localhost:55722",
                //    claims: claims,  // new List<Claim>(),
                //    expires: DateTime.Now.AddDays(2),
                //    signingCredentials: signinCredentials
                //);

                //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                //return Ok(new { Token = tokenString });
                ////return Ok(uvm);


    //Too: Move to separate file
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}