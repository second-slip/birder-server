using Birder.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Birder.Tests.Services
{
    public class BirdThumbnailPhotoServiceTests
    {
        //private readonly Mock<IMemoryCache> _mockCache;
        //private readonly Mock<IFlickrService> _mockFlickrService;
        public BirdThumbnailPhotoServiceTests()
        {
            //_systemClockService = new BirdThumbnailPhotoService();
            //_mockCache = new Mock<IMemoryCache>();
            //_mockFlickrService = new Mock<IFlickrService>();
        }

        [Fact]
        public void DetermineCacheEntryId_SingleInt_ReturnsCorrectString()
        {
            // Arrange
            var mockCache = new Mock<IMemoryCache>();
            var mockFlickrService = new Mock<IFlickrService>();
            var mockLogger = new Mock<ILogger<BirdThumbnailPhotoService>>();
            var birdId = 1;

            var service = new BirdThumbnailPhotoService(mockCache.Object, mockLogger.Object, mockFlickrService.Object);

            // Act
            var actual = service.GetCacheEntryKey(birdId);

            // Assert
            Assert.Equal("BirdThumbUrl1", actual);
        }


        [Fact]
        public void AddResponseToCache_info()
        {
            // do not test because we are just testing Microsoft's implementation...
        }

        #region GetUrlForObservations tests

        [Fact]
        public void GetUrlForObservations_NullArgument_ReturnsNullReferenceException()
        {
            // Arrange
            var mockCache = new Mock<IMemoryCache>();
            var mockFlickrService = new Mock<IFlickrService>();
            var mockLogger = new Mock<ILogger<BirdThumbnailPhotoService>>();

            var service = new BirdThumbnailPhotoService(mockCache.Object, mockLogger.Object, mockFlickrService.Object);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            service.GetUrlForObservations(null));

            // Assert
            Assert.Equal("The observations collection is null (Parameter 'observations')", ex.Message);
        }
            



        #endregion
    }
}
