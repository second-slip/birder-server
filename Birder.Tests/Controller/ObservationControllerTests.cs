using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Birder.Tests.Controller
{
    public class ObservationControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemClockService _systemClock;
        private readonly IBirdRepository _birdRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;

        public ObservationControllerTests()
        {

        }


    }
}
