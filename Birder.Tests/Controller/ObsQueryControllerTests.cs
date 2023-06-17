namespace Birder.Tests.Controller;

public class ObsQueryControllerTests
{

    [Fact]
    public async Task GetObservationsByBirdSpeciesAsync_On_Success_Returns_200()
    {
        // Arrange
        Mock<ILogger<ObservationQueryController>> loggerMock = new();

        var expectedResponseObject = new ObservationsPagedDto();
        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedResponseObject);

        var controller = new ObservationQueryController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var actual = Assert.IsType<ObservationsPagedDto>(objectResult.Value);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task GetObservationsByBirdSpeciesAsync_On_Exception_Returns_500()
    {
        // Arrange
        Mock<ILogger<ObservationQueryController>> loggerMock = new();

        var expectedResponseObject = "an unexpected error occurred";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationQueryController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
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
    public async Task GetObservationsByBirdSpeciesAsync_When_Object_Is_NULL_Returns_500()
    {
        // Arrange
        Mock<ILogger<ObservationQueryController>> loggerMock = new();

        var expectedResponseObject = "an unexpected error occurred";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<ObservationsPagedDto>(null));

        var controller = new ObservationQueryController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
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