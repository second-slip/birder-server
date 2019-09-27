using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserController>> _logger;

        public UserControllerTests()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<UserController>>();

        }

        #region GetUser action tests

        [Fact]
        public async Task GetUserAsync_ReturnsBadRequest_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string username = "Test username";

            // Act
            var result = await controller.GetUserAsync(username);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact] //Requesting own profile
        public async Task GetUserAsync_ReturnsOkObjectResult_WithOwnUserProfileViewModelObject()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOwnUserProfile());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string username = "Test username";

            // Act
            var result = await controller.GetUserAsync(username);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<UserProfileViewModel>(objectResult.Value);

            var model = objectResult.Value as UserProfileViewModel;
            Assert.Equal("Own Profile Test", model.UserName);
        }

        [Fact] //Requesting other member's profile
        public async Task GetUserAsync_ReturnsOkObjectResult_WithOtherMembersUserProfileViewModelObject()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOtherMemberUserProfile());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string username = "example name"; // same as claims principle
            //new Claim(ClaimTypes.Name, "example name"),
            
            // Act
            var result = await controller.GetUserAsync(username);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<UserProfileViewModel>(objectResult.Value);

            var model = objectResult.Value as UserProfileViewModel;
            Assert.Equal("Other Member's Profile Test", model.UserName);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string username = "Test username";

            // Act
            var result = await controller.GetUserAsync(username);

            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error getting the user", objectResult.Value);
        }

        #endregion


        #region GetNetwork action tests

        [Fact]
        public async Task GetNetworkAsync_ReturnsOkObjectResult_WhenRepositoryReturnsGetFollowersNotFollowedAsync()
        {
            // WHEN if (followersNotBeingFollowed.Count() == 0) IS FALSE
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOwnUserProfileWithOneFollower());
            
            //GetFollowersNotFollowedAsync
            mockRepo.Setup(repo => repo.GetFollowersNotFollowedAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(GetListOfApplicationUsers(3));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetNetworkAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

            var model = objectResult.Value as List<NetworkUserViewModel>;
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task GetNetworkAsync_ReturnsOkObjectResult_WhenRepositoryReturnsGetSuggestedBirdersToFollowAsync()
        {
            //WHEN if (followersNotBeingFollowed.Count() == 0) IS TRUE
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOwnUserProfile());

            // GetSuggestedBirdersToFollowAsync
            mockRepo.Setup(repo => repo.GetSuggestedBirdersToFollowAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(GetListOfApplicationUsers(5));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetNetworkAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

            var model = objectResult.Value as List<NetworkUserViewModel>;
            Assert.Equal(5, model.Count);
        }


        [Fact]
        public async Task GetNetworkAsync_ReturnsNotFoundWithStringObject_WhenRepositoryReturnsNullUser()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
                    .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetNetworkAsync();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task GetNetworkAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetNetworkAsync();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred", objectResult.Value);
        }

        #endregion

        #region SearchNetwork unit tests

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsOkWithNetworkListViewModelCollection_WhenRepositoryReturns3UserObjects()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOwnUserProfile());
            mockRepo.Setup(repo => repo.SearchBirdersToFollowAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(GetListOfApplicationUsers(3));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string searchCriterion = "Test String";

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);
            
            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);

            var model = objectResult.Value as List<NetworkUserViewModel>;
            Assert.Equal(3, model.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetSearchNetworkAsync_ReturnsBadRequestWithStringObject_WhenStringArgumentIsNullOrEmpty(string searchCriterion)
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("No search criterion", objectResult.Value);
        }

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsNotFoundWithStringObject_WhenRepositoryReturnsNullUser()
        {

            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .Returns(Task.FromResult<ApplicationUser>(null));
            //mockRepo.Setup(repo => repo.SearchBirdersToFollowAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
            //    .ReturnsAsync(GetListOfApplicationUsers(3));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string searchCriterion = "Test String";

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            string searchCriterion = "Test String";

            // Act
            var result = await controller.GetSearchNetworkAsync(searchCriterion);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred", objectResult.Value);
        }

        #endregion






        #region Follow action tests

        [Fact]
        public async Task PostFollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            // Act
            var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

            var modelState = controller.ModelState;
            Assert.Equal(1, modelState.ErrorCount);
            Assert.True(modelState.ContainsKey("Test"));
            Assert.True(modelState["Test"].Errors.Count == 1);
            Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);

            // test response
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            //
            Assert.IsType<String>(objectResult.Value);

            //Assert.Contains("This is a test model error", "This is a test model error");
            Assert.Equal("Invalid modelstate", objectResult.Value);
        }

        [Fact]
        public async Task PostFollowUserAsync_ReturnsNotFound_WhenUserIsNullFromRepository()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
                    .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task PostFollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
                    .ReturnsAsync(GetOwnUserProfile());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("Trying to follow yourself", objectResult.Value);
        }

        [Fact]
        public async Task PostFollowUserAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred trying to follow user: Test Network View Model", objectResult.Value);
        }

        [Fact]
        public async Task PostFollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.SetupSequence(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                .ReturnsAsync(GetOwnUserProfile())
                .ReturnsAsync(GetOtherMemberUserProfile())
                .ReturnsAsync(GetOwnUserProfile());

            mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
                .Verifiable();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<NetworkUserViewModel>(objectResult.Value);

            var model = objectResult.Value as NetworkUserViewModel;
            Assert.Equal("Other Member's Profile Test", model.UserName);
        }

        #endregion




        #region Unfollow action tests

        [Fact]
        public async Task PostUnfollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            // Act
            var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

            var modelState = controller.ModelState;
            Assert.Equal(1, modelState.ErrorCount);
            Assert.True(modelState.ContainsKey("Test"));
            Assert.True(modelState["Test"].Errors.Count == 1);
            Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);

            // test response
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            //
            Assert.IsType<String>(objectResult.Value);

            //Assert.Contains("This is a test model error", "This is a test model error");
            Assert.Equal("Invalid modelstate", objectResult.Value);
        }

        [Fact]
        public async Task PostUnfollowUserAsync_ReturnsNotFound_WhenUserIsNullFromRepository()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
                    .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task PostUnfollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
                    .ReturnsAsync(GetOwnUserProfile());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("Trying to unfollow yourself", objectResult.Value);
        }

        [Fact]
        public async Task PostUnfollowUserAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("An error occurred trying to unfollow user: Test Network View Model", objectResult.Value);
        }

        [Fact]
        public async Task PostUnfollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.SetupSequence(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                .ReturnsAsync(GetOwnUserProfile())
                .ReturnsAsync(GetOtherMemberUserProfile())
                .ReturnsAsync(GetOwnUserProfile());

            mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
                .Verifiable();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<NetworkUserViewModel>(objectResult.Value);

            var model = objectResult.Value as NetworkUserViewModel;
            Assert.Equal("Other Member's Profile Test", model.UserName);
        }

        #endregion


        #region Mock methods

        private NetworkUserViewModel GetTestNetworkUserViewModel()
        {
            return new NetworkUserViewModel()
            {
                UserName = "Test Network View Model"
            };
        }

        private List<ApplicationUser> GetListOfApplicationUsers(int collectionLength)
        {
            var users = new List<ApplicationUser>();

            for (int i = 0; i < collectionLength; i++)
            {
                users.Add(new ApplicationUser()
                {
                    UserName = "Test User " + (i + 1).ToString()
                });
            }

            return users;
        }

        private ApplicationUser GetOwnUserProfile()
        {
            var user = new ApplicationUser()
            {
                UserName = "Own Profile Test",
                Followers = new List<Network>(),
                Following = new List<Network>()
            };

            return user;
        }

        private ApplicationUser GetOwnUserProfileWithOneFollower()
        {
            var user = new ApplicationUser()
            {
                UserName = "Own Profile Test With Follower",
                Followers = new List<Network>() { new Network() { ApplicationUser = new ApplicationUser(), Follower = new ApplicationUser() } },
                Following = new List<Network>()
            };

            return user;
        }

        private ApplicationUser GetOtherMemberUserProfile()
        {
            var user = new ApplicationUser()
            {
                UserName = "Other Member's Profile Test"
            };

            return user;
        }

        //ToDo: move to shared
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

        #endregion
    }
}
