namespace Birder.Tests.Controller;

public class LifeListTests
{
    [Fact]
    public async Task Returns_Ok_With_Viewmodel()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetLifeListAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                .ReturnsAsync(new List<LifeListViewModel>());

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetLifeListAsync();

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var actual = Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(objectResult.Value);
    }

    [Fact]
    public async Task Returns_500_When_Exception_Is_Raised()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetLifeListAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetLifeListAsync();

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
        var _systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
        };

        // Act
        var result = await controller.GetLifeListAsync();

        // Assert
        var objectResult = Assert.IsType<BadRequestResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task Returns_500_When_Repository_Returns_Null()
    {
        // Arrange
        Mock<ILogger<ListController>> loggerMock = new();
        var _systemClock = new Mock<ISystemClockService>();
        var mockListService = new Mock<IListService>();
        mockListService.Setup(obs => obs.GetLifeListAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                .Returns(Task.FromResult<IEnumerable<LifeListViewModel>>(null));

        var controller = new ListController(loggerMock.Object, _systemClock.Object, mockListService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.GetLifeListAsync();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("an unexpected error occurred", actual);
    }
}