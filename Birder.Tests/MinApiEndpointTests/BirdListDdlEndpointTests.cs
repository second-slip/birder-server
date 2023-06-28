using Birder.MinApiEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Birder.Tests.MinApiEndpoints;

public class GetBirdsDdlAsyncTests
{
    [Fact]
    public async Task GetBirdsDdlAsync_Returns_NotFound_IfNotExists()
    {
        // Arrange
        Mock<ILogger<BirdEndpoints>> loggerMock = new();
        var mock = new Mock<ICachedBirdsDdlService>();

        mock.Setup(m => m.GetAll())
            .ReturnsAsync((IReadOnlyList<BirdSummaryDto>)null);

        // Act
        var result = await BirdEndpoints.GetBirdsDdlAsync(mock.Object, loggerMock.Object);

        // Assert
        Assert.IsType<Results<Ok<IReadOnlyList<BirdSummaryDto>>, NotFound, StatusCodeHttpResult>>(result);
        var notFoundResult = (NotFound)result.Result;
        Assert.NotNull(notFoundResult);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task GetBirdsDdlAsync_Returns_Ok()
    {
        // Arrange
        Mock<ILogger<BirdEndpoints>> loggerMock = new();
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
        var result = await BirdEndpoints.GetBirdsDdlAsync(mock.Object, loggerMock.Object);

        // Assert
        Assert.IsType<Results<Ok<IReadOnlyList<BirdSummaryDto>>, NotFound, StatusCodeHttpResult>>(result);

        var okResult = (Ok<IReadOnlyList<BirdSummaryDto>>)result.Result;
        Assert.NotNull(okResult.Value);
        Assert.NotEmpty(okResult.Value);
        Assert.Collection(okResult.Value, bird1 =>
        {
            Assert.Equal(1, bird1.BirdId);
            Assert.Equal("Test species 1", bird1.EnglishName);
        }, bird2 =>
        {
            Assert.Equal(2, bird2.BirdId);
            Assert.Equal("Test species 2", bird2.EnglishName);
        });

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task GetBirdsDdlAsync_Returns_500_On_Exception()
    {
        // Arrange
        Mock<ILogger<BirdEndpoints>> loggerMock = new();
        var mock = new Mock<ICachedBirdsDdlService>();

        mock.Setup(m => m.GetAll())
            .Throws(new InvalidOperationException());

        // Act
        var result = await BirdEndpoints.GetBirdsDdlAsync(mock.Object, loggerMock.Object);

        // Assert
        Assert.IsType<Results<Ok<IReadOnlyList<BirdSummaryDto>>, NotFound, StatusCodeHttpResult>>(result);
        var internalServerError = (StatusCodeHttpResult)result.Result;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerError.StatusCode);
        Assert.NotNull(internalServerError);

        loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}