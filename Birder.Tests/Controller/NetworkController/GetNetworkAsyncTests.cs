﻿using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class GetNetworkAsyncTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<NetworkController>> _logger;

    public GetNetworkAsyncTests()
    {
        // private readonly ILoggerFactory _loggerFactory;
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
        _logger = new Mock<ILogger<NetworkController>>();
    }

    [Fact]
    public async Task GetNetworkSummaryAsync_Returns_500_On_Internal_Error()
    {
        // Arrange
        string requesterUsername = "testUser1";
        var mockHelper = new Mock<IUserNetworkHelpers>();

        UserManager<ApplicationUser> userManager = null; //to cause internal error
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, new NullLogger<NetworkController>(), mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetNetworkSummaryAsync();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"an unexpected error occurred", objectResult.Value);
    }

    [Fact]
    public async Task GetNetworkSummaryAsync_Returns_500_When_User_Is_Null()
    {
        // Arrange
        string requesterUsername = "This requested user does not exist";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var mockHelper = new Mock<IUserNetworkHelpers>();

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetNetworkSummaryAsync();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"requesting user not found", objectResult.Value);
    }


    [Fact]
    public async Task GetNetworkSummaryAsync_Returns_Ok()
    {
        // Arrange
        string requesterUsername = "testUser1";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var mockHelper = new Mock<IUserNetworkHelpers>();

        var userManager = SharedFunctions.InitialiseUserManager(context);
        var mockRepo = new Mock<INetworkRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager, mockHelper.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        };

        // Act
        var result = await controller.GetNetworkSummaryAsync();

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result); ;
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<NetworkSummaryDto>(objectResult.Value);
    }
}