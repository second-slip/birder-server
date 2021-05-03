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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class CreateObservationAsyncTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationController>> _logger;
        private readonly ISystemClockService _systemClock;

        public CreateObservationAsyncTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _systemClock = new SystemClockService();
        }

        #region CreateObservationAsync

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CreateObservationAsync_ReturnsBadRequest_OnInvalidModelState(int id)
        {
            //Arrange
            int birdId = 1;
            //var model = GetTestObservationViewModel(id, birdId);
            var model = new ObservationAddDto()
            {
                //ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
                Position = new ObservationPositionDto() { }
            };
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            //           .ThrowsAsync(new InvalidOperationException());
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();

            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _systemClock
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object
                , mockObsPositionRepo.Object
                , mockObsNotesRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            controller.ModelState.AddModelError("Test", "This is a test model error");

            // Act
            var result = await controller.CreateObservationAsync(model);

            // Assert
            string expectedMessage = "An error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CreateObservationAsync_ReturnsNotFound_WhenRequestingUserNotFound(int id)
        {
            //Arrange
            int birdId = 1;
            //var model = GetTestObservationViewModel(id, birdId);
            var model = new ObservationAddDto()
            {
                //ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
                Position = new ObservationPositionDto() { }
            };
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                            .Returns(Task.FromResult<ApplicationUser>(null));
            var mockObsRepo = new Mock<IObservationRepository>();
            //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            //           .ThrowsAsync(new InvalidOperationException());
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _systemClock
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object
                , mockObsPositionRepo.Object
                , mockObsNotesRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            // Act
            var result = await controller.CreateObservationAsync(model);

            // Assert
            string expectedMessage = "Requesting user not found";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task CreateObservationAsync_ReturnsNotFound_WhenBirdNotFound(int id, int birdId)
        {
            //Arrange
            //var model = GetTestObservationViewModel(id, birdId);
            var model = new ObservationAddDto()
            {
                //ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
                Position = new ObservationPositionDto() { }
            };
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Bird>(null));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            //           .ThrowsAsync(new InvalidOperationException());
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _systemClock
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object
                , mockObsPositionRepo.Object
                , mockObsNotesRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            // Act
            var result = await controller.CreateObservationAsync(model);

            // Assert
            string expectedMessage = $"Bird species with id '{model.BirdId}' was not found.";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CreateObservationAsync_ReturnsBadRequest_OnException(int id)
        {
            //Arrange
            int birdId = 1;
            //var model = GetTestObservationViewModel(id, birdId);
            var model = new ObservationAddDto()
            {
                //ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
                Position = new ObservationPositionDto() { }
            };
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestBird(birdId));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.Add(It.IsAny<Observation>()))
                       .Throws(new InvalidOperationException());
            var mockObjectValidator = new Mock<IObjectModelValidator>();
            mockObjectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _systemClock
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object
                , mockObsPositionRepo.Object
                , mockObsNotesRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            controller.ObjectValidator = mockObjectValidator.Object;

            // Act
            var result = await controller.CreateObservationAsync(model);

            // Assert
            string expectedMessage = "An unexpected error occurred.";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        public async Task CreateObservationAsync_ReturnsOkWithObservationViewModel_OnSuccess(int id, int birdId)
        {
            //Arrange
            //var model = GetTestObservationViewModel(id, birdId);
            var model = new ObservationAddDto()
            {
                //ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
                Position = new ObservationPositionDto() { }
            };
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(w => w.CompleteAsync())
                .Returns(Task.CompletedTask);
            var mockBirdRepo = new Mock<IBirdRepository>();
            mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestBird(birdId));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.Add(It.IsAny<Observation>()))
                       .Verifiable();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();
            mockObsNotesRepo.Setup(o => o.Add(It.IsAny<ObservationNote>()))
                        .Verifiable();
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            mockObsPositionRepo.Setup(o => o.Add(It.IsAny<ObservationPosition>()))
                        .Verifiable();
            var mockObjectValidator = new Mock<IObjectModelValidator>();
            mockObjectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            

            var controller = new ObservationController(
                _mapper
                , _systemClock
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object
                , mockObsPositionRepo.Object
                , mockObsNotesRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            controller.ObjectValidator = mockObjectValidator.Object;

            // Act
            var result = await controller.CreateObservationAsync(model);

            // Assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("CreateObservationAsync", objectResult.ActionName);
            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
            var actual = Assert.IsType<ObservationDto>(objectResult.Value);
            Assert.Equal(model.BirdId, actual.BirdId);
        }

        #endregion


        private Bird GetTestBird(int birdId)
        {
            return new Bird() { BirdId = birdId };
        }

        private ObservationDto GetTestObservationViewModel(int id, int birdId)
        {
            return new ObservationDto()
            {
                ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
                Position = new ObservationPositionDto() { }
            };
        }

        private ApplicationUser GetUser(string username)
        {
            return new ApplicationUser()
            {
                UserName = username
            };
        }


        private IEnumerable<Observation> GetTestObservations(int length, Bird bird)
        {
            var observations = new List<Observation>();
            for (int i = 0; i < length; i++)
            {
                observations.Add(new Observation
                {
                    ObservationId = i,
                    Quantity = 1,
                    HasPhotos = false,
                    SelectedPrivacyLevel = PrivacyLevel.Public,
                    ObservationDateTime = DateTime.Now.AddDays(-4),
                    CreationDate = DateTime.Now.AddDays(-4),
                    LastUpdateDate = DateTime.Now.AddDays(-4),
                    ApplicationUserId = "",
                    BirdId = bird.BirdId,
                    Bird = bird,
                    ApplicationUser = null,
                    ObservationTags = null
                });
            }
            return observations;
        }


    }
}
