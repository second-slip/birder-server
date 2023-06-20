namespace Birder.Tests.Controller;

public class BirdsControllerTests
{
    #region GetBirds (Collection) tests

    [Fact]
    public async Task GetBirds_ReturnsOkObjectResult_WithABirdsObject()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>()))
             .ReturnsAsync(GetQueryResult(30));

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdsAsync(1, 25, BirderStatus.Common);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetBirds_ReturnsOkObjectResult_WithBirdSummaryDto()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>()))
        //.ReturnsAsync(GetTestBirds());
        .ReturnsAsync(GetQueryResult(30));

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdsAsync(1, 25, BirderStatus.Common);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsAssignableFrom<BirdsListDto>(objectResult.Value);
    }

    [Fact]
    public async Task GetBirds_ReturnsNotFoundResult_WhenRepositoryReturnsNull()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>()))
             .Returns(Task.FromResult<BirdsListDto>(null));

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>());

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"bird service returned null", objectResult.Value);
    }

    [Fact]
    public async Task GetBirds_ReturnsBadRequestResult_WhenExceptionIsRaised()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdsAsync(1, 25, BirderStatus.Common))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdsAsync(1, 25, BirderStatus.Common);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("an unexpected error occurred", objectResult.Value);
    }

    #endregion

    #region GetBird tests

    [Fact]
    public async Task GetBird_ReturnsOkObjectResult_WithABirdObject()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdAsync(It.IsAny<int>()))
             .ReturnsAsync(GetTestBird());

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdAsync(It.IsAny<int>());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetBird_ReturnsOkObjectResult_WithBirdDetailViewModel()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var birdId = 1;
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdAsync(birdId))
             .ReturnsAsync(GetTestBird());

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdAsync(birdId);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var model = Assert.IsType<BirdDetailDto>(objectResult.Value);
        Assert.Equal(birdId, model.BirdId);
    }

    [Fact]
    public async Task GetBird_ReturnsNotFoundResult_WhenRepositoryReturnsNull()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdAsync(It.IsAny<int>()))
             .Returns(Task.FromResult<BirdDetailDto>(null));

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdAsync(It.IsAny<int>());

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"bird service returned null", objectResult.Value);
    }

    [Fact]
    public async Task GetBird_ReturnsBadRequestResult_WhenExceptionIsRaised()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BirdsController>>();
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(repo => repo.GetBirdAsync(It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new BirdsController(mockLogger.Object, mockService.Object);

        // Act
        var result = await controller.GetBirdAsync(It.IsAny<int>());

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("an unexpected error occurred", objectResult.Value);
    }


    #endregion

    private BirdsListDto GetQueryResult(int length)
    {
        var result = new BirdsListDto();
        //var bird = new Bird() { BirdId = 1 };

        result.TotalItems = length;
        result.Items = SharedFunctions.GetTestBirds();

        return result;
    }
    #region BirdRepository mock methods

    private BirdDetailDto GetTestBird()
    {
        var bird = new BirdDetailDto
        {
            BirdId = 1,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "Test species 1",
            InternationalName = "",
            Category = "",
            PopulationSize = "",
            BtoStatusInBritain = "",
            ThumbnailUrl = "",
            CreationDate = DateTime.Now.AddDays(-4),
            LastUpdateDate = DateTime.Now.AddDays(-4),
            BirdConservationStatus = null,
            BirderStatus = BirderStatus.Common
        };

        return bird;
    }

    #endregion
}