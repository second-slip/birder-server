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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class ObservationControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationController>> _logger;
        //private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ISystemClockService _systemClock;
        //private readonly Mock<IUnitOfWork _unitOfWork;
        //private readonly IBirdRepository _birdRepository;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IObservationRepository _observationRepository;

        public ObservationControllerTests()
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

        #region GetObservationAsync()

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetObservationAsync_ReturnsNotFound_WhenObservationIsNotFound(int id)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
                       .Returns(Task.FromResult<Observation>(null));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationAsync(id);

            // Assert
            string expectedMessage = $"Observation with id '{id}' was not found.";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetObservationAsync_ReturnsBadRequest_OnException(int id)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
                       .ThrowsAsync(new InvalidOperationException());
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationAsync(id);

            // Assert
            string expectedMessage = "An error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetObservationAsync_ReturnsOkWithObservation_OnSuccessfulRequest(int id)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
                       .ReturnsAsync(GetTestObservation(id, requestingUser));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationAsync(id);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var actualObs = Assert.IsType<ObservationDto>(objectResult.Value);
            Assert.Equal(id, actualObs.ObservationId);
            Assert.Equal(requestingUser.UserName, actualObs.User.UserName);
        }

        #endregion


        #region GetObservationsByUserAsync

        [Theory]
        [InlineData("")]
        [InlineData("Test")]
        [InlineData("TestAgain")]
        public async Task GetObservationsByUserAsync_ReturnsNotFound_WhenObservationsIsNotFound(string username)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                       .Returns(Task.FromResult<IEnumerable<Observation>>(null));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationsByUserAsync(username, 1, 1);

            // Assert
            string expectedMessage = $"Observations with username '{username}' was not found.";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Test")]
        [InlineData("TestAgain")]
        public async Task GetObservationsByUserAsync_ReturnsBadRequest_OnException(string username)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ThrowsAsync(new InvalidOperationException());
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationsByUserAsync(username, 1, 10);

            // Assert
            string expectedMessage = "An error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData("Test", 1, 4)]
        [InlineData("TestUser", 2, 3)]
        [InlineData("TestUsername", 3, 2)]
        public async Task GetObservationsByUserAsync_ReturnsOkWithObservations_OnSuccessfulRequest(string username, int birdId, int length)
        {
            //Arrange
            var requestingUser = GetUser("Any");
            var bird = new Bird() { BirdId = birdId };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ReturnsAsync(GetQueryResult(length, bird));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationsByUserAsync(username, 1, 10);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var actualObs = Assert.IsType<ObservationFeedDto>(objectResult.Value);
            Assert.Equal(length, actualObs.TotalItems);
            Assert.Equal(birdId, actualObs.Items.First().BirdId);
        }

        #endregion


        #region GetObservationsByBirdSpeciesAsync

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetObservationsBySpeciesAsync_ReturnsNotFound_WhenObservationsIsNotFound(int birdId)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                       .Returns(Task.FromResult<IEnumerable<Observation>>(null));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationsByBirdSpeciesAsync(birdId, 1, 1);

            // Assert
            string expectedMessage = $"Observations with birdId '{birdId}' was not found.";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetObservationsBySpeciesAsync_ReturnsBadRequest_OnException(int birdId)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ThrowsAsync(new InvalidOperationException());
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationsByBirdSpeciesAsync(birdId, 1, 10);

            // Assert
            string expectedMessage = "An error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async Task GetObservationsBySpeciesAsync_ReturnsOkWithObservations_OnSuccessfulRequest(int birdId, int length)
        {
            //Arrange
            var requestingUser = GetUser("Any");
            var bird = new Bird() { BirdId = birdId };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetPagedObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ReturnsAsync(GetQueryResult(length, bird));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.GetObservationsByBirdSpeciesAsync(birdId, 1, 10);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var actualObs = Assert.IsType<ObservationFeedDto>(objectResult.Value);
            Assert.Equal(length, actualObs.TotalItems);
            Assert.Equal(birdId, actualObs.Items.First().BirdId);
        }

        #endregion





        #region DeleteObservationAsync

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteObservationAsync_ReturnsBadRequest_WhenObservationNotFound(int id)
        {
            //Arrange
            var requestingUser = GetUser("Any");
            //var model = GetTestObservationViewModel(id, birdId, requestingUser);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult<Observation>(null));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.DeleteObservationAsync(id);

            // Assert
            string expectedMessage = $"Observation with id '{id}' was not found";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task DeleteObservationAsync_ReturnsUnauthorised_WhenRequestingUserIsNotObservationOwner(int id, int birdId)
        {
            //Arrange
            var requestingUser = GetUser("Any");
            var model = GetTestObservationViewModel(id, birdId, requestingUser);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(GetTestObservation(0, new ApplicationUser { UserName = "Someone else" }));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.DeleteObservationAsync(id);

            // Assert
            string expectedMessage = "Requesting user is not allowed to delete this observation";

            var objectResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task DeleteObservationAsync_ReturnsBadRequest_OnException(int id, int birdId)
        {
            //Arrange
            var model = GetTestObservationViewModel(id, birdId);
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
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
                , _cache
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
            var result = await controller.DeleteObservationAsync(id);

            // Assert
            string expectedMessage = "An unexpected error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task DeleteObservationAsync_ReturnsOk_OnSuccess(int id)
        {
            //Arrange
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(w => w.CompleteAsync())
                .Returns(Task.CompletedTask);
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(GetTestObservation(id, requestingUser));
            var mockObjectValidator = new Mock<IObjectModelValidator>();
            mockObjectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

            var controller = new ObservationController(
                _mapper
                , _cache
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
            var result = await controller.DeleteObservationAsync(id);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<int>(objectResult.Value);
            Assert.Equal(actual, id);
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
            };
        }

        private ObservationDto GetTestObservationViewModel(int id, int birdId, ApplicationUser user)
        {
            return new ObservationDto()
            {
                ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
            };
        }


        private ApplicationUser GetUser(string username)
        {
            return new ApplicationUser()
            {
                UserName = username
            };
        }

        private Observation GetTestObservation(int id, ApplicationUser user)
        {
            return new Observation()
            {
                ObservationId = id,
                ApplicationUser = user,
                ObservationDateTime = _systemClock.GetNow
            };
        }

        private QueryResult<Observation> GetQueryResult(int length, Bird bird)
        {
            var result = new QueryResult<Observation>();
            //var bird = new Bird() { BirdId = 1 };

            result.TotalItems = length;
            result.Items = GetTestObservations(length, bird);

            return result;
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
