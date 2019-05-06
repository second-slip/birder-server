using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly ISystemClock _systemClock;
        private readonly IObservationsAnalysisRepository _observationsAnalysisRepository;

        public ObservationAnalysisController(IObservationsAnalysisRepository observationsAnalysisRepository
                                            , IMemoryCache memoryCache
                                            , ISystemClock systemClock)
        {
            _cache = memoryCache;
            _systemClock = systemClock;
            _observationsAnalysisRepository = observationsAnalysisRepository;

        }

        /*
         * Cache
         * Try / Catch
         * Logging
         */



        [HttpGet, Route("GetObservationAnalysis")]
        public async Task<IActionResult> GetObservationAnalysis()
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                if (_cache.TryGetValue(nameof(ObservationAnalysisViewModel), out ObservationAnalysisViewModel observationAnalysisCache))
                {
                    return Ok(observationAnalysisCache);
                }

                var viewModel = await _observationsAnalysisRepository.GetObservationsAnalysis(username);

                var cacheEntryExpiryDate = TimeSpan.FromDays(1);

                _cache.Set(nameof(ObservationAnalysisViewModel), viewModel, cacheEntryExpiryDate);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("GetTopObservationAnalysis")]
        public IActionResult GetTopObservationAnalysis()
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                _cache.Remove(nameof(TopObservationsAnalysisViewModel));

                if (_cache.TryGetValue(nameof(TopObservationsAnalysisViewModel), out TopObservationsAnalysisViewModel topObservationsCache))
                {
                    return Ok(topObservationsCache);
                }

                var viewModel = await _observationsAnalysisRepository.gtAsync(username, _systemClock.GetToday.AddDays(-30));
                //viewModel.TopObservations = _observationsAnalysisRepository.GetTopObservations(username);
                //viewModel.TopMonthlyObservations = _observationsAnalysisRepository.GetTopObservations(username, _systemClock.GetToday.AddDays(-30));
                //{
                //    TopObservations = _observationsAnalysisRepository.GetTopObservations(username),
                //    // TopMonthlyObservations = _observationsAnalysisRepository.GetTopObservations(username, _systemClock.GetToday.AddDays(-30))
                //};

                var cacheEntryExpiryDate = TimeSpan.FromDays(1);

                // _cache.Set(nameof(TopObservationsAnalysisViewModel), viewModel, cacheEntryExpiryDate);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("GetLifeList")]
        public IActionResult GetLifeList()
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                if (_cache.TryGetValue(nameof(LifeListViewModel), out LifeListViewModel lifeListCache))
                {
                    return Ok(lifeListCache);
                }

                var viewModel = new LifeListViewModel()
                {
                    LifeList = _observationsAnalysisRepository.GetLifeList(username)
                };

                var cacheEntryExpiryDate = TimeSpan.FromDays(1);

                _cache.Set(nameof(LifeListViewModel), viewModel, cacheEntryExpiryDate);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
