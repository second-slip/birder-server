using Microsoft.AspNetCore.Http;

namespace Birder.Tests.Controller;

public class Request_Public_Records
{
    private readonly Mock<ILogger<ObservationFeedController>> _logger;
    private readonly Mock<IObservationQueryService> _mockService;

    public Request_Public_Records()
    {
        _logger = new Mock<ILogger<ObservationFeedController>>();
        _mockService = new Mock<IObservationQueryService>();
    }

    [Fact]
    public async Task Returns_OkResult_With_Public_Records()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationQueryService>();
        var model = new List<ObservationFeedDto>() { new ObservationFeedDto() };

        mockObsRepo.SetupSequence(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(model);

        var controller = new ObservationFeedController(_logger.Object, mockUserManager.Object, mockObsRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
        };

        // Act
        var result = await controller.GetPublicFeedAsync(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsAssignableFrom<List<ObservationFeedDto>>(objectResult.Value);
    }

    [Fact]
    public async Task Returns_500_When_Repository_Returns_Null()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationQueryService>();
        mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<IEnumerable<ObservationFeedDto>>(null));

        var controller = new ObservationFeedController(_logger.Object, mockUserManager.Object, mockObsRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
        };

        // Act
        var result = await controller.GetPublicFeedAsync(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("an unexpected error occurred", actual);
    }

    [Fact]
    public async Task Returns_500_When_Exception_Is_Raised()
    {
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationQueryService>();
        mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationFeedController(_logger.Object, mockUserManager.Object, mockObsRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
        };

        // Act
        var result = await controller.GetPublicFeedAsync(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal($"an unexpected error occurred", actual);
    }
}