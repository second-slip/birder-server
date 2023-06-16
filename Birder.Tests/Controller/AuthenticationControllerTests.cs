namespace Birder.Tests.Controller;

public class AuthenticationControllerTests
{
    public AuthenticationControllerTests() { }

    [Fact]
    public async Task Returns_OkObjectResult_With_Dto()
    {
        // Arrange
        Mock<ILogger<AuthenticationController>> loggerMock = new();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync(GetValidTestUser());

        var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
        mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                        .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

        var mockAuthenticationTokenService = new Mock<IAuthenticationTokenService>();
        var expected = "test token";
        // new AuthenticationResultDto()
        // {
        //     AuthenticationToken = "test token",
        //     FailureReason = AuthenticationFailureReason.None
        // };
        mockAuthenticationTokenService.Setup(x => x.CreateToken(It.IsAny<ApplicationUser>()))
        .Returns(expected);

        var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, loggerMock.Object, mockAuthenticationTokenService.Object);

        var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

        // Act
        var result = await controller.Login(model);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<AuthenticationResultDto>(objectResult.Value);
        var returnModel = objectResult.Value as AuthenticationResultDto;
        Assert.Equal(AuthenticationFailureReason.None, returnModel.FailureReason);
        Assert.Equal(returnModel.AuthenticationToken, expected);
        Assert.NotNull(returnModel.AuthenticationToken);
    }

    [Fact]
    public async Task Returns_500_And_Other_Status_When_Login_Is_Unsuccessful_For_Any_Other_Reason()
    {
        // Arrange
        Mock<ILogger<AuthenticationController>> loggerMock = new();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync(GetValidTestUser());

        var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
        mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                        .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

        var mockAuthenticationTokenService = new Mock<IAuthenticationTokenService>();

        var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, loggerMock.Object, mockAuthenticationTokenService.Object);

        var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

        // Act
        var result = await controller.Login(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is ObjectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<AuthenticationResultDto>(objectResult.Value);

        var returnModel = objectResult.Value as AuthenticationResultDto;
        Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
        Assert.Null(returnModel.AuthenticationToken);
    }

    [Fact]
    public async Task Returns_500_And_LockedOut_Status_When_User_Is_Locked_Out()
    {
        // Arrange
        Mock<ILogger<AuthenticationController>> loggerMock = new();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync(GetValidTestUser());

        var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
        mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                        .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.LockedOut));

        var mockAuthenticationTokenService = new Mock<IAuthenticationTokenService>();

        var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, loggerMock.Object, mockAuthenticationTokenService.Object);

        var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

        // Act
        var result = await controller.Login(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is ObjectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<AuthenticationResultDto>(objectResult.Value);

        var returnModel = objectResult.Value as AuthenticationResultDto;
        Assert.Equal(AuthenticationFailureReason.LockedOut, returnModel.FailureReason);
        Assert.Null(returnModel.AuthenticationToken);
    }

    [Fact]
    public async Task Returns_500_And_EmailConfirmationRequired_Status_When_User_Email_Not_Confirmed()
    {
        // Arrange
        Mock<ILogger<AuthenticationController>> loggerMock = new();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(GetTestUserWithEmailNotConfirmed());

        var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);

        var mockAuthenticationTokenService = new Mock<IAuthenticationTokenService>();

        var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, loggerMock.Object, mockAuthenticationTokenService.Object);

        var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

        // Act
        var result = await controller.Login(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is ObjectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<AuthenticationResultDto>(objectResult.Value);

        var returnModel = objectResult.Value as AuthenticationResultDto;
        Assert.Equal(AuthenticationFailureReason.EmailConfirmationRequired, returnModel.FailureReason);
        Assert.Null(returnModel.AuthenticationToken);
    }

    [Fact]
    public async Task Returns_500_And_Other_Status_When_User_Is_Null()
    {
        // Arrange
        Mock<ILogger<AuthenticationController>> loggerMock = new();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                        .Returns(Task.FromResult<ApplicationUser>(null));

        var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);

        var mockAuthenticationTokenService = new Mock<IAuthenticationTokenService>();

        var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, loggerMock.Object, mockAuthenticationTokenService.Object);

        var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

        // Act
        var result = await controller.Login(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is ObjectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<AuthenticationResultDto>(objectResult.Value);

        var expectedToken = objectResult.Value as AuthenticationResultDto;
        Assert.Equal(AuthenticationFailureReason.Other, expectedToken.FailureReason);
        Assert.Null(expectedToken.AuthenticationToken);
    }

    [Fact]
    public async Task Returns_500_And_Logs_Error_When_InvalidOperationException_Is_Raised()
    {
        // Arrange
        Mock<ILogger<AuthenticationController>> loggerMock = new();

        var expectedExceptionMessage = "InvalidOperationException thrown";
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException(expectedExceptionMessage));
        var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
        var mockAuthenticationTokenService = new Mock<IAuthenticationTokenService>();
        var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, loggerMock.Object, mockAuthenticationTokenService.Object);

        // Act
        var result = await controller.Login(new LoginViewModel() { UserName = "", Password = "", RememberMe = false });

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.NotNull(objectResult);
        Assert.True(objectResult is ObjectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<AuthenticationResultDto>(objectResult.Value);

        var returnModel = Assert.IsType<AuthenticationResultDto>(objectResult.Value); //objectResult.Value as AuthenticationResultDto;
        Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
        Assert.Null(returnModel.AuthenticationToken);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<InvalidOperationException>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }




    #region Mock methods



    private ApplicationUser GetTestUserWithEmailNotConfirmed()
    {
        var user = new ApplicationUser()
        {
            EmailConfirmed = false,
        };

        return user;
    }

    private ApplicationUser GetValidTestUser()
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

    #endregion
}
