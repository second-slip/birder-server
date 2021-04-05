using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class TweetsControllerTests
    {
        private IMemoryCache _cache; // Mock<IMemoryCache> _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TweetsController>> _logger;
        private readonly Mock<ISystemClockService> _systemClock;

        public TweetsControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions()); // new Mock<IMemoryCache>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _systemClock = new Mock<ISystemClockService>();
            _logger = new Mock<ILogger<TweetsController>>();
        }

        [Fact]
        public async Task GetTweetArchive_ReturnsNotFoundResult_WhenRepoReturnsNull()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetArchiveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<QueryResult<TweetDay>>(null));

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetArchiveAsync(1, 1);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal($"tweets repository returned null", objectResult.Value);
        }

        [Fact]
        public async Task GetTweetArchive_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetArchiveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new InvalidOperationException());

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

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
            var mockRepo = new Mock<ITweetDayRepository>();

            mockRepo.Setup(repo => repo.GetTweetArchiveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                       .ReturnsAsync(GetQueryResult(30));

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetArchiveAsync(1, 25);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<TweetArchiveDto>(objectResult.Value);
        }



        #region GetTweetDay


        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithViewModel()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();

            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(GetTestTweetDay());

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetDayAsync();
            
            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<TweetDayViewModel>(objectResult.Value);
        }

        [Fact]
        public async Task GetTweetDay_ReturnsNotFoundResult_WhenTweetIsNull()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult<TweetDay>(null));

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetDayAsync();

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal($"tweets repository returned null", objectResult.Value);
        }

        [Fact]
        public async Task GetTweetDay_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new InvalidOperationException());

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetDayAsync();

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("an unexpected error occurred", objectResult.Value);
        }

        #endregion

        private QueryResult<TweetDay> GetQueryResult(int length)
        {
            var result = new QueryResult<TweetDay>();
            //var bird = new Bird() { BirdId = 1 };

            result.TotalItems = length;
            result.Items = GetTweetDayCollection(length);

            return result;
        }

        private TweetDay GetTestTweetDay()
        {
            var tweet = new TweetDay()
            {
                Bird = new Bird(),
                BirdId = 0,
                CreationDate = DateTime.Now.AddDays(-4),
                DisplayDay = DateTime.Today.AddDays(-2),
                LastUpdateDate = DateTime.Now.AddDays(-3),
                TweetDayId = 0
            };

            return tweet;
        }

        private IEnumerable<TweetDay> GetTweetDayCollection(int length)
        {
            var tweets = new List<TweetDay>();

            for (int i = 0; i < length; i++)
            {
                var tweet = new TweetDay()
                {
                    Bird = new Bird(),
                    BirdId = 0,
                    CreationDate = DateTime.Now.AddDays(-4),
                    DisplayDay = DateTime.Today.AddDays(-2),
                    LastUpdateDate = DateTime.Now.AddDays(-3),
                    TweetDayId = 0
                };
                tweets.Add(tweet);
            };

            return tweets;
        }
    }
}
