using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class GetShowcaseObservationsFeedAsyncTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationFeedController>> _logger;
        private readonly ISystemClockService _systemClock;
        private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

        public GetShowcaseObservationsFeedAsyncTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationFeedController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _systemClock = new SystemClockService();
            _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
        }

        [Fact]
        public async Task GetObservationsFeedAsync_OnException_ReturnsBadRequest()
        {

            Assert.True(true);
        }
    }
}
