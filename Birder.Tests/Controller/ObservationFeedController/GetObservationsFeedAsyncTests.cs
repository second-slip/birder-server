using Birder.Controllers;
using Birder.Data.Model;
using Birder.Services;
using Birder.TestsHelpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class GetPagedObservationsFeedAsyncTests
    {
        private readonly Mock<ILogger<ObservationFeedController>> _logger;
        private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

        public GetPagedObservationsFeedAsyncTests()
        {
            _logger = new Mock<ILogger<ObservationFeedController>>();
            _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
        }


        [Fact]
        public async Task GetPagedObservationsFeedAsync_ReturnsBadRequest_OnException()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var mockObsRepo = new Mock<IObservationQueryService>();
            mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException());

            var controller = new ObservationFeedController(_logger.Object, mockUserManager.Object, mockObsRepo.Object, _mockProfilePhotosService.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                { User = SharedFunctions.GetTestClaimsPrincipal(string.Empty) }
            };

            // Act
            var result = await controller.GetObservationsFeedAsync(It.IsAny<int>(), It.IsAny<ObservationFeedFilter>());

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal($"An unexpected error occurred", actual);
        }
    }
}
