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
// using Xunit;

// namespace Birder.Tests.Controller
// {
//     public class Request_Public_Records
//     {
//         private readonly Mock<ILogger<ObservationFeedController>> _logger;
//         private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

//         public Request_Public_Records()
//         {
//             _logger = new Mock<ILogger<ObservationFeedController>>();
//             _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
//         }

//         [Theory]
//         [InlineData(0)]
//         [InlineData(1)]
//         [InlineData(2)]
//         public async Task Returns_OkResult_With_Public_Records(int totalItems)
//         {
//             // Arrange
//             var mockUserManager = SharedFunctions.InitialiseMockUserManager();
//             var mockObsRepo = new Mock<IObservationQueryService>();
//             mockObsRepo.SetupSequence(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
//                 .ReturnsAsync(FeedTestHelpers.GetModel(totalItems));

//             var controller = new ObservationFeedController(_logger.Object, mockUserManager.Object, mockObsRepo.Object);

//             controller.ControllerContext = new ControllerContext()
//             {
//                 HttpContext = new DefaultHttpContext()
//                 { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
//             };

//             // Act
//             var result = await controller.GetObservationsFeedAsync(It.IsAny<int>(), It.IsAny<int>(), ObservationFeedFilter.Public);

//             // Assert
//             var objectResult = Assert.IsType<OkObjectResult>(result);
//             Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
//             var actual = Assert.IsType<ObservationFeedPagedDto>(objectResult.Value);
//             Assert.Equal(totalItems, actual.TotalItems);
//             Assert.IsType<ObservationFeedDto>(actual.Items.FirstOrDefault());
//             Assert.Equal(ObservationFeedFilter.Public, actual.ReturnFilter);
//         }

//         [Fact]
//         public async Task Returns_500_When_Repository_Returns_Null()
//         {
//             // Arrange
//             var mockUserManager = SharedFunctions.InitialiseMockUserManager();
//             var mockObsRepo = new Mock<IObservationQueryService>();
//             mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
//                 .Returns(Task.FromResult<ObservationFeedPagedDto>(null));

//             var controller = new ObservationFeedController(_logger.Object, mockUserManager.Object, mockObsRepo.Object);

//             controller.ControllerContext = new ControllerContext()
//             {
//                 HttpContext = new DefaultHttpContext()
//                 { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
//             };

//             // Act
//             var result = await controller.GetObservationsFeedAsync(1, 10, ObservationFeedFilter.Public);

//             // Assert
//             var objectResult = Assert.IsType<ObjectResult>(result);
//             Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
//             var actual = Assert.IsType<string>(objectResult.Value);
//             Assert.Equal($"Public observations object is null", actual);
//         }
//     }
// }
