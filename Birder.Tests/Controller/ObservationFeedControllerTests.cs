using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class ObservationFeedControllerTests
    {
        private const int pageSize = 10;
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationFeedController>> _logger;
        //private readonly ISystemClockService _systemClock;


        public ObservationFeedControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationFeedController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            //_systemClock = new SystemClockService();
        }



        [Fact]
        public async Task GetObservationsFeedAsync_FilterOwn_ReturnsNotFound_WhenRepoReturnsNull()
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<QueryResult<Observation>>(null));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Own;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            string expectedMessage = $"Observations not found";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task GetObservationsFeedAsync_FilterOwn_ReturnsOk_OnSuccess(int length)
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GetQueryResult(length));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Own;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
            Assert.Equal(length, actual.TotalItems);
            Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
            Assert.Equal(filter, actual.ReturnFilter);
        }



        private QueryResult<Observation> GetQueryResult(int length)
        {
            var result = new QueryResult<Observation>();
            var bird = new Bird() { BirdId = 1 };

            result.TotalItems = length;
            result.Items = SharedFunctions.GetTestObservations(1, bird);

            return result;
        }
    }
}