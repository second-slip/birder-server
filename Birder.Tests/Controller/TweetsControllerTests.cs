using AutoMapper;
using Birder.Controllers;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
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
        //private readonly Mock<ITweetDayRepository> _tweetDayRepository;

        public TweetsControllerTests()
        {
            //_tweetDayRepository = new Mock<ITweetDayRepository>();
            _cache = new Mock<IMemoryCache>();
            _mapper = new Mock<IMapper>();
            _systemClock = new Mock<ISystemClockService>();
            _logger = new Mock<ILogger<TweetsController>>();

        }

        [Fact]
        public async Task GetTweetDay_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(DateTime.Today))
                .ReturnsAsync(GetTestSession());

            var controller = new TweetsController(mockRepo.Object, _cache.Object, _logger.Object,
                                                     _systemClock.Object, _mapper.Object);

            //// Act
            //var result = await controller.Index();

            //// Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
            //    viewResult.ViewData.Model);
            //Assert.Equal(2, model.Count());
        }

        private TweetDay GetTestSession()
        {
            var session = new TweetDay()
            {

            };

            return session;
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
