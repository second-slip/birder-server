namespace Birder.Tests.Controller;

public class GetObservationUpdateDtoAsync_Tests
{
    [Fact]
    public async Task GetObservationUpdateDtoAsync_On_Success_Returns_200()
    {
        // Arrange
        const int OBSERVATION_ID = 1;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = new ObservationUpdateDto();
        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetObservationUpdateModelAsync(OBSERVATION_ID))
                .ReturnsAsync(expectedResponseObject);

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationUpdateDtoAsync(OBSERVATION_ID);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var actual = Assert.IsType<ObservationUpdateDto>(objectResult.Value);

        mockService.Verify(x => x.GetObservationUpdateModelAsync(It.IsAny<int>()), Times.Once);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task GetObservationUpdateDtoAsync_On_Exception_Returns_500()
    {
        // Arrange
        const int OBSERVATION_ID = 1;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetObservationUpdateModelAsync(OBSERVATION_ID))
                   .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationUpdateDtoAsync(OBSERVATION_ID);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockService.Verify(x => x.GetObservationUpdateModelAsync(It.IsAny<int>()), Times.Once);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => true),//It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }

    [Fact]
    public async Task GetObservationUpdateDtoAsync_When_Object_Is_NULL_Returns_500()
    {
        // Arrange
        const int OBSERVATION_ID = 1;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetObservationUpdateModelAsync(OBSERVATION_ID))
                   .Returns(Task.FromResult<ObservationUpdateDto>(null));

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationUpdateDtoAsync(OBSERVATION_ID);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockService.Verify(x => x.GetObservationUpdateModelAsync(It.IsAny<int>()), Times.Once);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Warning),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => true),//It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }

    [Fact]
    public async Task GetObservationUpdateDtoAsync_When_Id_Is_Zero_Returns_400()
    {
        // Arrange
        const int OBSERVATION_ID_IS_ZERO = 0;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetObservationUpdateModelAsync(OBSERVATION_ID_IS_ZERO))
                   .Returns(Task.FromResult<ObservationUpdateDto>(null));

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationUpdateDtoAsync(OBSERVATION_ID_IS_ZERO);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

        mockService.Verify(x => x.GetObservationUpdateModelAsync(It.IsAny<int>()), Times.Never);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((o, t) => string.Equals("id argument is 0", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }
}