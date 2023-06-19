using Microsoft.Extensions.Caching.Memory;

namespace Birder.Tests.Services;

public class CachedBirdsDdlServiceTests
{
    [Fact]
    public async Task GetAll______On_Success_Returns_200()
    {
        // Arrange
        Mock<ILogger<BirdsController>> loggerMock = new();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var expectedResponseObject = new List<BirdSummaryDto>();

        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(b => b.GetBirdsDropDownListAsync())
                .ReturnsAsync(expectedResponseObject);

        var sut = new CachedBirdsDdlService(cache, mockService.Object);

        // Act
        var actual = await sut.GetAll(); // controller.GetBirdsDdlAsync();

        // Assert
        actual.ShouldBeType<List<BirdSummaryDto>>();
        Assert.IsAssignableFrom<List<BirdSummaryDto>>(actual);


        // loggerMock.Verify(x => x.Log(
        //     It.IsAny<LogLevel>(),
        //     It.IsAny<EventId>(),
        //     It.Is<It.IsAnyType>((v, t) => true),
        //     It.IsAny<Exception>(),
        //     It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        //     Times.Never);
    }


}