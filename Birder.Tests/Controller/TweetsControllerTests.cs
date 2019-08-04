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
        private Mock<IMemoryCache> _cache;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<TweetsController>> _logger;
        private readonly Mock<ISystemClockService> _systemClock;
        //private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsControllerTests()
        {
            //_tweetDayRepository = new TweetDayRepository(new ApplicationDbCon);
            _cache = new Mock<IMemoryCache>();
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
                .ReturnsAsync(GetTestTweet());

            var myCache = new MemoryCache(new MemoryCacheOptions());

            //var entryMock = new Mock<ICacheEntry>();
            //myCache.Setup(m => m.CreateEntry(It.IsAny<object>())
            //               .Returns(entryMock.Object));

            //var myCache = new MemoryCache(new MemoryCacheOptions());
            //var memCache = new MemoryCache(MemoryCacheOptions.)

            var controller = new TweetsController(mockRepo.Object, myCache, _logger.Object,
                                                     _systemClock.Object, _mapper.Object);

            // Act
            var result = await controller.GetTweetDay();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        private TweetDay GetTestTweet()
        {
            var tweet = new TweetDay()
            {
                Bird = new Bird(),
                BirdId = 0,
                CreationDate = new DateTime(2016, 7, 2),
                DisplayDay = new DateTime(2016, 7, 2),
                LastUpdateDate = new DateTime(2016, 7, 2),
                TweetDayId = 0
            };

            return tweet;
        }

        private IEnumerable<TweetDay> GetTestSessions()
        {
            var sessions = new List<TweetDay>();
            sessions.Add(new TweetDay()
            {
                //TweetDayId = 1,
                //DisplayDay = new DateTime(2016, 7, 1),
                //CreationDate = new DateTime(2016, 7, 1),
                //LastUpdateDate = new DateTime(2016, 7, 1),
                //Bird = null
            });
            sessions.Add(new TweetDay()
            {
                //TweetDayId = 2,
                //DisplayDay = new DateTime(2016, 7, 1),
                //CreationDate = new DateTime(2016, 7, 1),
                //LastUpdateDate = new DateTime(2016, 7, 1),
                //Bird = null
            });
            return sessions;
        }
    }
}
