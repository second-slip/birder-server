﻿using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class UserProfileControllerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<UserProfileController>> _logger;

    public UserProfileControllerTests()
    {
        _logger = new Mock<ILogger<UserProfileController>>();

        var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // Add a console logger
});
        var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new BirderMappingProfile()), loggerFactory);
        //new MapperConfiguration(cfg =>
        // {
        //     cfg.AddProfile(new BirderMappingProfile());
        // }, loggerFactory);
        _mapper = mappingConfig.CreateMapper();

        // var config = new MapperConfiguration(cfg => cfg.AddProfile(new BirderMappingProfile()), loggerFactory);
    }


    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Returns_Bad_Request_When_Argument_Is_NullOrEmpty(string requestedUsername)
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var mockService = new Mock<IObservationsAnalysisService>();

        var controller = new UserProfileController(_mapper, _logger.Object, mockUserManager.Object, mockService.Object, mockHelper.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("NonExistentUsername") }
        };

        // Act
        var result = await controller.GetUserProfileAsync(requestedUsername);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<string>(objectResult.Value);
        Assert.Equal("requestedUsername argument is null or empty", objectResult.Value);
    }

    [Fact]
    public async Task Returns_500_When_User_Is_Null()
    {
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));

        context.SaveChanges();

        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockService = new Mock<IObservationsAnalysisService>();
        var controller = new UserProfileController(_mapper, _logger.Object, userManager, mockService.Object, mockHelper.Object);

        string requestedUsername = "This requested user does not exist";

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestedUsername) }
        };

        // Act
        var result = await controller.GetUserProfileAsync(requestedUsername);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"userManager returned null", objectResult.Value);

    }

    [Fact]
    public async Task Returns_500_When_Exception_Is_Raised()
    {
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(obs => obs.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .ThrowsAsync(new InvalidOperationException());

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));

        context.SaveChanges();

        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);

        var controller = new UserProfileController(_mapper, _logger.Object, userManager, mockService.Object, mockHelper.Object);

        string requestedUsername = "testUser2";

        string requesterUsername = "testUser1";

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetUserProfileAsync(requestedUsername);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal($"an unexpected error occurred", actual);

    }

    [Fact]
    public async Task Returns_Ok_With_Own_Profile()
    {
        var mockService = new Mock<IObservationsAnalysisService>();
        var mockHelper = new Mock<IUserNetworkHelpers>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var controller = new UserProfileController(_mapper, _logger.Object, userManager, mockService.Object, mockHelper.Object);

        string requestedUsername = "testUser1";

        string requesterUsername = requestedUsername; // i.e. requesting OWN profile

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetUserProfileAsync(requestedUsername);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.IsType<OkObjectResult>(result);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<UserProfileViewModel>(objectResult.Value);

        var model = objectResult.Value as UserProfileViewModel;
        Assert.Equal(requestedUsername, model.User.UserName);
    }

    [Fact]
    public async Task Returns_Ok_With_Other_User_Profile()
    {
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var countModel = new ObservationAnalysisViewModel { TotalObservationsCount = 2, UniqueSpeciesCount = 2 };
        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(countModel);

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));

        context.SaveChanges();

        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var controller = new UserProfileController(_mapper, _logger.Object, userManager, mockService.Object, mockHelper.Object);

        string requestedUsername = "testUser2";

        string requesterUsername = "testUser1";

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetUserProfileAsync(requestedUsername);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<UserProfileViewModel>(objectResult.Value);

        var model = objectResult.Value as UserProfileViewModel;
        Assert.Equal(requestedUsername, model.User.UserName);
        Assert.Equal(countModel, model.ObservationCount);
    }
}