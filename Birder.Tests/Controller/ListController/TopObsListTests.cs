namespace Birder.Tests.Controller;

public class TopObsListTests
{
    [Fact]
    public async Task Returns_Ok_With_Viewmodel()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new Mock<ISystemClockService>();
        _systemClock.SetupGet(x => x.GetToday).Returns(DateTime.Today);
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<TopObservationsViewModel>());

        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<TopObservationsViewModel>());

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

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
        var _systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task Returns_500_When_Repository_Returns_Null()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<List<TopObservationsViewModel>>(null));
        mockListService.Setup(obs => obs.GetTopObservationsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<List<TopObservationsViewModel>>(null));

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}