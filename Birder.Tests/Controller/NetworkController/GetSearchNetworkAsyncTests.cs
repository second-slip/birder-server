using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.Controller
{
    public class GetSearchNetworkAsyncTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<NetworkController>> _logger;
        //private readonly UserManager<ApplicationUser> _userManager;

        public GetSearchNetworkAsyncTests()
        {
            //_userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<NetworkController>>();
        }




        [Fact]
        public async Task GetSearchNetworkAsync_Exception_ReturnsBadRequest()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                //context.ConservationStatuses.Add(new ConservationStatus { ConservationList = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var mockRepo = new Mock<INetworkRepository>();
                var mockUnitOfWork = new Mock<IUnitOfWork>();
                var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

                string requesterUsername = string.Empty;

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                string searchCriterion = "testUser2";

                // Act
                var result = await controller.GetSearchNetworkAsync(searchCriterion);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var objectResult = result as ObjectResult;
                Assert.Equal($"An unexpected error occurred", objectResult.Value);

            }
        }

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsOkWithNetworkListViewModelCollection_WhenSuccessful()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
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

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("testUser1") }
                };

                string searchCriterion = "testUser2";

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
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetSearchNetworkAsync_ReturnsBadRequestWithstringObject_WhenStringArgumentIsNullOrEmpty(string searchCriterion)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
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
        }

        [Fact]
        public async Task GetSearchNetworkAsync_ReturnsNotFoundWithstringObject_WhenRepositoryReturnsNullUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
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
        }









    }
}
