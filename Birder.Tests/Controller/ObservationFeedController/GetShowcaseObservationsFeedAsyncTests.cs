using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.TestsHelpers;
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
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class GetShowcaseObservationsFeedAsyncTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationFeedController>> _logger;
        private readonly ISystemClockService _systemClock;
        private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

        public GetShowcaseObservationsFeedAsyncTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationFeedController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _systemClock = new SystemClockService();
            _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
        }

        [Fact]
        public async Task GetShowcaseObservationsFeedAsync_OnException_ReturnsBadRequest()
        {
            // Arrange
            int mockQuanity = 1;

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetShowcaseObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException());

            _mockProfilePhotosService.Setup(obs => obs.GetUrlForObservations(It.IsAny<IEnumerable<Observation>>()))
            .Returns(SharedFunctions.GetTestObservations(1, new Bird()));

            var controller = new ObservationFeedController(_mapper, _cache, _systemClock, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { }
            };


            // Act
            var result = await controller.GetShowcaseObservationsFeedAsync(mockQuanity);

            // Assert
            string expectedMessage = "An unexpected error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Fact]
        public async Task GetShowcaseObservationsFeedAsync_RepoReturnsNullObject_ReturnsNotFound()
        {
            // Arrange
            int mockQuanity = 1;

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetShowcaseObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>()))
                .Returns(Task.FromResult<QueryResult<Observation>>(null));

            _mockProfilePhotosService.Setup(obs => obs.GetUrlForObservations(It.IsAny<IEnumerable<Observation>>()))
                .Returns(SharedFunctions.GetTestObservations(1, new Bird()));

            var controller = new ObservationFeedController(_mapper, _cache, _systemClock, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { }
            };

            // Act
            var result = await controller.GetShowcaseObservationsFeedAsync(mockQuanity);

            // Assert
            string expectedMessage = $"Showcase observations not found";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Fact]
        public async Task GetShowcaseObservationsFeedAsync_RepoReturnsObject_ReturnsSuccessWithObject()
        {
            // Arrange
            int mockQuanity = 1;
            int length = 10;

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetShowcaseObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>()))
                .ReturnsAsync(GetQueryResult(length));

            _mockProfilePhotosService.Setup(obs => obs.GetUrlForObservations(It.IsAny<IEnumerable<Observation>>()))
                .Returns(SharedFunctions.GetTestObservations(1, new Bird()));

            var controller = new ObservationFeedController(_mapper, _cache, _systemClock, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { }
            };

            // Act
            var result = await controller.GetShowcaseObservationsFeedAsync(mockQuanity);

            // Assert
            string expectedMessage = $"Showcase observations not found";

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
            Assert.Equal(length, actual.TotalItems);
            Assert.IsType<ObservationDto>(actual.Items.FirstOrDefault());
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
