using Birder.Controllers;
using Birder.Data.Model;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<ILogger<AuthenticationController>> _logger;
        private readonly Mock<IConfiguration> _config;
        private readonly ISystemClockService _systemClock;

        public AuthenticationControllerTests()
        {
            _logger = new Mock<ILogger<AuthenticationController>>();
            _systemClock = new SystemClockService();
            _config = new Mock<IConfiguration>();
            _config.SetupGet(x => x[It.Is<string>(s => s == "Tokens:Issuer")]).Returns("http://localhost:55722");
        }

        [Fact]
        public async Task Login_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var mockUserManager = initialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>())) // (It.IsAny<Expression<Func<Observation, bool>>>()))
                        .ReturnsAsync(GetTestUser());

            var mockSignInManager = initialiseMockSignInManager(mockUserManager);
            mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

             var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                            _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOkObjectResult_WithLoginDto()
        {
            // Arrange
            var mockUserManager = initialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(GetTestUser());

            var mockSignInManager = initialiseMockSignInManager(mockUserManager);
            mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                           _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<LoginDto> (objectResult.Value);
        }





        private Mock<UserManager<ApplicationUser>> initialiseMockUserManager()
        {
            return new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        }

        private Mock<SignInManager<ApplicationUser>> initialiseMockSignInManager(Mock<UserManager<ApplicationUser>> userManager)
        {
            return new Mock<SignInManager<ApplicationUser>>(
                        userManager.Object,
                        new Mock<IHttpContextAccessor>().Object,
                        new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                        new Mock<IOptions<IdentityOptions>>().Object,
                        new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                        new Mock<IAuthenticationSchemeProvider>().Object);
        }

        private ApplicationUser GetTestUser()
        {
            var user = new ApplicationUser()
            {
                Email = "a@b.com",
                EmailConfirmed = true,
                UserName = "Test",
                Avatar = ""
            };

            return user;
        }
    }
}
