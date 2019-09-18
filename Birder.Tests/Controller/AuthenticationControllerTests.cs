using AutoMapper.Configuration;
using Birder.Data.Model;
using Birder.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Birder.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IConfiguration> _config;
        private readonly ISystemClockService _systemClock;


        public AuthenticationControllerTests()
        {

        }
    }
}
