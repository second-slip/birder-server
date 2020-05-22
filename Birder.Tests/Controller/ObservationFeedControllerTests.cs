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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.Controller
{
    public class ObservationFeedControllerTests
    {
        private const int pageSize = 10;
        //private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationFeedController>> _logger;
        //private readonly ISystemClockService _systemClock;
        private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

        public ObservationFeedControllerTests()
        {
            //_cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationFeedController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            //_systemClock = new SystemClockService();
            _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
        }



        [Fact]
        public async Task GetObservationsFeedAsync_ReturnsBadRequest_OnException()
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException());

            //var mockP = new Mock<IProfilePhotosService>();
            //var bird = new Bird();
            //mockP.Setup(obs => obs.GetThumbnailsUrl(It.IsAny<IEnumerable<Observation>>()))
            //.Returns(SharedFunctions.GetTestObservations(1, bird));
            //var bird = new Bird();
            _mockProfilePhotosService.Setup(obs => obs.GetUrlForObservations(It.IsAny<IEnumerable<Observation>>()))
            .Returns(SharedFunctions.GetTestObservations(1, new Bird()));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Own;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            string expectedMessage = "An unexpected error occurred";

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        #region When request Filter == Own

        [Fact]
        public async Task GetObservationsFeedAsync_FilterOwn_ReturnsNotFound_WhenRepoReturnsNull()
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<QueryResult<Observation>>(null));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Own;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            string expectedMessage = $"Observations not found";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task GetObservationsFeedAsync_FilterOwn_ReturnsOk_OnSuccess(int length)
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GetQueryResult(length));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Own;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
            Assert.Equal(length, actual.TotalItems);
            Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
            Assert.Equal(filter, actual.ReturnFilter);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task GetObservationsFeedAsync_FilterOwn_ReturnsOkWithNetwork_WhenNoOwnObservations(int length)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var requestingUsername = "testUser1";
                //var userManager = SharedFunctions.InitialiseUserManager();
                var mockObsRepo = new Mock<IObservationRepository>();
                mockObsRepo.SetupSequence(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(new QueryResult<Observation>())
                    .ReturnsAsync(GetQueryResult(length));

                var requestFilter = ObservationFeedFilter.Own;

                var controller = new ObservationFeedController(_mapper, _logger.Object, userManager, mockObsRepo.Object, _mockProfilePhotosService.Object);

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
                };

                // Act
                var result = await controller.GetObservationsFeedAsync(1, requestFilter);

                // Assert
                var returnFilter = ObservationFeedFilter.Network;

                var objectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
                Assert.Equal(length, actual.TotalItems);
                Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
                Assert.Equal(returnFilter, actual.ReturnFilter);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task GetObservationsFeedAsync_FilterOwn_ReturnsOkWithPublic_WhenNoOwnOrNetworkObservations(int length)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var requestingUsername = "testUser1";

                var mockObsRepo = new Mock<IObservationRepository>();
                mockObsRepo.SetupSequence(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(new QueryResult<Observation>())
                    .ReturnsAsync(new QueryResult<Observation>())
                    .ReturnsAsync(GetQueryResult(length));

                var requestFilter = ObservationFeedFilter.Own;

                var controller = new ObservationFeedController(_mapper, _logger.Object, userManager, mockObsRepo.Object, _mockProfilePhotosService.Object);

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
                };

                // Act
                var result = await controller.GetObservationsFeedAsync(1, requestFilter);

                // Assert
                var returnFilter = ObservationFeedFilter.Public;

                var objectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
                Assert.Equal(length, actual.TotalItems);
                Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
                Assert.Equal(returnFilter, actual.ReturnFilter);
            }
        }


        #endregion


        #region When request Filter == Network

        [Fact]
        public async Task GetObservationsFeedAsync_FilterNetwork_ReturnsNotFound_WhenUserIsNull()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var requestingUsername = "Does not exist";


                var mockObsRepo = new Mock<IObservationRepository>();
                //mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                //    .Returns(Task.FromResult<QueryResult<Observation>>(null));

                var controller = new ObservationFeedController(_mapper, _logger.Object, userManager, mockObsRepo.Object, _mockProfilePhotosService.Object);

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
                };

                var filter = ObservationFeedFilter.Network;

                // Act
                var result = await controller.GetObservationsFeedAsync(1, filter);

                // Assert
                string expectedMessage = "Requesting user not found";

                var objectResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
                var actual = Assert.IsType<string>(objectResult.Value);
                Assert.Equal(expectedMessage, actual);
            }
        }

        [Fact]
        public async Task GetObservationsFeedAsync_FilterNetwork_ReturnsNotFound_WhenRepoReturnsNull()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var requestingUsername = "testUser1";

                var mockObsRepo = new Mock<IObservationRepository>();
                mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult<QueryResult<Observation>>(null));

                var controller = new ObservationFeedController(_mapper, _logger.Object, userManager, mockObsRepo.Object, _mockProfilePhotosService.Object);

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
                };

                var filter = ObservationFeedFilter.Network;

                // Act
                var result = await controller.GetObservationsFeedAsync(1, filter);

                // Assert
                string expectedMessage = $"Observations not found";

                var objectResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
                var actual = Assert.IsType<string>(objectResult.Value);
                Assert.Equal(expectedMessage, actual);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task GetObservationsFeedAsync_FilterNetwork_ReturnsOkWithNetwork_OnSuccessfulRequest(int length)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);
                var requestingUsername = "testUser1";
                //var userManager = SharedFunctions.InitialiseUserManager();
                var mockObsRepo = new Mock<IObservationRepository>();
                mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                    //.ReturnsAsync(new QueryResult<Observation>())
                    .ReturnsAsync(GetQueryResult(length));

                var requestFilter = ObservationFeedFilter.Network;

                var controller = new ObservationFeedController(_mapper, _logger.Object, userManager, mockObsRepo.Object, _mockProfilePhotosService.Object);

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
                };

                // Act
                var result = await controller.GetObservationsFeedAsync(1, requestFilter);

                // Assert
                var returnFilter = ObservationFeedFilter.Network;

                var objectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
                Assert.Equal(length, actual.TotalItems);
                Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
                Assert.Equal(returnFilter, actual.ReturnFilter);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(9)]
        [InlineData(31)]
        public async Task GetObservationsFeedAsync_FilterNetwork_ReturnsOkWithPublic_WhenNoNetwork(int length)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(2);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);

                var requestingUsername = "testUser1";
                //var userManager = SharedFunctions.InitialiseUserManager();
                var mockObsRepo = new Mock<IObservationRepository>();
                mockObsRepo.SetupSequence(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(new QueryResult<Observation>())
                    .ReturnsAsync(GetQueryResult(length));

                var requestFilter = ObservationFeedFilter.Network;

                var controller = new ObservationFeedController(_mapper, _logger.Object, userManager, mockObsRepo.Object, _mockProfilePhotosService.Object);

                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
                };

                // Act
                var result = await controller.GetObservationsFeedAsync(1, requestFilter);

                // Assert
                var returnFilter = ObservationFeedFilter.Public;

                var objectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
                var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
                Assert.Equal(length, actual.TotalItems);
                Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
                Assert.Equal(returnFilter, actual.ReturnFilter);
            }
        }


        #endregion


        #region When request Filter == Public

        [Fact]
        public async Task GetObservationsFeedAsync_FilterPublic_ReturnsNotFound_WhenRepoReturnsNull()
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<QueryResult<Observation>>(null));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Public;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            string expectedMessage = $"Observations not found";

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal(expectedMessage, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(45)]
        public async Task GetObservationsFeedAsync_FilterPublic_ReturnsPublic_OnSuccess(int length)
        {
            // Arrange
            var requestingUser = SharedFunctions.GetUser("Any");

            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GetQueryResult(length));

            var controller = new ObservationFeedController(_mapper, _logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
            };

            var filter = ObservationFeedFilter.Public;

            // Act
            var result = await controller.GetObservationsFeedAsync(1, filter);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var actual = Assert.IsType<ObservationFeedDto>(objectResult.Value);
            Assert.Equal(length, actual.TotalItems);
            Assert.IsType<ObservationViewModel>(actual.Items.FirstOrDefault());
            Assert.Equal(filter, actual.ReturnFilter);
        }

        #endregion


        private QueryResult<Observation> GetQueryResult(int length)
        {
            var result = new QueryResult<Observation>();
            var bird = new Bird() { BirdId = 1 };

            result.TotalItems = length;
            result.Items = SharedFunctions.GetTestObservations(1, bird);

            return result;
        }
    }
}