namespace Birder.Tests.Controller;

public class TopObsListTests
{
    [Fact]
    public async Task Returns_Ok_With_Viewmodel()
    {
        // Arrange
        const string TEST_USERNAME = "non_null_or_empty_string";
        Mock<ILogger<ListController>> loggerMock = new();
        var systemClock = new Mock<ISystemClockService>();
        systemClock.SetupGet(x => x.GetToday).Returns(DateTime.Today);
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(TEST_USERNAME))
            .ReturnsAsync(new List<TopObservationsViewModel>());

        mockListService.Setup(obs => obs.GetTopObservationsAsync(TEST_USERNAME, It.IsAny<DateTime>()))
                .ReturnsAsync(new List<TopObservationsViewModel>());

        var controller = new ListController(loggerMock.Object, systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(TEST_USERNAME) }
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
        const string TEST_USERNAME = "non_null_or_empty_string";
        Mock<ILogger<ListController>> loggerMock = new();
        var systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(TEST_USERNAME, It.IsAny<DateTime>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ListController(loggerMock.Object, systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(TEST_USERNAME) }
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
        const string TEST_USERNAME = "non_null_or_empty_string";
        DateTime TEST_DATE = DateTime.Today;
        Mock<ILogger<ListController>> loggerMock = new();
        var systemClock = new Mock<ISystemClockService>();
        systemClock.SetupGet(x => x.GetToday).Returns(TEST_DATE);
            
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetTopObservationsAsync(TEST_USERNAME))
                .Returns(Task.FromResult<List<TopObservationsViewModel>>(null));
        mockListService.Setup(obs => obs.GetTopObservationsAsync(TEST_USERNAME, TEST_DATE))
                .Returns(Task.FromResult<List<TopObservationsViewModel>>(null));

        var controller = new ListController(loggerMock.Object, systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(TEST_USERNAME) }
        };

        // Act
        var result = await controller.GetTopObservationsListAsync();

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}