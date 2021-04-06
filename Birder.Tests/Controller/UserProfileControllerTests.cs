using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.TestsHelpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.Controller
{
    public class UserProfileControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserProfileController>> _logger;

        public UserProfileControllerTests()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<UserProfileController>>();

        }

        #region GetUser action tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetUserProfileAsync_ReturnsBadRequest_WhenStringArgumentIsNullOrEmpty(string requestedUsername)
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            var controller = new UserProfileController(_mapper, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("NonExistentUsername") }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("requestedUsername argument is null or empty", objectResult.Value);
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsNotFound_WhenRequestedUserIsNull()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var controller = new UserProfileController(_mapper, _logger.Object, userManager);

                string requestedUsername = "This requested user does not exist";

                string requesterUsername = requestedUsername;

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                // Act
                var result = await controller.GetUserProfileAsync(requestedUsername);

                // Assert
                Assert.IsType<ObjectResult>(result);
                var objectResult = result as ObjectResult;
                Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
                Assert.Equal($"userManager returned null", objectResult.Value);
            }
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsOkObjectResultWithUserProfileViewModel_WhenRequestedUserIsRequestingUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var controller = new UserProfileController(_mapper, _logger.Object, userManager);

                string requestedUsername = "testUser1";

                string requesterUsername = requestedUsername;

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                // Act
                var result = await controller.GetUserProfileAsync(requestedUsername);

                // Assert
                var objectResult = result as ObjectResult;
                Assert.NotNull(objectResult);
                Assert.IsType<OkObjectResult>(result);
                Assert.True(objectResult is OkObjectResult);
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                Assert.IsType<UserProfileViewModel>(objectResult.Value);

                var model = objectResult.Value as UserProfileViewModel;
                Assert.Equal(requestedUsername, model.UserName);
            }
        }

        // Don't bother getting requesterUser object.  Just use username from claim instead
        //[Fact]
        //public async Task GetUserProfileAsync_ReturnsNotFound_WhenRequesterUserIsNull()
        //{
        //    var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        //You have to create the database
        //        context.Database.EnsureClean();
        //        context.Database.EnsureCreated();
        //        //context.SeedDatabaseFourBooks();

        //        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        //        context.Users.Add(SharedFunctions.CreateUser("testUser2"));

        //        context.SaveChanges();

        //        context.Users.Count().ShouldEqual(2);

        //        // Arrange
        //        var userManager = SharedFunctions.InitialiseUserManager(context);
        //        var controller = new UserProfileController(_mapper, _logger.Object, userManager);

        //        string requestedUsername = "testUser2";

        //        string requesterUsername = "This requested user does not exist";

        //        controller.ControllerContext = new ControllerContext()
        //        {
        //            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
        //        };

        //        // Act
        //        var result = await controller.GetUserProfileAsync(requestedUsername);

        //        // Assert
        //        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        //        Assert.IsType<string>(objectResult.Value);
        //        Assert.Equal("Requesting user not found", objectResult.Value);
        //    }
        //}

        [Fact]
        public async Task GetUserProfileAsync_ReturnsOkObjectResultWithUserProfileViewModel_WhenRequestedUserIsNotRequestingUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var controller = new UserProfileController(_mapper, _logger.Object, userManager);

                string requestedUsername = "testUser2";

                string requesterUsername = "testUser1";

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                // Act
                var result = await controller.GetUserProfileAsync(requestedUsername);

                // Assert
                var objectResult = result as ObjectResult;
                Assert.NotNull(objectResult);
                Assert.True(objectResult is OkObjectResult);
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                Assert.IsType<UserProfileViewModel>(objectResult.Value);

                var model = objectResult.Value as UserProfileViewModel;
                Assert.Equal(requestedUsername, model.UserName);
            }
        }

        #endregion

    }
}
