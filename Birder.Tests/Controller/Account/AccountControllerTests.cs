using Microsoft.Extensions.Options;

namespace Birder.Tests.Controller;

public class AccountControllerTests
{
    private readonly Mock<ILogger<AccountController>> _logger;
    private readonly Mock<ISystemClockService> _systemClock;
    private readonly Mock<IEmailSender> _emailSender;
    private readonly Mock<IUrlService> _urlService;

    IOptions<ConfigOptions> testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
    { BaseUrl = "http://localhost:55722", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf" });

    public AccountControllerTests()
    {
        _logger = new Mock<ILogger<AccountController>>();
        _urlService = new Mock<IUrlService>();
        _emailSender = new Mock<IEmailSender>();
        _systemClock = new Mock<ISystemClockService>();
    }

    #region PostRegisterAsync unit tests

    //[Fact]
    //public async Task PostRegisterAsync_ReturnsBadRequestWithModelStateError_WhenModelStateIsInvalid()
    //{
    //    // Arrange
    //    var mockUserManager = SharedFunctions.InitialiseMockUserManager();

    //    var testModel = new RegisterViewModel() { };

    //    var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

    //    controller.ModelState.AddModelError("Test", "This is a test model error");

    //    // Act
    //    var result = await controller.PostRegisterAsync(testModel);

    //    // Assert
    //    var objectResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.NotNull(objectResult);
    //    Assert.True(objectResult is BadRequestObjectResult);
    //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

    //    var expected = new SerializableError
    //        {
    //            { "Test", new[] {"This is a test model error"}},
    //        };

    //    objectResult.Value.Should().BeOfType<SerializableError>();
    //    objectResult.Value.Should().BeEquivalentTo(expected);
    //}

    [Fact]
    public async Task PostRegisterAsync_ReturnsBadRequest_WhenExceptionRaised()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var testModel = new RegisterViewModel();

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.PostRegisterAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task PostRegisterAsync_ReturnsBadRequestWithModelState_WhenCreateUserIsUnsuccessful()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = string.Empty, Description = "Test IdentityError" })));

        var testModel = new RegisterViewModel() { };

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.PostRegisterAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

        var expected = new SerializableError
                {
                    { string.Empty, new[] {"Test IdentityError"}},
                };

        objectResult.Value.Should().BeOfType<SerializableError>();
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task PostRegisterAsync_ReturnsOk_WhenCreateUserIsSuccessful()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

        var testModel = new RegisterViewModel() { };

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.PostRegisterAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        //var expected = "New user successfully created";
        //objectResult.Value.Should().BeOfType<string>();
        //objectResult.Value.Should().BeEquivalentTo(expected);
    }

    #endregion



    #region ForgotPasswordAsync unit tests

    //[Fact]
    //public async Task PostForgotPasswordAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
    //{
    //    // Arrange
    //    var mockUserManager = SharedFunctions.InitialiseMockUserManager();

    //    var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

    //    var testModel = new UserEmailDto() { };

    //    controller.ModelState.AddModelError("Test", "This is a test model error");

    //    // Act
    //    var result = await controller.PostForgotPasswordAsync(testModel);

    //    // Assert
    //    var objectResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.NotNull(objectResult);
    //    Assert.True(objectResult is BadRequestObjectResult);
    //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

    //    var expected = "An error occurred";

    //    objectResult.Value.Should().BeOfType<string>();
    //    objectResult.Value.Should().BeEquivalentTo(expected);
    //}

    [Fact]
    public async Task PostForgotPasswordAsync_ReturnsBadRequest_WhenExceptionIsRaised()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        var expected = "An error occurred";
        objectResult.Value.Should().BeOfType<string>();
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task PostForgotPasswordAsync_ReturnsOkResult_WhenRepositoryReturnsNull()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<ApplicationUser>(null));

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var expected = "An error occurred";
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task PostForgotPasswordAsync_ReturnsOkResult_WhenUserEmailIsNotConfirmed()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(GetValidTestUser(false));

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var expected = "An error occurred";
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task PostForgotPasswordAsync_ReturnsOkObjectResult_WhenResetRequestIsHandledSuccessfully()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var testUser = GetValidTestUser(true);
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(testUser);
        var testCode = "myTestCode";
        mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(testCode);




        var urlService = new UrlService(testOptions);

        var controller = new AccountController(_systemClock.Object, urlService, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new UserEmailDto() { Email = "TestEmail" };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(testModel);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        //Assert.NotNull(objectResult);
        //Assert.True(objectResult is OkObjectResult);
        //Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        //var expected = new Uri($"{config["Url:ResetPasswordUrl"]}{testCode}");
        //objectResult.Value.Should().BeOfType<Uri>();
        //objectResult.Value.Should().BeEquivalentTo(expected);
    }

    #endregion


    #region PostResetPasswordAsync unit tests

    //[Fact]
    //public async Task PostResetPasswordAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
    //{
    //    // Arrange
    //    var mockUserManager = SharedFunctions.InitialiseMockUserManager();

    //    var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

    //    var testModel = new ResetPasswordViewModel() { };

    //    controller.ModelState.AddModelError("Test", "This is a test model error");

    //    // Act
    //    var result = await controller.PostResetPasswordAsync(testModel);

    //    // Assert
    //    var objectResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.NotNull(objectResult);
    //    Assert.True(objectResult is BadRequestObjectResult);
    //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

    //    var expected = new SerializableError
    //    {
    //        { "Test", new[] {"This is a test model error"}},
    //    };

    //    objectResult.Value.Should().BeOfType<SerializableError>();
    //    objectResult.Value.Should().BeEquivalentTo(expected);
    //}

    [Fact]
    public async Task PostResetPasswordAsync_ReturnsBadRequest_WhenExceptionIsRaised()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        // Assert.NotNull(objectResult);
        // Assert.True(objectResult is BadRequestObjectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // var expected = "An error occurred";
        // objectResult.Value.Should().BeOfType<string>();
        // objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task PostResetPasswordAsync_ReturnsOKRequest_WhenRepositoryReturnsNull()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<ApplicationUser>(null));

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
    }

    [Fact]
    public async Task PostResetPasswordAsync_ReturnsBadRequest_WhenResetPasswordFails()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(GetValidTestUser(true));
        mockUserManager.Setup(repo => repo.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed()));

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.NotNull(objectResult);

        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task PostResetPasswordAsync_ReturnsOKObjectResult_WhenResetPasswordIsSuccessful()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(GetValidTestUser(true));
        mockUserManager.Setup(repo => repo.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

        var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

        var testModel = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(testModel);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        //Assert.NotNull(objectResult);
        //Assert.True(objectResult is OkObjectResult);
        //Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        //objectResult.Value.Should().BeOfType<string>();
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    #endregion



    private static ApplicationUser GetValidTestUser(bool emailConfirmed)
    {
        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = emailConfirmed,
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };

        return user;
    }
}