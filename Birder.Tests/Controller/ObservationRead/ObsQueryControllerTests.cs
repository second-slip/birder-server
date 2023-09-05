namespace Birder.Tests.Controller;

public class ObsQueryControllerTests
{
    [Fact]
    public async Task GetObservationsByBirdSpeciesAsync_On_Success_Returns_200()
    {
        // Arrange
        const int BIRD_ID = 1001;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = new ObservationsPagedDto();
        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedResponseObject);

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(BIRD_ID, It.IsAny<int>(), It.IsAny<int>());

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
        const int BIRD_ID = 1001;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = "an unexpected error occurred";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(BIRD_ID, It.IsAny<int>(), It.IsAny<int>());

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
        const int BIRD_ID = 1001;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = "an unexpected error occurred";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<ObservationsPagedDto>(null));

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(BIRD_ID, It.IsAny<int>(), It.IsAny<int>());

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

    [Fact]
    public async Task GetObservationsByBirdSpeciesAsync_When_BirdId_Is_Zero_Returns_400()
    {
        // Arrange
        const int BIRD_ID_IS_ZERO = 0;
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = "birdId is zero";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<ObservationsPagedDto>(null));

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByBirdSpeciesAsync(BIRD_ID_IS_ZERO, It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedResponseObject, actual);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((o, t) => string.Equals("birdId argument is 0", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }

    [Fact]
    public async Task GetObservationsByUserAsync_On_Success_Returns_200()
    {
        // Arrange
        const string USERNAME = "USERNAME";
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = new ObservationsPagedDto();
        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedResponseObject);

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByUserAsync(USERNAME, It.IsAny<int>(), It.IsAny<int>());

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
    public async Task GetObservationsByUserAsyncOn_Exception_Returns_500()
    {
        // Arrange
        const string USERNAME = "USERNAME";
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = "an unexpected error occurred";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByUserAsync(USERNAME, It.IsAny<int>(), It.IsAny<int>());

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
    public async Task GetObservationsByUserAsync_When_Object_Is_NULL_Returns_500()
    {
        // Arrange
        const string USERNAME = "USERNAME";
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = "an unexpected error occurred";


        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<ObservationsPagedDto>(null));

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByUserAsync(USERNAME, It.IsAny<int>(), It.IsAny<int>());

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

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetObservationsByUserAsync_When_Username_Is_NULL_Or_Empty_Returns_400(string username)
    {
        // Arrange
        Mock<ILogger<ObservationReadController>> loggerMock = new();

        var expectedResponseObject = "username is null or empty";

        var mockService = new Mock<IObservationQueryService>();
        mockService.Setup(obs => obs.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(),
                                                                  It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<ObservationsPagedDto>(null));

        var controller = new ObservationReadController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationsByUserAsync(username, It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedResponseObject, actual);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((o, t) => string.Equals("username argument is null or empty", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }
}