using AutoMapper;
using Birder.Controllers;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Castle.Core.Logging;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class TweetsControllerTests
    {
        private Mock<IMemoryCache> _cache;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ISystemClockService> _systemClock;
        //private readonly Mock<ITweetDayRepository> _tweetDayRepository;

        public TweetsControllerTests()
        {
            //_tweetDayRepository = new Mock<ITweetDayRepository>();

        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var mockRepo = new Mock<ITweetDayRepository>();
            mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(DateTime.Today))
                .ReturnsAsync(GetTestSession());

            var a = new Mock<IMemoryCache>();
            var b = new Mock<IMapper>();
            var c = new Mock<ISystemClockService>();
            var d = new Mock<ILogger>();

            //var controller = new TweetsController(mockRepo.Object);

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
