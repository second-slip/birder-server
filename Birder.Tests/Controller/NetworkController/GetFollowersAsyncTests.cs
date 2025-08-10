using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class GetFollowersAsyncTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ManageController>> _logger;
    // private readonly ILoggerFactory _loggerFactory;

    public GetFollowersAsyncTests()
    {
                var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Add a console logger
        });
        var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new BirderMappingProfile()), loggerFactory);
        // var mappingConfig = new MapperConfiguration(cfg =>
        // {
        //     cfg.AddProfile(new BirderMappingProfile());
        // });
        _mapper = mappingConfig.CreateMapper();
    }

    [Fact]
    public async Task GetFollowersAsync_Returns_500_On_Internal_Error()
    {
        // Arrange
        string requesterUsername = "username";

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();

        UserManager<ApplicationUser> userManager = null; //to cause internal error

        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetFollowersAsync(requesterUsername);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        loggerMock.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),//It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetFollowersAsync_Returns_400_When_Argument_Is_Null_Or_Empty(string requesterUsername)
    {
        // Arrange

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();

        UserManager<ApplicationUser> userManager = null;

        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("") }
        };

        // Act
        var result = await controller.GetFollowersAsync(requesterUsername);

        // Assert
        var objectResult = Assert.IsType<BadRequestResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

        loggerMock.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => string.Equals("requestedUsername method argument is null or empty", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetFollowersAsync_When_Own_Profile_Requested_Returns_200()
    {
        // Arrange
        string requesterUsername = "username";
        string requestedUsername = "otherUsername";

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(requesterUsername));
        context.Users.Add(SharedFunctions.CreateUser(requestedUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetFollowersAsync(requesterUsername);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<List<FollowerViewModel>>(objectResult.Value);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);

        mockHelper.Verify(x => x.SetupFollowersCollection(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<FollowerViewModel>>()), Times.Once);
    }

    [Fact]
    public async Task GetFollowersAsync_When_Other_Profile_Requested_Returns_200()
    {
        // Arrange
        string requesterUsername = "username";
        string requestedUsername = "otherUsername";

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(requesterUsername));
        context.Users.Add(SharedFunctions.CreateUser(requestedUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetFollowersAsync(requestedUsername);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<List<FollowerViewModel>>(objectResult.Value);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);

        mockHelper.Verify(x => x.SetupFollowersCollection(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<FollowerViewModel>>()), Times.Once);
    }
}