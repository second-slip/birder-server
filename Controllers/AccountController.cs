using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // GET api/values
        [HttpPost, Route("login")] //[HttpPost("[action]")]
        public IActionResult Login([FromBody]LoginViewModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.UserName == "a@b.com" && user.Password == "test")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:53468",
                    audience: "http://localhost:53468",
                    claims: claims,  // new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                
                var uvm = new UserViewModel();
                uvm.UserName = "Andrew Cross";
                uvm.Token = tokenString;

                return Ok(new { Token = tokenString });
                //return Ok(uvm);
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }




    //Too: Move to separate file
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}