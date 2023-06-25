using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class PostUnfollowUserAsyncTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<NetworkController>> _logger;

    public PostUnfollowUserAsyncTests()
    {
        var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BirderMappingProfile());
        });
        _mapper = mappingConfig.CreateMapper();
        _logger = new Mock<ILogger<NetworkController>>();
    }

    #region Unfollow action tests

    [Fact]
    public async Task PostUnfollowUserAsync_ReturnsNotFound_WhenRequestingUserIsNullFromRepository()
    {
        // Arrange
        string requestingUser = "This requested user does not exist";
        string userToUnfollow = "This requested user does not exist";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHelper = new Mock<IUserNetworkHelpers>();

        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        };

        // Act
        var result = await controller.PostUnfollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToUnfollow));

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.True(objectResult is NotFoundObjectResult);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("Requesting user not found", actual);
    }

    [Fact]
    public async Task PostUnfollowUserAsync_ReturnsNotFound_WhenUserToFollowIsNullFromRepository()
    {
        // Arrange
        string requestingUser = "testUser1";
        string userToUnfollow = "This requested user does not exist";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        };

        // Act
        var result = await controller.PostUnfollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToUnfollow));

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.True(objectResult is NotFoundObjectResult);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("User to Unfollow not found", actual);
    }

    [Fact]
    public async Task PostUnfollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
    {
        // Arrange
        string requestingUser = "testUser1";
        string userToUnfollow = requestingUser;

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        };

        // Act
        var result = await controller.PostUnfollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToUnfollow));

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.True(objectResult is BadRequestObjectResult);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("Trying to unfollow yourself", actual);
    }


    [Fact]
    public async Task PostUnfollowUserAsync_Returns_500_On_Internal_Error()
    {
        // Arrange
        string requestingUser = "testUser1";
        string userToUnfollow = "testUser2";

        UserManager<ApplicationUser> userManager = null; //to cause internal error
        var mockRepo = new Mock<INetworkRepository>();
        mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
            .Verifiable();

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHelper = new Mock<IUserNetworkHelpers>();
        mockUnitOfWork.Setup(x => x.CompleteAsync())
            .ThrowsAsync(new InvalidOperationException());

        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        };

        // Act
        var result = await controller.PostUnfollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToUnfollow));

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"an unexpected error occurred", objectResult.Value);
    }

    [Fact]
    public async Task PostUnfollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
    {
        // Arrange
        string requestingUser = "testUser1";
        string userToUnfollow = "testUser2";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockRepo = new Mock<INetworkRepository>();
        mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
            .Verifiable();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHelper = new Mock<IUserNetworkHelpers>();
        mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        };

        // Act
        var result = await controller.PostUnfollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToUnfollow));

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<OkObjectResult>(result);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<NetworkUserViewModel>(objectResult.Value);
        var model = objectResult.Value as NetworkUserViewModel;
        Assert.Equal(userToUnfollow, model.UserName);
    }

    #endregion
}