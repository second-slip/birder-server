using Microsoft.Extensions.Caching.Memory;

namespace Birder.Tests.Services;

public class CachedBirdsDdlServiceTests
{
    [Fact]
    public async Task GetAll_Returns_Result_From_Service_When_Cache_Is_Empty()
    {
        // Arrange
        const string cacheKey = CachedBirdsDdlService.CacheKey;
        var cache = new MemoryCache(new MemoryCacheOptions());
        var expectedResponseObject = new List<BirdSummaryDto>();

        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(b => b.GetBirdsDropDownListAsync())
                .ReturnsAsync(expectedResponseObject);

        var sut = new CachedBirdsDdlService(cache, mockService.Object);

        // Act
        Assert.False(cache.TryGetValue(cacheKey, out _)); // no cache at the initial stage

        var actual = await sut.GetAll(); // get the cached value for the first time

        Assert.True(cache.TryGetValue(cacheKey, out _)); // stored values into memoryCache due to query

        // Assert
        Assert.IsAssignableFrom<List<BirdSummaryDto>>(actual);
        mockService.Verify(x => x.GetBirdsDropDownListAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_Returns_Result_From_Cache_When_Cache_Is_Set()
    {
        // Arrange
        const string cacheKey = CachedBirdsDdlService.CacheKey;
        var cache = new MemoryCache(new MemoryCacheOptions());
        var expectedResponseObject = new List<BirdSummaryDto>();
        var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
        //
        cache.Set(cacheKey, expectedResponseObject, options);
        //
        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(b => b.GetBirdsDropDownListAsync())
                .ReturnsAsync(expectedResponseObject);

        var sut = new CachedBirdsDdlService(cache, mockService.Object);

        // Act
        Assert.True(cache.TryGetValue(cacheKey, out _)); // cache at the initial stage

        var actual = await sut.GetAll(); // get the cached value for the first time

        Assert.True(cache.TryGetValue(cacheKey, out _)); // stored values still in cache

        // Assert
        Assert.IsAssignableFrom<List<BirdSummaryDto>>(actual);
        mockService.Verify(x => x.GetBirdsDropDownListAsync(), Times.Never);
    }

    [Fact]
    public async Task GetAll_Cache_Should_Have_Correct_LifeCycle()
    {
        //Arrange
        const string cacheKey = CachedBirdsDdlService.CacheKey;
        var cache = new MemoryCache(new MemoryCacheOptions());
        var expectedResponseObject = new List<BirdSummaryDto>();

        var mockService = new Mock<IBirdDataService>();
        mockService.Setup(b => b.GetBirdsDropDownListAsync())
                .ReturnsAsync(expectedResponseObject);

        var sut = new CachedBirdsDdlService(cache, mockService.Object);

        //Act
        Assert.False(cache.TryGetValue(cacheKey, out _)); // no cache at the initial stage

        await sut.GetAll(); // get the cached value for the first time

        Assert.True(cache.TryGetValue(cacheKey, out _)); // stored values into memoryCache due to query

        mockService.Verify(x => x.GetBirdsDropDownListAsync(), Times.Once);

        await sut.GetAll(); //  get the cached value for the second time

        // assert that should have executed one query, means no new query, because all data is already in memoryCache
        mockService.Verify(x => x.GetBirdsDropDownListAsync(), Times.Once);
    }
}