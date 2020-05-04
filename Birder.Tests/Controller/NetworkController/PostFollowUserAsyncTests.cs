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
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class PostFollowUserAsyncTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<NetworkController>> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostFollowUserAsyncTests()
        {
            _userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<NetworkController>>();
        }

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        //{
        //    var UserStoreMock = Mock.Of<IUserStore<ApplicationUser>>();
        //    var userMgr = new Mock<UserManager<ApplicationUser>>(UserStoreMock, null, null, null, null, null, null, null, null);
        //    var user = new ApplicationUser() { Id = "f00", UserName = "f00", Email = "f00@example.com" };
        //    var tcs = new TaskCompletionSource<ApplicationUser>();
        //    tcs.SetResult(user);
        //    userMgr.Setup(x => x.FindByIdAsync("f00")).Returns(tcs.Task);


        //}
        //public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        //{
        //    var store = new Mock<IUserStore<TUser>>();
        //    var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        //    mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        //    mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        //    mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        //    mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
        //    mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

        //    return mgr;
        //}


        #region Follow action tests

        [Fact]
        public async Task PostFollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid1()
        {
            var UserStoreMock = Mock.Of<IUserStore<ApplicationUser>>();
            var userMgr = new Mock<UserManager<ApplicationUser>>(UserStoreMock, null, null, null, null, null, null, null, null);
            var user = new ApplicationUser() { Id = "f00", UserName = "f00", Email = "f00@example.com" };
            var tcs = new TaskCompletionSource<ApplicationUser>();
            tcs.SetResult(user);
            userMgr.Setup(x => x.FindByIdAsync("f00")).Returns(tcs.Task);
            //userMgr.Setup(i => i.GetUserWithNetworkAsync("w")).Returns(tcs.Task);

            // Arrange
            var mockRepo = new Mock<INetworkRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userMgr.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");


            // Act
            var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel("Test User"));

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
            var actual = Assert.IsType<string>(objectResult.Value);

            //Assert.Contains("This is a test model error", "This is a test model error");
            Assert.Equal("Invalid modelstate", actual);
        }

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsNotFound_WhenRequestingUserIsNullFromRepository()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<INetworkRepository>();
        //    //mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<string>()))
        //    //        .Returns(Task.FromResult<ApplicationUser>(null));

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

        //    string requestingUser = "This requested user does not exist";

        //    string userToFollow = "This requested user does not exist";

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel(userToFollow));

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<NotFoundObjectResult>(result);
        //    Assert.True(objectResult is NotFoundObjectResult);
        //    Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        //    Assert.IsType<string>(objectResult.Value);
        //    Assert.Equal("Requesting user not found", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsNotFound_WhenUserToFollowIsNullFromRepository()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<INetworkRepository>();
        //    //mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<string>()))
        //    //        .Returns(Task.FromResult<ApplicationUser>(null));

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

        //    string requestingUser = "Tenko";

        //    string userToFollow = "This requested user does not exist";

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel(userToFollow));

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<NotFoundObjectResult>(result);
        //    Assert.True(objectResult is NotFoundObjectResult);
        //    Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        //    Assert.IsType<string>(objectResult.Value);
        //    Assert.Equal("User to follow not found", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<INetworkRepository>();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

        //    string requestingUser = "Tenko";

        //    string userToFollow = requestingUser;

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel(userToFollow));

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        //    var actual = Assert.IsType<string>(objectResult.Value);
        //    Assert.Equal("Trying to follow yourself", actual);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequestWithstringObject_WhenExceptionIsRaised()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<INetworkRepository>();
        //    mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
        //        .Verifiable();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(x => x.CompleteAsync())
        //        .ThrowsAsync(new InvalidOperationException());

        //    var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

        //    string requestingUser = "Tenko";

        //    string userToFollow = "Toucan";

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel(userToFollow));

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    var objectResult = result as ObjectResult;
        //    Assert.Equal($"An error occurred trying to follow user: {userToFollow}", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<INetworkRepository>();
        //    mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
        //        .Verifiable();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

        //    var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, _userManager);

        //    string requestingUser = "Tenko";

        //    string userToFollow = "Toucan";

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel(userToFollow));

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<OkObjectResult>(result);
        //    Assert.True(objectResult is OkObjectResult);
        //    Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        //    Assert.IsType<NetworkUserViewModel>(objectResult.Value);

        //    var model = objectResult.Value as NetworkUserViewModel;
        //    Assert.Equal(userToFollow, model.UserName);
        //}

        #endregion

        private NetworkUserViewModel GetTestNetworkUserViewModel(string username)
        {
            return new NetworkUserViewModel() { UserName = username };
        }

        //[Fact]
        //public async Task<IdentityResult> Register(UserDto data)
        //{
        //    //SystemUser user = ConvertDtoToUser(data);
        //    var userManager = MockUserManager.
        //    IdentityResult result = userManager.CreateAsync(user, data.Password);

        //    //some more code that is dependent on the result
        //}
        ////////////////////////////////////////////
        public static class MockHelpers
        {
            public static StringBuilder LogMessage = new StringBuilder();

            public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
            {
                var store = new Mock<IUserStore<TUser>>();
                var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
                mgr.Object.UserValidators.Add(new UserValidator<TUser>());
                mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
                return mgr;
            }

            public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
            {
                store = store ?? new Mock<IRoleStore<TRole>>().Object;
                var roles = new List<IRoleValidator<TRole>>();
                roles.Add(new RoleValidator<TRole>());
                return new Mock<RoleManager<TRole>>(store, roles, new UpperInvariantLookupNormalizer(),
                    new IdentityErrorDescriber(), null);
            }

            public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
            {
                store = store ?? new Mock<IUserStore<TUser>>().Object;
                var options = new Mock<IOptions<IdentityOptions>>();
                var idOptions = new IdentityOptions();
                idOptions.Lockout.AllowedForNewUsers = false;
                options.Setup(o => o.Value).Returns(idOptions);
                var userValidators = new List<IUserValidator<TUser>>();
                var validator = new Mock<IUserValidator<TUser>>();
                userValidators.Add(validator.Object);
                var pwdValidators = new List<PasswordValidator<TUser>>();
                pwdValidators.Add(new PasswordValidator<TUser>());
                var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                    userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                    new IdentityErrorDescriber(), null,
                    new Mock<ILogger<UserManager<TUser>>>().Object);
                validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                    .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
                return userManager;
            }

            public static RoleManager<TRole> TestRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
            {
                store = store ?? new Mock<IRoleStore<TRole>>().Object;
                var roles = new List<IRoleValidator<TRole>>();
                roles.Add(new RoleValidator<TRole>());
                return new RoleManager<TRole>(store, roles,
                    new UpperInvariantLookupNormalizer(),
                    new IdentityErrorDescriber(),
                    null);
            }

        }


    }
}
