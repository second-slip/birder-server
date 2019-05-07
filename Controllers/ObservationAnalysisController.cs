using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Birder.Data.Model;
using AutoMapper;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationAnalysisController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ISystemClock _systemClock;
        private readonly IObservationsAnalysisRepository _observationsAnalysisRepository;

        public ObservationAnalysisController(IObservationsAnalysisRepository observationsAnalysisRepository
                                            , IMemoryCache memoryCache
                                            , ISystemClock systemClock, IMapper mapper)
        {
            _mapper = mapper;
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

                // ToDo: Mapping logic. IEnumerable<Observations> => ObservationAnalysisViewModel

                if (_cache.TryGetValue("Observations",  out IEnumerable<Observation> observationsCache))
                {
                    return Ok(_mapper.Map<IEnumerable<Observation>, ObservationAnalysisViewModel>(observationsCache));
                }

                var observations = await _observationsAnalysisRepository.FindAsync(x => x.ApplicationUser.UserName == username);
                // var viewModel = new ObservationAnalysisViewModel();
                var viewModel = _mapper.Map<IEnumerable<Observation>, ObservationAnalysisViewModel>(observations);


                //var viewModel = new ObservationAnalysisViewModel();
                //viewModel.TotalObservationsCount = observations.Count();
                //viewModel.UniqueSpeciesCount = observations.Select(i => i.BirdId).Distinct().Count();

                // var viewModel = await _observationsAnalysisRepository.GetObservationsAnalysis(username);

                 var cacheEntryExpiryDate = TimeSpan.FromDays(1);

                 _cache.Set("Observations", viewModel, cacheEntryExpiryDate);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("GetTopObservationAnalysis")]
        public async Task<IActionResult> GetTopObservationAnalysisAsync()
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                if (_cache.TryGetValue(nameof(TopObservationsAnalysisViewModel), out TopObservationsAnalysisViewModel topObservationsCache))
                {
                    return Ok(topObservationsCache);
                }

                var viewModel = await _observationsAnalysisRepository.gtAsync(username, _systemClock.GetToday.AddDays(-30));
                //viewModel.TopObservations = _observationsAnalysisRepository.GetTopObservations(username);

                // var viewModel = new TopObservationsAnalysisViewModel
                // {
                //     TopObservations = _observationsAnalysisRepository.GetTopObservations(username),
                //     TopMonthlyObservations = _observationsAnalysisRepository.GetTopObservations(username, _systemClock.GetToday.AddDays(-30))
                // };

                var cacheEntryExpiryDate = TimeSpan.FromDays(1);

                _cache.Set(nameof(TopObservationsAnalysisViewModel), viewModel, cacheEntryExpiryDate);

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

                // _cache.Remove(nameof(LifeListViewModel));

                if (_cache.TryGetValue(nameof(LifeListViewModel), out LifeListViewModel lifeListCache))
                {

                    return Ok(lifeListCache);
                }

                var viewModel = new LifeListViewModel()
                {
                    LifeList = _observationsAnalysisRepository.GetLifeList(username)
                };

                // var cacheEntryExpiryDate = TimeSpan.FromDays(1);

                // var t = viewModel.GetType();
                // var y = viewModel.LifeList.GetType();

                // _cache.Set(nameof(LifeListViewModel), viewModel, cacheEntryExpiryDate);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
