using AutoMapper.Configuration;
using Birder.Data.Model;
using Birder.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Birder.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly ISystemClockService _systemClock;
        private readonly IConfiguration _config;


        public AuthenticationControllerTests()
        {

        }
    }
}
