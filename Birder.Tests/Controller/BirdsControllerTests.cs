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
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class BirdsControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<BirdsController>> _logger;

        public BirdsControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions()); 
            _logger = new Mock<ILogger<BirdsController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        #region GetBirds (Collection) tests

        [Fact]
        public async Task GetBirds_ReturnsOkObjectResult_WithABirdsObject()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdSummaryListAsync())
                 .ReturnsAsync(GetTestBirds());

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdsAsync(BirderStatus.Common);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetBirds_ReturnsOkObjectResult_WithBirdSummaryViewModel()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdSummaryListAsync())
                 .ReturnsAsync(GetTestBirds());

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdsAsync(BirderStatus.Common);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<BirdSummaryViewModel>>(objectResult.Value);
        }

        [Fact]
        public async Task GetBirds_ReturnsNotFoundResult_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdSummaryListAsync())
                 .Returns(Task.FromResult<IEnumerable<Bird>>(null));

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdsAsync(BirderStatus.Common);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBirds_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdSummaryListAsync())
                .ReturnsAsync(GetTestBirds());
                 //.Returns(Task.FromResult<IEnumerable<Bird>>(null));

            // cache object is null => raise an exception
            var controller = new BirdsController(_mapper, null, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdsAsync(BirderStatus.Common);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region GetBird tests

        [Fact]
        public async Task GetBird_ReturnsOkObjectResult_WithABirdObject()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdAsync(It.IsAny<int>()))
                 .ReturnsAsync(GetTestBird());

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdAsync(It.IsAny<int>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetBird_ReturnsOkObjectResult_WithBirdDetailViewModel()
        {
            // Arrange
            var birdId = 1;
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdAsync(birdId))
                 .ReturnsAsync(GetTestBird());

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdAsync(birdId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var model = Assert.IsType<BirdDetailViewModel>(objectResult.Value);
            Assert.Equal(birdId, model.BirdId);
        }

        [Fact]
        public async Task GetBird_ReturnsNotFoundResult_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdAsync(It.IsAny<int>()))
                 .Returns(Task.FromResult<Bird>(null));

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBird_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestBird());

            // _mapper object is null => raise an exception
            var controller = new BirdsController(null, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }


        #endregion


        #region Repository mock methods

        private Bird GetTestBird()
        {
            var bird = new Bird
            {
                BirdId = 1,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "Test species 1",
                InternationalName = "",
                Category = "",
                PopulationSize = "",
                BtoStatusInBritain = "",
                ThumbnailUrl = "",
                SongUrl = "",
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                ConservationStatusId = 0,
                Observations = null,
                BirdConservationStatus = null,
                BirderStatus = BirderStatus.Common,
                TweetDay = null
            };

            return bird;
        }
        private IEnumerable<Bird> GetTestBirds()
        {
            var birds = new List<Bird>();
            birds.Add(new Bird
            {
                BirdId = 1,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "Test species 1",
                InternationalName = "",
                Category = "",
                PopulationSize = "",
                BtoStatusInBritain = "",
                ThumbnailUrl = "",
                SongUrl = "",
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                ConservationStatusId = 0,
                Observations = null,
                BirdConservationStatus = null,
                BirderStatus = BirderStatus.Common,
                TweetDay = null
            });
            birds.Add(new Bird
            {
                BirdId = 2,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "Test species 2",
                InternationalName = "",
                Category = "",
                PopulationSize = "",
                BtoStatusInBritain = "",
                ThumbnailUrl = "",
                SongUrl = "",
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                ConservationStatusId = 0,
                Observations = null,
                BirdConservationStatus = null,
                BirderStatus = BirderStatus.Common,
                TweetDay = null
            });

            return birds;
        }

        #endregion
    }
}
