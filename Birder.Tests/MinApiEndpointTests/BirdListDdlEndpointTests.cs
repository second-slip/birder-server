using Birder.MinApiEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Birder.Tests.MinApiEndpoints;

public class GetBirdsDdlAsyncTests
{

    [Fact]
    public async Task GetBirdsAsync_Returns_Ok_When_Request_Resource_Exists()
    {
        // Arrange
        var mock = new Mock<IBirdDataService>();

        mock.Setup(m => m.GetBirdsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>()))
                .ReturnsAsync(new BirdsListDto() { });

        var result = await BirdEndpoints.GetBirdsAsync(mock.Object, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>());

        Assert.IsType<Ok<BirdsListDto>>(result);
    }

    [Fact]
    public async Task GetBirdsAsync_Returns_NotFound_When_Request_Resource_Not_Exists()
    {
        // Arrange
        var service = new Mock<IBirdDataService>();

        service.Setup(m => m.GetBirdsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>()))
                .Returns(Task.FromResult<BirdsListDto>(null));

        var result = await BirdEndpoints.GetBirdsAsync(service.Object, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>());

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetBirdsAsync_Returns_BadRequest_When_Service_Is_Null()
    {
        // Arrange
        IBirdDataService service = null; // new Mock<IBirdDataService>(null);

        var result = await BirdEndpoints.GetBirdsAsync(service, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<BirderStatus>());

        Assert.IsType<BadRequest>(result);
    }




















    [Fact]
    public async Task GetBirdAsync_Returns_Ok_When_Request_Resource_Exists()
    {
        // Arrange
        var mock = new Mock<IBirdDataService>();

        var requestedId = 1009;

        mock.Setup(m => m.GetBirdAsync(requestedId))
                .ReturnsAsync(new BirdDetailDto() { BirdId = requestedId });

        var result = await BirdEndpoints.GetBirdAsync(mock.Object, requestedId);

        Assert.IsType<Ok<BirdDetailDto>>(result);
    }

    [Fact]
    public async Task GetBirdAsync_Returns_NotFound_When_Request_Resource_Not_Exists()
    {
        // Arrange
        var service = new Mock<IBirdDataService>();

        var requestedId = 1009;

        service.Setup(m => m.GetBirdAsync(requestedId))
                .Returns(Task.FromResult<BirdDetailDto>(null));

        var result = await BirdEndpoints.GetBirdAsync(service.Object, requestedId);

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetBirdAsync_Returns_BadRequest_When_Service_Is_Null()
    {
        // Arrange
        IBirdDataService service = null; // new Mock<IBirdDataService>(null);

        var requestedId = 1009;

        var result = await BirdEndpoints.GetBirdAsync(service, requestedId);

        Assert.IsType<BadRequest>(result);
    }

    [Fact]
    public async Task GetBirdsDdlAsync_Returns_NotFound_IfNotExists()
    {
        // Arrange
        // Mock<ILogger<BirdEndpoints>> loggerMock = new();
        var mock = new Mock<ICachedBirdsDdlService>();

        mock.Setup(m => m.GetAll())
                .ReturnsAsync(new List<BirdSummaryDto>());

        // Act
        var result = await BirdEndpoints.GetBirdsDdlAsync(mock.Object);

        // Assert
        Assert.IsType<NotFound>(result);
        // var notFoundResult = (NotFound)result.Result;
        // Assert.NotNull(notFoundResult);

        // loggerMock.Verify(x => x.Log(
        //     It.IsAny<LogLevel>(),
        //     It.IsAny<EventId>(),
        //     It.Is<It.IsAnyType>((v, t) => true),
        //     It.IsAny<Exception>(),
        //     It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        //     Times.Never);
    }

    [Fact]
    public async Task GetBirdsDdlAsync_Returns_Ok()
    {
        // Arrange
        // Mock<ILogger<BirdEndpoints>> loggerMock = new();
        var mock = new Mock<ICachedBirdsDdlService>();

        mock.Setup(m => m.GetAll())
         .ReturnsAsync(
               new List<BirdSummaryDto> {
                new BirdSummaryDto
                {
                    BirdId = 1,
                    Species = "",
                    EnglishName = "Test species 1",
                    PopulationSize = "",
                    BtoStatusInBritain = "",
                    ThumbnailUrl = "",
                    ConservationListColourCode = "",
                    ConservationStatus = "",
                    BirderStatus = BirderStatus.Common
                },
                new BirdSummaryDto
                {
                    BirdId = 2,
                    Species = "",
                    EnglishName = "Test species 2",
                    PopulationSize = "",
                    BtoStatusInBritain = "",
                    ThumbnailUrl = "",
                    ConservationListColourCode = "",
                    ConservationStatus = "",
                    BirderStatus = BirderStatus.Common
                }
            }.AsReadOnly);

        // Act
        var result = await BirdEndpoints.GetBirdsDdlAsync(mock.Object);

        // Assert
        Assert.IsType<Ok<IReadOnlyList<BirdSummaryDto>>>(result);

        // var okResult = (IReadOnlyList<BirdSummaryDto>)result.;
        // // Assert.NotNull(okResult.Value);
        // // Assert.NotEmpty(okResult.Value);
        // Assert.Collection(okResult, bird1 =>
        // {
        //     Assert.Equal(1, bird1.BirdId);
        //     Assert.Equal("Test species 1", bird1.EnglishName);
        // }, bird2 =>
        // {
        //     Assert.Equal(2, bird2.BirdId);
        //     Assert.Equal("Test species 2", bird2.EnglishName);
        // });


    }

    // [Fact]
    // public async Task GetBirdsDdlAsync_Returns_500_On_Exception()
    // {
    //     // Arrange
    //     Mock<ILogger<BirdEndpoints>> loggerMock = new();
    //     var mock = new Mock<ICachedBirdsDdlService>();

    //     mock.Setup(m => m.GetAll())
    //         .Throws(new InvalidOperationException());

    //     // Act
    //     var result = await BirdEndpoints.GetBirdsDdlAsync(mock.Object, loggerMock.Object);

    //     // Assert
    //     Assert.IsType<Results<Ok<IReadOnlyList<BirdSummaryDto>>, NotFound, StatusCodeHttpResult>>(result);
    //     var internalServerError = (StatusCodeHttpResult)result.Result;
    //     Assert.Equal(StatusCodes.Status500InternalServerError, internalServerError.StatusCode);
    //     Assert.NotNull(internalServerError);

    //     loggerMock.Verify(x => x.Log(
    //         It.IsAny<LogLevel>(),
    //         It.IsAny<EventId>(),
    //         It.Is<It.IsAnyType>((v, t) => true),
    //         It.IsAny<Exception>(),
    //         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
    //         Times.Once);
    // }
}