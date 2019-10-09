using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class ObservationControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationController>> _logger;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ISystemClockService> _systemClock;
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
            _systemClock = new Mock<ISystemClockService>();
        }

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

            var controller = new ObservationController(
                _mapper
                ,_cache
                ,_systemClock.Object
                ,mockUnitOfWork.Object
                ,mockBirdRepo.Object
                ,_logger.Object
                ,mockUserManager.Object
                ,mockObsRepo.Object);

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

            var controller = new ObservationController(
                _mapper
                , _cache
                , _systemClock.Object
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            // Act
            var result = await controller.GetObservationAsync(id);

            // Assert
            string expectedMessage = $"Observation with id '{id}' was not found.";

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
                       .ReturnsAsync(GetObservation(id, requestingUser));

            var controller = new ObservationController(
                _mapper
                , _cache
                , _systemClock.Object
                , mockUnitOfWork.Object
                , mockBirdRepo.Object
                , _logger.Object
                , mockUserManager.Object
                , mockObsRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            // Act
            var result = await controller.GetObservationAsync(id);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var actualObs = Assert.IsType<ObservationViewModel>(objectResult.Value);
            Assert.Equal(id, actualObs.ObservationId);
            Assert.Equal(requestingUser.UserName, actualObs.User.UserName);
        }

        private ApplicationUser GetUser(string username)
        {
            return new ApplicationUser()
            {
                UserName = username
            };
        }

        private Observation GetObservation(int id, ApplicationUser user)
        {
            return new Observation()
            {
                ObservationId = id,
                ApplicationUser = user
            };

        }

    }
}
