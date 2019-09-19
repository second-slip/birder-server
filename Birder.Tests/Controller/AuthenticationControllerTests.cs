using Birder.Controllers;
using Birder.Data.Model;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace Birder.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
        private readonly Mock<ILogger<AuthenticationController>> _logger;
        private readonly Mock<IConfiguration> _config;
        private readonly ISystemClockService _systemClock;


        public AuthenticationControllerTests()
        {
            _userManager = new Mock<UserManager<ApplicationUser>>(0); // userStore.Object);
            _signInManager = new Mock<SignInManager<ApplicationUser>>();
            _logger = new Mock<ILogger<AuthenticationController>>();
            _systemClock = new SystemClockService();
            _config = new Mock<IConfiguration>();
            _config.SetupGet(x => x[It.Is<string>(s => s == "ConnectionStrings:default")]).Returns("mock value");

        }

        [Fact]
        public async Task Login_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                                new Mock<IUserStore<ApplicationUser>>().Object,
                                new Mock<IOptions<IdentityOptions>>().Object,
                                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                                new IUserValidator<ApplicationUser>[0],
                                new IPasswordValidator<ApplicationUser>[0],
                                new Mock<ILookupNormalizer>().Object,
                                new Mock<IdentityErrorDescriber>().Object,
                                new Mock<IServiceProvider>().Object,
                                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                                mockUserManager.Object,
                                new Mock<IHttpContextAccessor>().Object,
                                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                                new Mock<IOptions<IdentityOptions>>().Object,
                                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                                new Mock<IAuthenticationSchemeProvider>().Object);
        
            //var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            //                        new Mock)

            //mockUserStore.Setup(x => x.FindByIdAsyncFindByIdAsync(userId))
            //    .ReturnsAsync(new ApplicationUser()
            //    {
            //        UserName = "test@email.com"
            //    });

            //mockUserRoleStore.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            //  .ReturnsAsync(roleName.Equals(SWEET_TOOTH_ROLENAME, StringComparison.OrdinalIgnoreCase));
            //_configuration = new Mock<IConfiguration>();
            //_config.SetupGet(x => x[It.Is<string>(s => s == "ConnectionStrings:default")]).Returns("mock value");

            // Act

            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //// Duplicate here any configuration sources you use.
            //configurationBuilder.AddJsonFile("AppSettings.json");
            //IConfiguration configuration = configurationBuilder.Build();

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                            _systemClock, _config.Object);


            // Act
            var result = await controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);



        }
    }
}
