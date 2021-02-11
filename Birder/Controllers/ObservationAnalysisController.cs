using AutoMapper;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationAnalysisController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ISystemClockService _systemClock;
        private readonly IObservationRepository _observationRepository;
        private readonly IObservationsAnalysisService _observationsAnalysisService;

        public ObservationAnalysisController(IObservationRepository observationRepository
                                            , ILogger<ObservationAnalysisController> logger
                                            , IMemoryCache memoryCache
                                            , ISystemClockService systemClock
                                            , IMapper mapper
                                            , IObservationsAnalysisService observationsAnalysisService)
        {
            _observationsAnalysisService = observationsAnalysisService;
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _systemClock = systemClock;
            _observationRepository = observationRepository;
        }

        [HttpGet, Route("GetObservationAnalysis")]
        public async Task<IActionResult> GetObservationAnalysisAsync()
        {
            try
            {
                var username = User.Identity.Name;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized();
                }

                //if (_cache.TryGetValue(CacheEntries.ObservationsList, out IEnumerable<Observation> observationsCache))
                //{
                //    return Ok(_mapper.Map<IEnumerable<Observation>, ObservationAnalysisViewModel>(observationsCache));
                //}

                // CACHE FAULT 4/7/20  **************************************
                // CACHE WITH USER ID OR USERNAME OR SOMETHING UNIQUE
                // GET RID -- ONLY CALLS WHEN USER NAVIGATES AWAY FROM VIEW WITH ANALYSIS SIDEBAR
                // if (_cache.TryGetValue(CacheEntries.ObservationsSummary, out ObservationAnalysisViewModel observationsSummaryCache))
                // {
                //     return Ok(observationsSummaryCache);
                // }

                // var observations = await _observationRepository.GetObservationsAsync(x => x.ApplicationUser.UserName == username);

                //_cache.Set(CacheEntries.ObservationsList, observations, _systemClock.GetEndOfToday);

                // var viewModel = _mapper.Map<IEnumerable<Observation>, ObservationAnalysisViewModel>(observations);

                var viewModel = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == username);

                // _cache.Set(CacheEntries.ObservationsSummary, viewModel, _systemClock.GetEndOfToday);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Observations Analysis");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("GetTopObservationAnalysis")]
        public async Task<IActionResult> GetTopObservationAnalysisAsync()
        {
            try
            {
                var username = User.Identity.Name;

                if (username is null)
                {
                    return Unauthorized();
                }

                // CACHE FAULT 4/7/20  **************************************
                // CACHE WITH USER ID OR USERNAME OR SOMETHING UNIQUE
                // GET RID -- ONLY CALLS WHEN USER NAVIGATES AWAY FROM VIEW WITH ANALYSIS SIDEBAR
                // if (_cache.TryGetValue(CacheEntries.ObservationsList, out IEnumerable<Observation> observationsCache))
                // {
                //     var viewModelCache = ObservationsAnalysisHelper.MapTopObservations(observationsCache, _systemClock.GetToday.AddDays(-30));
                //     return Ok(viewModelCache);
                // }

                var observations = await _observationRepository.GetObservationsAsync(a => a.ApplicationUser.UserName == username);

                // observations is null check?

                // _cache.Set(CacheEntries.ObservationsList, observations, _systemClock.GetEndOfToday);

                var viewModel = ObservationsAnalysisHelper.MapTopObservations(observations, _systemClock.GetToday.AddDays(-30));

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Top Observations Analysis");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("GetLifeList")]
        public async Task<IActionResult> GetLifeListAsync()
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                // CACHE FAULT 4/7/20  **************************************
                // CACHE WITH USER ID OR USERNAME OR SOMETHING UNIQUE
                // GET RID -- ONLY CALLS WHEN USER NAVIGATES AWAY FROM VIEW WITH ANALYSIS SIDEBAR
                // if (_cache.TryGetValue(CacheEntries.ObservationsList, out IEnumerable<Observation> observationsCache))
                // {
                //     var viewModelCache = ObservationsAnalysisHelper.MapLifeList(observationsCache);
                //     return Ok(viewModelCache);
                // }

                var observations = await _observationRepository.GetObservationsAsync(a => a.ApplicationUser.UserName == username);

                // _cache.Set(CacheEntries.ObservationsList, observations, _systemClock.GetEndOfToday);

                var viewModel = ObservationsAnalysisHelper.MapLifeList(observations);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Life List");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("GetYearList")]
        public async Task<IActionResult> GetYearListAsync(string year)
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                var observations = await _observationRepository.GetObservationsAsync(a => a.ApplicationUser.UserName == username);

                // _cache.Set(CacheEntries.ObservationsList, observations, _systemClock.GetEndOfToday);

                var viewModel = ObservationsAnalysisHelper.MapLifeList(observations);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Life List");
                return BadRequest("An error occurred");
            }
        }
    }
}
