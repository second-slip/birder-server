using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class ObservationFeedControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ObservationFeedController>> _logger;
        //private readonly ISystemClockService _systemClock;


        public ObservationFeedControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<ObservationFeedController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            //_systemClock = new SystemClockService();
        }



        [Fact]
        public async Task GetObservationsFeedAsync_ReturnsOkObjectResult_WithOkResult()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            //                .ReturnsAsync(requestingUser);
            var mockObsRepo = new Mock<IObservationRepository>();
            mockObsRepo.Setup(obs => obs.GetObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<QueryResult<Observation>>(null));

        }
    }
}