using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using TestSupport.EfHelpers;
using Birder.TestsHelpers;

namespace Birder.Tests.Controller
{
    public class GetNetworkSuggestionsAsyncTests
    {

        private readonly IMapper _mapper;
        private readonly Mock<ILogger<NetworkController>> _logger;
        //private readonly UserManager<ApplicationUser> _userManager;

        public GetNetworkSuggestionsAsyncTests()
        {
            //_userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<NetworkController>>();
        }

        #region GetNetworkSuggestionsAsync action tests

        [Fact]
        public async Task GetNetworkSuggestionsAsync_Exception_ReturnsBadRequest()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                //context.ConservationStatuses.Add(new ConservationStatus { ConservationList = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                // Arrange
                var mockRepo = new Mock<INetworkRepository>();

                var mockUnitOfWork = new Mock<IUnitOfWork>();

                var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

                //string requestedUsername = "Tenko";

                string requesterUsername = string.Empty;

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                // Act
                var result = await controller.GetNetworkSuggestionsAsync();

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var objectResult = result as ObjectResult;
                Assert.Equal($"An unexpected error occurred", objectResult.Value);
            }

        }

        [Fact]
        public async Task GetNetworkSuggestionsAsync_ReturnsNotFoundWithstringObject_WhenRepositoryReturnsNullUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                //context.ConservationStatuses.Add(new ConservationStatus { ConservationList = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                // Arrange
                var mockUnitOfWork = new Mock<IUnitOfWork>();
                var mockRepo = new Mock<INetworkRepository>();

                var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

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
        }


        [Fact]
        public async Task GetNetworkSuggestionsAsync_ValidRequest_ReturnsOkWithObject()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                //context.ConservationStatuses.Add(new ConservationStatus { ConservationList = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);

                var mockUnitOfWork = new Mock<IUnitOfWork>();
                var mockRepo = new Mock<INetworkRepository>();

                var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

                string requesterUsername = "testUser1";

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
                Assert.IsType<List<NetworkUserViewModel>>(objectResult.Value);
            }
        }


        #endregion
    }
}
