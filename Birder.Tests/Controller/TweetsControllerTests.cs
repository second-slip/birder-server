using Microsoft.Extensions.Caching.Memory;

namespace Birder.Tests.Controller;

public class TweetsControllerTests
{
    private IMemoryCache _cache;
    private readonly Mock<ILogger<TweetsController>> _logger;
    private readonly Mock<ISystemClockService> _systemClock;

    public TweetsControllerTests()
    {
        _cache = new MemoryCache(new MemoryCacheOptions()); // new Mock<IMemoryCache>();
        _systemClock = new Mock<ISystemClockService>();
        _logger = new Mock<ILogger<TweetsController>>();
    }

    [Fact]
    public async Task GetTweetArchive_ReturnsNotFoundResult_WhenRepoReturnsNull()
    {
        // Arrange
        var service = new Mock<ITweetDataService>();
        service.Setup(repo => repo.GetTweetArchiveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult<IEnumerable<TweetDayDto>>(null));

        var controller = new TweetsController(_cache, _logger.Object, _systemClock.Object, service.Object);

        // Act
        var result = await controller.GetTweetArchiveAsync(1, 1);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"tweets service returned null", objectResult.Value);
    }

    [Fact]
    public async Task GetTweetArchive_ReturnsBadRequestResult_WhenExceptionIsRaised()
    {
        // Arrange
        var service = new Mock<ITweetDataService>();
        service.Setup(repo => repo.GetTweetArchiveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new TweetsController(_cache, _logger.Object, _systemClock.Object, service.Object);

        // Act
        var result = await controller.GetTweetArchiveAsync(1, 1);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("an unexpected error occurred", objectResult.Value);
    }

    [Fact]
    public async Task GetTweetArchiveAsync_ReturnsOkObjectResult_WithObject()
    {
        // Arrange
        var service = new Mock<ITweetDataService>();

        service.Setup(repo => repo.GetTweetArchiveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                   .ReturnsAsync(GetTweetDayCollection(30));

        var controller = new TweetsController(_cache, _logger.Object, _systemClock.Object, service.Object);

        // Act
        var result = await controller.GetTweetArchiveAsync(1, 25);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsAssignableFrom<IEnumerable<TweetDayDto>>(objectResult.Value);
    }



    #region GetTweetDay


    [Fact]
    public async Task Get_ReturnsOkObjectResult_WithViewModel()
    {
        // Arrange
        var service = new Mock<ITweetDataService>();

        service.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(GetTestTweetDay());

        var controller = new TweetsController(_cache, _logger.Object, _systemClock.Object, service.Object);

        // Act
        var result = await controller.GetTweetDayAsync();

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<TweetDayDto>(objectResult.Value);
    }

    [Fact]
    public async Task GetTweetDay_ReturnsNotFoundResult_WhenTweetIsNull()
    {
        // Arrange
        var service = new Mock<ITweetDataService>();
        service.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
            .Returns(Task.FromResult<TweetDayDto>(null));

        var controller = new TweetsController(_cache, _logger.Object, _systemClock.Object, service.Object);

        // Act
        var result = await controller.GetTweetDayAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal($"tweets service returned null", objectResult.Value);
    }

    [Fact]
    public async Task GetTweetDay_ReturnsBadRequestResult_WhenExceptionIsRaised()
    {
        // Arrange
        var service = new Mock<ITweetDataService>();
        service.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new TweetsController(_cache, _logger.Object, _systemClock.Object, service.Object);

        // Act
        var result = await controller.GetTweetDayAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("an unexpected error occurred", objectResult.Value);
    }

    #endregion

    private TweetDayDto GetTestTweetDay()
    {
        var tweet = new TweetDayDto()
        {
            BirdId = 0,
            CreationDate = DateTime.Now.AddDays(-4),
            DisplayDay = DateTime.Today.AddDays(-2),
            LastUpdateDate = DateTime.Now.AddDays(-3),
            TweetDayId = 0,
            EnglishName = "",
            SongUrl = "",
            Species = ""
        };

        return tweet;
    }

    private static IEnumerable<TweetDayDto> GetTweetDayCollection(int length)
    {
        var tweets = new List<TweetDayDto>();

        for (int i = 0; i < length; i++)
        {
            var tweet = new TweetDayDto()
            {
                BirdId = i + 1,
                CreationDate = DateTime.Now.AddDays(-4),
                DisplayDay = DateTime.Today.AddDays(-2),
                LastUpdateDate = DateTime.Now.AddDays(-3),
                TweetDayId = i + 1,
                EnglishName = "",
                SongUrl = "",
                Species = ""
            };

            tweets.Add(tweet);
        };

        return tweets;
    }
}