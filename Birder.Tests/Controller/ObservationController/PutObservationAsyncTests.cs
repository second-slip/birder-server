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
    public class PutObservationAsyncTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationController>> _logger;
        private readonly ISystemClockService _systemClock;

        public PutObservationAsyncTests()
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

        #region PutObservationAsync

        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 4)]
        [InlineData(3, 5)]
        public async Task PutObservationAsync_ReturnsBadRequest_OnInvalidModelState(int id, int birdId)
        {
            //Arrange
            var model = GetTestObservationEditViewModel(id, birdId);
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();
            //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            //           .ThrowsAsync(new InvalidOperationException());

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

            controller.ModelState.AddModelError("Test", "This is a test model error");

            // Act
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            string expectedMessage = "An error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Fact]
        public async Task PutObservationAsync_ReturnsBadRequest_OnIdNotEqualModelId()
        {
            //Arrange
            int birdId = 1;
            int id = 1;
            int modelId = 2;
            var model = GetTestObservationEditViewModel(modelId, birdId);
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>(); var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
            var mockObsNotesRepo = new Mock<IObservationNoteRepository>();
            //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            //           .ThrowsAsync(new InvalidOperationException());

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
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            string expectedMessage = "An error occurred (id)";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task PutObservationAsync_ReturnsNotFound_WhenObservationNotFound(int id, int birdId)
        {
            //Arrange
            var model = GetTestObservationEditViewModel(id, birdId);
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
            //    .Returns(Task.FromResult<Bird>(null));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
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
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            string expectedMessage = $"Observation with id '{model.ObservationId}' was not found.";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task PutObservationAsync_ReturnsBadRequest_WhenObservationNotFound(int id, int birdId)
        {
            //Arrange
            var model = GetTestObservationEditViewModel(id, birdId);
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
            //    .Returns(Task.FromResult<Bird>(null));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
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
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            string expectedMessage = $"Observation with id '{model.ObservationId}' was not found.";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task PutObservationAsync_ReturnsUnauthorised_WhenRequestingUserIsNotObservationOwner(int id, int birdId)
        {
            //Arrange
            var requestingUser = GetUser("Any");
            var model = GetTestObservationEditViewModel(id, birdId, requestingUser);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
            //    .Returns(Task.FromResult<Bird>(null));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
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
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            string expectedMessage = "Requesting user is not allowed to edit this observation";

            var objectResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public async Task PutObservationAsync_ReturnsBadRequest_OnException(int id, int birdId)
        {
            //Arrange
            var model = GetTestObservationEditViewModel(id, birdId);
            var requestingUser = GetUser("Any");

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBirdRepo = new Mock<IBirdRepository>();
            //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
            //    .ReturnsAsync(GetTestBird(birdId));
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
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
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            string expectedMessage = "An unexpected error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 5)]
        [InlineData(45, 12)]
        public async Task PutObservationAsync_ReturnsOkWithObservationViewModel_OnSuccess(int id, int birdId)
        {
            //Arrange
            var model = GetTestObservationEditViewModel(id, birdId);
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
            mockObsRepo.Setup(obs => obs.GetObservationAsync(It.IsAny<int>(), It.IsAny<bool>()))
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
            var result = await controller.PutObservationAsync(id, model);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<ObservationEditDto>(objectResult.Value);
            Assert.Equal(model.ObservationId, actual.ObservationId);
            Assert.Equal(model.BirdId, actual.Bird.BirdId);
        }

        #endregion


        private ObservationEditDto GetTestObservationEditViewModel(int id, int birdId)
        {
            return new ObservationEditDto()
            {
                ObservationId = id,
                Bird = new BirdSummaryViewModel() { BirdId = birdId },
                BirdId = birdId,
            };
        }

        private ObservationEditDto GetTestObservationEditViewModel(int id, int birdId, ApplicationUser user)
        {
            return new ObservationEditDto()
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

        private Bird GetTestBird(int birdId)
        {
            return new Bird() { BirdId = birdId };
        }
    }
}
