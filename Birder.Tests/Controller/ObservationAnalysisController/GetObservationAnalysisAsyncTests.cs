using Birder.Data.Repository;

namespace Birder.Tests.Controller;

public class GetObservationAnalysisAsyncTests
{
    private readonly Mock<ILogger<ObservationAnalysisController>> _logger;

    public GetObservationAnalysisAsyncTests()
    {
        _logger = new Mock<ILogger<ObservationAnalysisController>>();
    }

    #region GetObservationAnalysisAsync tests

    [Fact]
    public async Task GetObservationAnalysisAsync_ReturnsOkObjectResult_WithOkResult()
    {
        // Arrange
        var mockAnalysisService = new Mock<IObservationsAnalysisService>();
        var mockRepo = new Mock<IObservationRepository>();

        mockAnalysisService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .ReturnsAsync(new ObservationAnalysisViewModel { TotalObservationsCount = 2, UniqueSpeciesCount = 2 });

        var controller = new ObservationAnalysisController(_logger.Object, mockAnalysisService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationAnalysisAsync("test");

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var actualObs = Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
        Assert.Equal(2, actualObs.TotalObservationsCount);
        Assert.Equal(2, actualObs.UniqueSpeciesCount);
    }


    [Fact]
    public async Task GetObservationAnalysisAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
    {
        // Arrange
        var mockAnalysisService = new Mock<IObservationsAnalysisService>();
        var mockRepo = new Mock<IObservationRepository>();
        mockAnalysisService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                  .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationAnalysisController(_logger.Object, mockAnalysisService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationAnalysisAsync("test");

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal("an unexpected error occurred", objectResult.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetObservationAnalysisAsync_ReturnsBadRequest_WhenArgument_Is_Invalid(string requstedUsername)
    {
        // Arrange
        var mockAnalysisService = new Mock<IObservationsAnalysisService>();
        var mockRepo = new Mock<IObservationRepository>();
        mockAnalysisService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                  .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationAnalysisController(_logger.Object, mockAnalysisService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationAnalysisAsync(requstedUsername);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal("requestedUsername is missing", objectResult.Value);
    }

    #endregion
}