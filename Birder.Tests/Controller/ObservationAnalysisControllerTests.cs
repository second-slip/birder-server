using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Birder.ViewModels;

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

        [Fact]
        public async Task GetObservationAnalysisAsync_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetObservationAnalysisAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetObservationAnalysisAsync_ReturnsOkObjectResult_WithObservationAnalysisViewModel()
        {
            // Arrange
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetObservationAnalysisAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
        }

        [Fact]
        public async Task GetObservationAnalysisAsync_ReturnsUnauthorizedResult_WhenClaimsPrincipalIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());

            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = null }
            };

            // Act
            var result = await controller.GetObservationAnalysisAsync();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetObservationAnalysisAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .ReturnsAsync(GetTestObservations());
            
            // cache object is null => raise an exception
            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, null, _systemClock, _mapper);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetObservationAnalysisAsync();

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }






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
                LocationLatitude = 0,
                LocationLongitude = 0,
                Quantity = 1,
                NoteGeneral = "",
                NoteHabitat = "",
                NoteWeather = "",
                NoteAppearance = "",
                NoteBehaviour = "",
                NoteVocalisation = "",
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
                LocationLatitude = 0,
                LocationLongitude = 0,
                Quantity = 1,
                NoteGeneral = "",
                NoteHabitat = "",
                NoteWeather = "",
                NoteAppearance = "",
                NoteBehaviour = "",
                NoteVocalisation = "",
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
