namespace Birder.Tests.Controller;

public class GetIsUsernameTakenAsync
{
    [Theory, MemberData(nameof(InValid_Username_String_Test_Data))]
    public async Task GetIsUsernameAvailableAsync_ReturnsBadRequest_WhenUsernameArgumentIsNull(string username)
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsUsernameTakenAsync(username);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Theory, MemberData(nameof(Valid_Username_String_Test_Data))]
    public async Task GetIsUsernameAvailableAsync_ReturnsBadRequest_WhenExceptionIsRaised(string username)
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsUsernameTakenAsync(username);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory, MemberData(nameof(Valid_Username_String_Test_Data))]
    public async Task GetIsUsernameAvailableAsync_ReturnsBadRequest_WhenUsernameIsTaken(string username)
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
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

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsUsernameTakenAsync(username);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { usernameTaken = true };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Theory, MemberData(nameof(Valid_Username_String_Test_Data))]
    public async Task GetIsUsernameAvailableAsync_ReturnsOk_WhenUsernameIsAvailable(string username)
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<ApplicationUser>(null));

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsUsernameTakenAsync(username);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { usernameTaken = false };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }


    public static IEnumerable<object[]> InValid_Username_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { "" },
                new object[] { null },
                new object[] { "@b" },
                new object[] {  "1234" } ,
                new object[] { "12345 " },
                new object[] { "sdefd@b." },
                new object[] { "sdefd@b " },
                new object[] { "sdefdb " },
                new object[] { "rnt7fkjjtnrgntksd5rmpZSD" },
                new object[] { "@o.uk" },
                new object[] { "  andrew3" }, // leading whitepace
                new object[] { "andrew3 " }, // trailing whitespace
            };
        }
    }

    public static IEnumerable<object[]> Valid_Username_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { "andrew1" },
                new object[] { "andrew" },
                new object[] {  "jimmy77689rt" } ,
            };
        }
    }
}