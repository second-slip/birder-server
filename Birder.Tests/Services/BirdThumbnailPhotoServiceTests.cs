using Birder.Data.Model;
using Birder.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Birder.Tests.Services
{
    public class BirdThumbnailPhotoServiceTests
    {

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

        [Fact]
        public void GetUrlForObservations_OnFlickrServiceError_ReturnsDefaultUrl()
        {
            // Arrange
            var mockCache = new Mock<IMemoryCache>();
            var mockLogger = new Mock<ILogger<BirdThumbnailPhotoService>>();
            var mockFlickrService = new Mock<IFlickrService>();
            mockFlickrService.Setup(serve => serve.GetThumbnailUrl(It.IsAny<string>()))
                .Throws(new InvalidOperationException());

            var service = new BirdThumbnailPhotoService(mockCache.Object, mockLogger.Object, mockFlickrService.Object);

            const string expected = "https://farm1.staticflickr.com/908/28167626118_f9ed3a67cf_q.png";
            var observations = new List<Observation> { new Observation() { Bird = new Bird() } };

            // Act
            var result = service.GetUrlForObservations(observations);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Observation>>(result);
            Assert.Equal(expected, result.FirstOrDefault().Bird.ThumbnailUrl);
        }

        [Fact]
        public void GetUrlForObservations_OnFlickrServiceSuccess_ReturnsUrl()
        {
            // Arrange
            const string expected = "https://testUrl.png";
            var concreteCache = new MemoryCache(new MemoryCacheOptions());
            var mockLogger = new Mock<ILogger<BirdThumbnailPhotoService>>();
            var mockFlickrService = new Mock<IFlickrService>();
            mockFlickrService.Setup(serve => serve.GetThumbnailUrl(It.IsAny<string>()))
                .Returns(expected);

            var service = new BirdThumbnailPhotoService(concreteCache, mockLogger.Object, mockFlickrService.Object);

            var observations = new List<Observation> { new Observation() { Bird = new Bird() } };

            // Act
            var result = service.GetUrlForObservations(observations);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Observation>>(result);
            Assert.Equal(expected, result.FirstOrDefault().Bird.ThumbnailUrl);
        }



        #endregion




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
    }
}
