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

namespace Birder.Tests.Controller
{
    public class ObservationAnalysisControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationAnalysisController>> _logger;
        private readonly ISystemClockService _systemClock;
        private readonly IObservationRepository _observationRepository;

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


        [Fact]
        public async Task GetBirds_ReturnsOkObjectResult_WithABirdsObject()
        {
            // Arrange
            var mockRepo = new Mock<IObservationRepository>();
            mockRepo.Setup(repo => repo.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
             .ReturnsAsync(GetTestObservations());
            //(x => x.ApplicationUser.UserName == "")) //(It.IsAny<Func<Observation, bool>>()))

            //var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);
            var controller = new ObservationAnalysisController(mockRepo.Object, _logger.Object, _cache, _systemClock, _mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["UniqueName"] = ""; // ["device-id"] = "20317";
            // Act
            var result = await controller.GetObservationAnalysis();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }



        #region ObservationRepository mock methods

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
