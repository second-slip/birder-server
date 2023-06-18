namespace Birder.Tests.Controller;

public class GetObservationCountAsyncTests
{
    [Fact]
    public async Task GetObservationCountAsync_On_Success_Returns_200()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();

        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .ReturnsAsync(new ObservationAnalysisViewModel { TotalObservationsCount = 2, UniqueSpeciesCount = 2 });

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationCountAsync();

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var actualObs = Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
        Assert.Equal(2, actualObs.TotalObservationsCount);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }


    [Fact]
    public async Task GetObservationCountAsync_On_Exception_Returns_500()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();

        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                        .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationCountAsync();

        // Assert
        var expectedResponseObject = "an unexpected error occurred";
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedResponseObject, actual);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => true),//It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }

    [Fact]
    public async Task GetObservationCountAsync_When_Claim_IS_Null_Returns_500()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();

        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                        .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = null }
        };

        // Act
        var result = await controller.GetObservationCountAsync();

        // Assert
        var expectedResponseObject = "an unexpected error occurred";
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedResponseObject, actual);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((o, t) => string.Equals("requesting username is null or empty", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }

    [Fact]
    public async Task GetObservationCountAsync_When_Object_Is_NULL_Returns_500()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();

        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .Returns(Task.FromResult<ObservationAnalysisViewModel>(null));

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationCountAsync();

        // Assert
        var expectedResponseObject = "an unexpected error occurred";
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedResponseObject, actual);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Warning),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => true),//It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }
}