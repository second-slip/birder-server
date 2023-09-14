namespace Birder.Tests.Controller;

public class GetConfirmEmailAsync_Tests
{
    [Theory, MemberData(nameof(Null_Empty_Whitespace_String_Test_Data))]
    public async Task ConfirmEmailAsync_Returns_400_When_Username_Argument_Is_Not_Valid(string username)
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string VALID_STRING_ARG = "not-null-or-empty";
        // Act
        var result = await controller.GetConfirmEmailAsync(username, VALID_STRING_ARG);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Theory, MemberData(nameof(Null_Empty_Whitespace_String_Test_Data))]
    public async Task ConfirmEmailAsync_Returns_400_When_Code_Argument_Is_Not_Valid(string code)
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string VALID_STRING_ARG = "not-null-or-empty";

        // Act
        var result = await controller.GetConfirmEmailAsync(VALID_STRING_ARG, code);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task ConfirmEmailAsync_Returns_500_When_UserManager_Returns_Null()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<ApplicationUser>(null));
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string VALID_STRING_ARG = "not-null-or-empty";

        // Act
        var result = await controller.GetConfirmEmailAsync(VALID_STRING_ARG, VALID_STRING_ARG);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task ConfirmEmailAsync_Returns_500_On_Exception()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string VALID_STRING_ARG = "not-null-or-empty";

        // Act
        var result = await controller.GetConfirmEmailAsync(VALID_STRING_ARG, VALID_STRING_ARG);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task ConfirmEmailAsync_Returns_500_When_Confirm_Email_Async_Fails()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = true,
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);
        mockUserManager.Setup(repo => repo.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = string.Empty, Description = "Test IdentityError" })));
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string VALID_STRING_ARG = "not-null-or-empty";

        // Act
        var result = await controller.GetConfirmEmailAsync(VALID_STRING_ARG, VALID_STRING_ARG);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        // todo: test logger output
    }

    [Fact]
    public async Task ConfirmEmailAsync_Returns_500_On_Success()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = true,
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);
        mockUserManager.Setup(repo => repo.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string VALID_STRING_ARG = "not-null-or-empty";

        // Act
        var result = await controller.GetConfirmEmailAsync(VALID_STRING_ARG, VALID_STRING_ARG);

        // Assert
        var objectResult = Assert.IsType<RedirectResult>(result);
        Assert.NotNull(objectResult);
        Assert.True(objectResult is RedirectResult);

        var expected = "/confirmed-email";
        objectResult.Url.Should().BeOfType<string>();
        objectResult.Url.Should().BeEquivalentTo(expected);
        // todo: test logger output
    }


    public static IEnumerable<object[]> Null_Empty_Whitespace_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { null },
                new object[] { "" },
                new object[] { string.Empty },
                new object[] { "       " },
                new object[] { " " },
                new object[] { "    " }
            };
        }
    }

}