// using Birder.Controllers;
// using Birder.Data;
// using Birder.Data.Model;
// using Birder.Services;
// using Birder.TestsHelpers;
// using Birder.ViewModels;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Moq;
// using System;
// using System.Linq;
// using System.Linq.Expressions;
// using System.Threading.Tasks;
// using TestSupport.EfHelpers;
// using Xunit;
// using Xunit.Extensions.AssertExtensions;

// namespace Birder.Tests.Controller
// {
//     public class Request_Network_Records
//     {
//         private readonly Mock<ILogger<ObservationFeedController>> _logger;
//         private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

//         public Request_Network_Records()
//         {
//             _logger = new Mock<ILogger<ObservationFeedController>>();
//             _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
//         }

//         [Theory]
//         [InlineData(0, 2)]
//         [InlineData(1, 1)]
//         [InlineData(1, 2)]
//         public async Task Returns_OkResult_With_Network_Records(int totalItems, int pageIndex)
//         {

//             var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

//             using (var context = new ApplicationDbContext(options))
//             {
//                 //You have to create the database
//                 context.Database.EnsureClean();
//                 context.Database.EnsureCreated();
//                 context.Users.Add(SharedFunctions.CreateUser("testUser1"));
//                 context.Users.Add(SharedFunctions.CreateUser("testUser2"));

//                 context.SaveChanges();

//                 context.Users.Count().ShouldEqual(2);

//                 // Arrange
//                 var userManager = SharedFunctions.InitialiseUserManager(context);
//                 var requestingUsername = "testUser1";
//                 var mockObsRepo = new Mock<IObservationQueryService>();
//                 mockObsRepo.SetupSequence(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
//                     .ReturnsAsync(FeedTestHelpers.GetModel(totalItems)); 

//                 var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object);

//                 controller.ControllerContext = new ControllerContext()
//                 {
//                     HttpContext = new DefaultHttpContext()
//                     { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
//                 };

//                 // Act
//                 var result = await controller.GetObservationsFeedAsync(pageIndex, pageSize: 10, ObservationFeedFilter.Network);

//                 // Assert
//                 var objectResult = Assert.IsType<OkObjectResult>(result);
//                 Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
//                 var actual = Assert.IsType<ObservationFeedPagedDto>(objectResult.Value);
//                 Assert.Equal(totalItems, actual.TotalItems);
//                 Assert.IsType<ObservationFeedDto>(actual.Items.FirstOrDefault());
//                 Assert.Equal(ObservationFeedFilter.Network, actual.ReturnFilter);
//             }
//         }

//         [Fact]
//         public async Task Returns_OkResult_With_Public_Records_When_No_Network_Records()
//         {
//             var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

//             using (var context = new ApplicationDbContext(options))
//             {
//                 //You have to create the database
//                 context.Database.EnsureClean();
//                 context.Database.EnsureCreated();
//                 context.Users.Add(SharedFunctions.CreateUser("testUser1"));
//                 context.Users.Add(SharedFunctions.CreateUser("testUser2"));

//                 context.SaveChanges();

//                 context.Users.Count().ShouldEqual(2);

//                 // Arrange
//                 int pageIndex = 1;
//                 var userManager = SharedFunctions.InitialiseUserManager(context);
//                 var requestingUsername = "testUser1";
//                 var mockObsRepo = new Mock<IObservationQueryService>();
//                 mockObsRepo.SetupSequence(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
//                     .ReturnsAsync(FeedTestHelpers.GetModel(0)) // no Network records
//                     .ReturnsAsync(FeedTestHelpers.GetModel(1)); // one Public record

//                 var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object);

//                 controller.ControllerContext = new ControllerContext()
//                 {
//                     HttpContext = new DefaultHttpContext()
//                     { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
//                 };

//                 // Act
//                 var result = await controller.GetObservationsFeedAsync(pageIndex, pageSize: 10, ObservationFeedFilter.Network);

//                 // Assert
//                 var objectResult = Assert.IsType<OkObjectResult>(result);
//                 Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
//                 var actual = Assert.IsType<ObservationFeedPagedDto>(objectResult.Value);
//                 Assert.Equal(1, actual.TotalItems);
//                 Assert.IsType<ObservationFeedDto>(actual.Items.FirstOrDefault());
//                 Assert.Equal(ObservationFeedFilter.Public, actual.ReturnFilter);
//             }
//         }


//         [Fact]
//         public async Task Returns_500_When_Requesting_User_Is_Null()
//         {
//             var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

//             using (var context = new ApplicationDbContext(options))
//             {
//                 //You have to create the database
//                 context.Database.EnsureClean();
//                 context.Database.EnsureCreated();
//                 context.Users.Add(SharedFunctions.CreateUser("testUser1"));
//                 context.Users.Add(SharedFunctions.CreateUser("testUser2"));

//                 context.SaveChanges();

//                 context.Users.Count().ShouldEqual(2);

//                 // Arrange
//                 var userManager = SharedFunctions.InitialiseUserManager(context);
//                 var requestingUsername = "Does not exist";
//                 var mockObsRepo = new Mock<IObservationQueryService>();
//                 var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object);

//                 controller.ControllerContext = new ControllerContext()
//                 {
//                     HttpContext = new DefaultHttpContext()
//                     { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
//                 };

//                 // Act
//                 var result = await controller.GetObservationsFeedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ObservationFeedFilter>());

//                 // Assert
//                 var objectResult = Assert.IsType<ObjectResult>(result);
//                 Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
//                 var actual = Assert.IsType<string>(objectResult.Value);
//                 Assert.Equal("requesting user not found", actual);
//             }
//         }

//         [Fact]
//         public async Task Returns_500_When_Repository_Returns_Null()
//         {
//             var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

//             using (var context = new ApplicationDbContext(options))
//             {
//                 //You have to create the database
//                 context.Database.EnsureClean();
//                 context.Database.EnsureCreated();
//                 context.Users.Add(SharedFunctions.CreateUser("testUser1"));
//                 context.Users.Add(SharedFunctions.CreateUser("testUser2"));

//                 context.SaveChanges();

//                 context.Users.Count().ShouldEqual(2);

//                 // Arrange
//                 var userManager = SharedFunctions.InitialiseUserManager(context);
//                 var requestingUsername = "testUser1";

//                 var mockObsRepo = new Mock<IObservationQueryService>();
//                 mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
//                     .Returns(Task.FromResult<ObservationFeedPagedDto>(null));

//                 var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object);

//                 controller.ControllerContext = new ControllerContext()
//                 {
//                     HttpContext = new DefaultHttpContext()
//                     { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
//                 };

//                 // Act
//                 var result = await controller.GetObservationsFeedAsync(It.IsAny<int>(), It.IsAny<int>(), ObservationFeedFilter.Network);

//                 // Assert
//                 var objectResult = Assert.IsType<ObjectResult>(result);
//                 Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
//                 var actual = Assert.IsType<string>(objectResult.Value);
//                 Assert.Equal($"Network observations object is null", actual);
//             }
//         }
//     }
// }
