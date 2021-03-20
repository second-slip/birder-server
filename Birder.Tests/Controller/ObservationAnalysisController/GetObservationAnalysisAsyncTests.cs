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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class GetObservationAnalysisAsyncTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationAnalysisController>> _logger;
        private readonly ISystemClockService _systemClock;
        public GetObservationAnalysisAsyncTests()
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
        #region GetObservationAnalysisAsync tests

        [Fact]
        public async Task GetObservationAnalysisAsync_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();

            mockAnalysisService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                .ReturnsAsync(new ObservationAnalysisViewModel { TotalObservationsCount = 2, UniqueSpeciesCount = 2 });

            var controller = new ObservationAnalysisController(_logger.Object, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetObservationAnalysisAsync("test");

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var actualObs = Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
            Assert.Equal(2, actualObs.TotalObservationsCount);
            Assert.Equal(2, actualObs.UniqueSpeciesCount);
        }




        [Fact]
        public async Task GetObservationAnalysisAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockAnalysisService = new Mock<IObservationsAnalysisService>();
            var mockRepo = new Mock<IObservationRepository>();
            //mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            //        .ThrowsAsync(new InvalidOperationException());
            mockAnalysisService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                      .ThrowsAsync(new InvalidOperationException());

            var controller = new ObservationAnalysisController(_logger.Object, mockAnalysisService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetObservationAnalysisAsync("test");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred", objectResult.Value);
        }

        #endregion
    }
}

