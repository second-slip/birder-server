using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class GetNetworkSuggestionsAsyncTests
{
    private readonly IMapper _mapper;

    public GetNetworkSuggestionsAsyncTests()
    {
        var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BirderMappingProfile());
        });
        _mapper = mappingConfig.CreateMapper();
    }

    [Fact]
    public async Task GetNetworkSuggestionsAsync_Returns_500_On_Internal_Error()
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
        var result = await controller.GetNetworkSuggestionsAsync();

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

    [Fact]
    public async Task GetNetworkSuggestionsAsync_Returns_500_When_User_Is_Null()
    {
        // Arrange
        string requesterUsername = "username";

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetNetworkSuggestionsAsync();

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        loggerMock.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => string.Equals("UserManager returned null", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }




    [Fact]
    public async Task GetNetworkSuggestionsAsync_When_Followers_Not_Followed_returns_200()
    {
        // Arrange
        string requesterUsername = "username";

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();
        mockHelper.Setup(i => i.GetFollowersNotBeingFollowedUserNames(It.IsAny<ApplicationUser>()))
            .Returns(new List<string>());
        mockHelper.Setup(i => i.GetFollowingUserNames(It.IsAny<ICollection<Network>>()))
            .Returns(new List<string>());


        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(requesterUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);


        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetNetworkSuggestionsAsync();

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);

        mockHelper.Verify(x => x.GetFollowersNotBeingFollowedUserNames(It.IsAny<ApplicationUser>()), Times.Once);
        mockHelper.Verify(x => x.GetFollowingUserNames(It.IsAny<ICollection<Network>>()), Times.Once);
    }

    [Fact]
    public async Task GetNetworkSuggestionsAsync_When_All_Followers_Followed_returns_200()
    {
        // Arrange
        string requesterUsername = "username";

        Mock<ILogger<NetworkController>> loggerMock = new();

        var mockHelper = new Mock<IUserNetworkHelpers>();
        mockHelper.Setup(i => i.GetFollowersNotBeingFollowedUserNames(It.IsAny<ApplicationUser>()))
            .Returns(new List<string> { "test", "test2" });

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(requesterUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, loggerMock.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetNetworkSuggestionsAsync();

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);

        mockHelper.Verify(x => x.GetFollowersNotBeingFollowedUserNames(It.IsAny<ApplicationUser>()), Times.Once);
        mockHelper.Verify(x => x.GetFollowingUserNames(It.IsAny<ICollection<Network>>()), Times.Never);
    }
}