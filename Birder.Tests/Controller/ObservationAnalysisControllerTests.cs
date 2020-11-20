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
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class ObservationAnalysisControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationAnalysisController>> _logger;
        private readonly ISystemClockService _systemClock;

        public ObservationAnalysisControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationAnalysisController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _systemClock = new SystemClockService();
        }

        //ToDo: Test cache routes



        #region GetTopObservationAnalysisAsync tests

        [Fact]
        public async Task GetTopObservationAnalysisAsync_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetTopObservationAnalysisAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTopObservationAnalysisAsync_ReturnsOkObjectResult_WithTopObservationsAnalysisViewModel()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetTopObservationAnalysisAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<TopObservationsAnalysisViewModel>(objectResult.Value);
        }

        [Fact]
        public async Task GetTopObservationAnalysisAsync_ReturnsUnauthorizedResult_WhenClaimsPrincipalIsNull()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = null }
            };

            // Act
            var result = await controller.GetTopObservationAnalysisAsync();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetTopObservationAnalysisAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ThrowsAsync(new InvalidOperationException());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetTopObservationAnalysisAsync();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred", objectResult.Value);
        }

        #endregion

        #region GetLifeListAsync

        [Fact]
        public async Task GetLifeListAsync_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetLifeListAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLifeListAsync_ReturnsOkObjectResult_WithLifeListViewModel()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetLifeListAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(objectResult.Value);
        }

        [Fact]
        public async Task GetLifeListAsync_ReturnsUnauthorizedResult_WhenClaimsPrincipalIsNull()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = null }
            };

            // Act
            var result = await controller.GetLifeListAsync();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetLifeListAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ThrowsAsync(new InvalidOperationException());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetLifeListAsync();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred", objectResult.Value);
        }

        #endregion



        #region ObservationRepository mock methods

        private ClaimsPrincipal GetTestClaimsPrincipal()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

        private IEnumerable<Observation> GetTestObservations()
        {
            var observations = new List<Observation>();

            observations.Add(new Observation
            {
                ObservationId = 1,
                Quantity = 1,
                HasPhotos = false,
                SelectedPrivacyLevel = PrivacyLevel.Public,
                ObservationDateTime = DateTime.Now.AddDays(-4),
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                BirdId = 1,
                ApplicationUserId = "",
                Bird = null,
                ApplicationUser = null,
                ObservationTags = null
            });
            observations.Add(new Observation
            {
                ObservationId = 2,
                Quantity = 1,
                HasPhotos = false,
                SelectedPrivacyLevel = PrivacyLevel.Public,
                ObservationDateTime = DateTime.Now.AddDays(-4),
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                BirdId = 1,
                ApplicationUserId = "",
                Bird = null,
                ApplicationUser = null,
                ObservationTags = null
            });

            return observations;
        }

        #endregion
    }
}
