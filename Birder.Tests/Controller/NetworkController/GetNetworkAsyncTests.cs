using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestSupport.EfHelpers;
using Xunit.Extensions.AssertExtensions;
using System;
using Birder.TestsHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Identity;
using Birder.Data.Model;

namespace Birder.Tests.Controller
{
    public class GetNetworkAsyncTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<NetworkController>> _logger;

        public GetNetworkAsyncTests()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<NetworkController>>();
        }

        [Fact]
        public async Task GetNetworkSummaryAsync_Returns_500_On_Internal_Error()
        {
            // Arrange
            UserManager<ApplicationUser> userManager = null; //to cause internal error

            var mockRepo = new Mock<INetworkRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, new NullLogger<NetworkController>(), mockRepo.Object, userManager);

            string requesterUsername = "testUser1";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetNetworkSummaryAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal($"an unexpected error occurred", objectResult.Value);
        }

        [Fact]
        public async Task GetNetworkSummaryAsync_Returns_500_When_User_Is_Null()
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            //  this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                //context.Database.EnsureCreated();
                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var mockRepo = new Mock<INetworkRepository>();
                var mockUnitOfWork = new Mock<IUnitOfWork>();
                var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

                string requesterUsername = "This requested user does not exist";

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                // Act
                var result = await controller.GetNetworkSummaryAsync();

                // Assert
                var objectResult = Assert.IsType<ObjectResult>(result);
                Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
                Assert.Equal($"requesting user not found", objectResult.Value);
            }
        }

        [Fact]
        public async Task GetNetworkSummaryAsync_Returns_Ok()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.Database.EnsureClean();
                //context.Database.EnsureCreated();

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

                //string requestedUsername = "Tenko";

                string requesterUsername = "testUser1";

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
                };

                // Act
                var result = await controller.GetNetworkSummaryAsync();

                // Assert
                var objectResult = Assert.IsType<OkObjectResult>(result); ;
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                Assert.IsType<NetworkSummaryDto>(objectResult.Value);
            }
        }

    }
}
