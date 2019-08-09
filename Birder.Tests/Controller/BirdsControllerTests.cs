using AutoMapper;
using Birder.Controllers;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class BirdsControllerTests
    {
        private IMemoryCache _cache;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<BirdsController>> _logger;

        public BirdsControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions()); 
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<BirdsController>>();
        }
    }
}
