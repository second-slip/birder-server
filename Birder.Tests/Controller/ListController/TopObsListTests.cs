namespace Birder.Tests.Controller;

public class TopObsListTests
{
    [Fact]
    public async Task Returns_Ok_With_Viewmodel()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new SystemClockService();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new TopObservationsAnalysisViewModel());

        var controller = new ListController(loggerMock.Object, _systemClock, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var actual = Assert.IsType<TopObservationsAnalysisViewModel>(objectResult.Value);
    }

    [Fact]
    public async Task Returns_500_When_Exception_Is_Raised()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new SystemClockService();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ListController(loggerMock.Object, _systemClock, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal($"an unexpected error occurred", actual);
    }

    [Fact]
    public async Task Returns_Unauthorised_When_Username_Is_Empty()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new SystemClockService();
        var mockListService = new Mock<IListService>();

        var controller = new ListController(loggerMock.Object, _systemClock, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<BadRequestResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task Returns_500_When_Repository_Returns_Null()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new SystemClockService();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<TopObservationsAnalysisViewModel>(null));

        var controller = new ListController(loggerMock.Object, _systemClock, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("an unexpected error occurred", actual);
    }
}