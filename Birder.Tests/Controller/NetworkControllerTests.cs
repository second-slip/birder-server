using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using System;

namespace Birder.Tests.Controller
{
    public class NetworkControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<NetworkController>> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public NetworkControllerTests()
        {
            _userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<NetworkController>>();
        }

        #region GetNetworkAsync unit tests

        [Fact]
        public async Task GetNetworkAsync_ReturnsOkResultWithUserNetworkDto_WhenRequestIsSuccessful()
        {
            // Arrange
            var mockRepo = new Mock<INetworkRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            //string requestedUsername = "Tenko";

            string requesterUsername = "Toucan";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetNetworkAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var expected = await _userManager.GetUserWithNetworkAsync(requesterUsername);
            var actual = Assert.IsType<UserNetworkDto>(objectResult.Value);

            Assert.Equal(expected.Followers.Count, actual.Followers.Count());
            Assert.Equal(expected.Following.Count, actual.Following.Count());
        }

        [Fact]
        public async Task GetNetworkAsync_ReturnsNotFound_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockRepo = new Mock<INetworkRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            //string requestedUsername = "Tenko";
            string requesterUsername = "This requested user does not exist";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetNetworkAsync();

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Requesting user not found", objectResult.Value);
        }

        #endregion


        #region GetNetworkSuggestionsAsync action tests

        [Fact]
        public async Task GetNetworkSuggestionsAsync_ReturnsNotFoundWithstringObject_WhenRepositoryReturnsNullUser()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepo = new Mock<INetworkRepository>();
            
            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            string requesterUsername = "This requested user does not exist";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetNetworkSuggestionsAsync();

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Requesting user not found", objectResult.Value);
        }


        [Fact]
        public async Task GetNetworkSuggestionsAsync_ReturnsOkObjectResult_WhenRepositoryReturnsGetFollowersNotFollowedAsync()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepo = new Mock<INetworkRepository>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            string requesterUsername = "Tenko";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetNetworkSuggestionsAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

            //Assert.Equal(3, actual.Count);
        }


        #endregion


        #region SearchNetwork unit tests

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsOkWithNetworkListViewModelCollection_WhenSuccessful()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var mockRepo = new Mock<INetworkRepository>();
            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("Tenko") }
            };

            string searchCriterion = "a";

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

            //Assert.Equal(3, model.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetSearchNetworkAsync_ReturnsBadRequestWithstringObject_WhenstringArgumentIsNullOrEmpty(string searchCriterion)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var mockRepo = new Mock<INetworkRepository>();
            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("No search criterion", objectResult.Value);
        }

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsNotFoundWithstringObject_WhenRepositoryReturnsNullUser()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepo = new Mock<INetworkRepository>();
            
            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            string searchCriterion = "Test string";

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<string>(objectResult.Value);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Requesting user not found", objectResult.Value);
        }

        #endregion







    }
}
