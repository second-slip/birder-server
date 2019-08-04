using AutoMapper;
using Birder.Controllers;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class TweetsControllerTests
    {
        private IMemoryCache _cache; // Mock<IMemoryCache> _cache;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<TweetsController>> _logger;
        private readonly Mock<ISystemClockService> _systemClock;
        //private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsControllerTests()
        {
            //_tweetDayRepository = new TweetDayRepository(new ApplicationDbCon);
            _cache = new MemoryCache(new MemoryCacheOptions()); // new Mock<IMemoryCache>();
            _mapper = new Mock<IMapper>();
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
                                                     _systemClock.Object, _mapper.Object);

            // Act
            var result = await controller.GetTweetDay();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTweetDay_ReturnsBadRequestResult_WhenTweetIsNull()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            //mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
            //    .ReturnsAsync(GetTestTweetDay());

            var controller = new TweetsController(null, _cache, _logger.Object,
                                                     _systemClock.Object, _mapper.Object);


            //var controller = new SessionController(sessionRepository: null);
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





    //var entryMock = new Mock<ICacheEntry>();
    //myCache.Setup(m => m.CreateEntry(It.IsAny<object>())
    //               .Returns(entryMock.Object));

    //var myCache = new MemoryCache(new MemoryCacheOptions());
    //var memCache = new MemoryCache(MemoryCacheOptions.)
}
