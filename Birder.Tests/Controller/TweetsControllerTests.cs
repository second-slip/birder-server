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
        public async Task GetTweetDay_ReturnsOkObjectResult_WithATweetObject()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            //mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(DateTime.Today))
            //     .ReturnsAsync(GetTestTweet()); //--> needs a real SystemClockService
            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(GetTestTweetDay());

            var controller = new TweetsController(mockRepo.Object, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetDay();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

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
            var result = await controller.GetTweetDay();
            
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
            var result = await controller.GetTweetDay();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTweetDay_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult<TweetDay>(null));

            // cache object is null => raise an exception
            var controller = new TweetsController(mockRepo.Object, null, _logger.Object,
                                                     _systemClock.Object, _mapper);

            // Act
            var result = await controller.GetTweetDay();

            // Assert
            Assert.IsType<BadRequestResult>(result);
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
    }
}
