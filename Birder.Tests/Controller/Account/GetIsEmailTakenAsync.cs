namespace Birder.Tests.Controller;

public class GetIsEmailTakenAsync
{
    [Theory, MemberData(nameof(InValid_Email_String_Test_Data))]
    public async Task GetIsEmailTakenAsync_ReturnsBadRequest_When_Email_Argument_Is_Not_Valid(string email)
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsEmailTakenAsync(email);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Theory, MemberData(nameof(Valid_Email_String_Test_Data))]
    public async Task GetIsEmailTakenAsync_Returns_500_On_Exception(string email)
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsEmailTakenAsync(email);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory, MemberData(nameof(Valid_Email_String_Test_Data))]
    public async Task GetIsEmailTakenAsync_Returns_200_With_False_When_Email_Is_Available(string email)
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<ApplicationUser>(null));

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsEmailTakenAsync(email);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { emailTaken = false };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Theory, MemberData(nameof(Valid_Email_String_Test_Data))]
    public async Task GetIsEmailTakenAsync_Returns_200_With_True_When_Email_Is_Not_Available(string email)
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
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        // Act
        var result = await controller.GetIsEmailTakenAsync(email);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { emailTaken = true };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }



    public static IEnumerable<object[]> InValid_Email_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { "" },
                new object[] { null },
                new object[] { "@b" },
                new object[] {  "@" } ,
                new object[] { "sdefd@b. " },
                new object[] { "sdefd@b." },
                new object[] { "sdefd@b " },
                new object[] { "sdefdb " },
                new object[] { "@b.com" },
                new object[] { " @.co.uk" },
                new object[] { "  a@b.com" }, // leading whitepace
                new object[] { "a@b.com  " }, // trailing whitespace
            };
        }
    }

    public static IEnumerable<object[]> Valid_Email_String_Test_Data
    {
        get
        {
            return new[]
            {
                new object[] { "a@b.com" },
                new object[] { "fggre@b.co" },
                new object[] { "A@AND.CO.UK" },
                new object[] { "zsdffv@j.org.uk" },
                new object[] { "AESTDNdmrjcm@b.com" }
            };
        }
    }
}