using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class GetSearchNetworkAsyncTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<NetworkController>> _logger;

    public GetSearchNetworkAsyncTests()
    {
        var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BirderMappingProfile());
        });
        _mapper = mappingConfig.CreateMapper();
        _logger = new Mock<ILogger<NetworkController>>();
    }

    [Fact]
    public async Task GetSearchNetworkAsync_Returns_500_On_Internal_Error()
    {
        // Arrange
        string requesterUsername = "username";
        string searchCriterion = "testUser2";

        UserManager<ApplicationUser> userManager = null; //to cause internal error
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetSearchNetworkAsync(searchCriterion);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"an unexpected error occurred", objectResult.Value);
    }

    [Fact]
    public async Task GetSearchNetworkAsync_ReturnsOkWithNetworkListViewModelCollection_WhenSuccessful()
    {
        // Arrange
        string searchCriterion = "testUser2";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepo = new Mock<INetworkRepository>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("testUser1") }
        };

        // Act
        var result = await controller.GetSearchNetworkAsync(searchCriterion);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<OkObjectResult>(result);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetSearchNetworkAsync_ReturnsBadRequestWithstringObject_WhenStringArgumentIsNullOrEmpty(string searchCriterion)
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepo = new Mock<INetworkRepository>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
        };

        // Act
        var result = await controller.GetSearchNetworkAsync(searchCriterion);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.True(objectResult is BadRequestObjectResult);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.IsType<string>(objectResult.Value);
        Assert.Equal("No search criterion", objectResult.Value);
    }

    [Fact]
    public async Task GetSearchNetworkAsync_ReturnsNotFoundWithstringObject_WhenRepositoryReturnsNullUser()
    {
        // Arrange
        string searchCriterion = "Test string";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepo = new Mock<INetworkRepository>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
        };

        // Act
        var result = await controller.GetSearchNetworkAsync(searchCriterion);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<string>(objectResult.Value);
        Assert.Equal("requesting user not found", objectResult.Value);
    }
}