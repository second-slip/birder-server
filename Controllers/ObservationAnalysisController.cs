using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ISystemClock _systemClock;
        private readonly IObservationRepository _observationRepository;

        public ObservationAnalysisController(IObservationRepository observationRepository
                                            , IMemoryCache memoryCache
                                            , ISystemClock systemClock, IMapper mapper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _systemClock = systemClock;
            _observationRepository = observationRepository;
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

                if (_cache.TryGetValue("Observations", out IEnumerable<Observation> observationsCache))
                {
                    return Ok(_mapper.Map<IEnumerable<Observation>, ObservationAnalysisViewModel>(observationsCache));
                }

                var observations = await _observationRepository.ObservationsWithBird(x => x.ApplicationUser.UserName == username);

                _cache.Set("Observations", observations, _systemClock.GetEndOfToday);

                var viewModel = _mapper.Map<IEnumerable<Observation>, ObservationAnalysisViewModel>(observations);

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

                if (_cache.TryGetValue("Observations", out IEnumerable<Observation> observationsCache))
                {
                   return Ok(_mapper.Map<IEnumerable<Observation>, TopObservationsAnalysisViewModel>(observationsCache, opt => opt.Items["Date"] = _systemClock.GetToday.AddDays(-30)));
                }

                var observations = await _observationRepository.ObservationsWithBird(a => a.ApplicationUser.UserName == username);

                _cache.Set("Observations", observations, _systemClock.GetEndOfToday);

                var date = _systemClock.GetToday.AddDays(-30);

                var viewModel = _mapper.Map<IEnumerable<Observation>, TopObservationsAnalysisViewModel>(observations, opt => opt.Items["Date"] = date);

                // var viewModel = new TopObservationsAnalysisViewModel();

                // viewModel.TopObservations = observations
                //     .GroupBy(n => n.Bird)
                //     .Select(n => new TopObservationsViewModel
                //     {
                //         BirdId = n.Key.BirdId,
                //         Name = n.Key.EnglishName,
                //         Count = n.Count()
                //     }).OrderByDescending(n => n.Count).Take(5);

                // viewModel.TopMonthlyObservations = observations
                //     .Where(o => o.ObservationDateTime >= date)
                //     .GroupBy(n => n.Bird)
                //     .Select(n => new TopObservationsViewModel
                //     {
                //         BirdId = n.Key.BirdId,
                //         Name = n.Key.EnglishName,
                //         Count = n.Count()
                //     }).OrderByDescending(n => n.Count).Take(5);

               return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("GetLifeList")]
        public async Task<IActionResult> GetLifeList()
        {
            try
            {
                var username = User.Identity.Name;

                if (username == null)
                {
                    return Unauthorized();
                }

                // _cache.Remove(nameof(LifeListViewModel));

                if (_cache.TryGetValue("Observations", out IEnumerable<Observation> observationsCache))
                {
                    var viewModelCache = observationsCache
                        .GroupBy(n => n.Bird)
                        .Select(n => new LifeListViewModel
                        {
                            BirdId = n.Key.BirdId,
                            EnglishName = n.Key.EnglishName,
                            Species = n.Key.Species,
                            PopulationSize = n.Key.PopulationSize,
                            BtoStatusInBritain = n.Key.BtoStatusInBritain,
                            ConservationStatus = n.Key.BirdConservationStatus.ConservationList,
                            Count = n.Count()
                        }).OrderByDescending(n => n.Count);
                    return Ok(viewModelCache);
                }

                var observations = await _observationRepository.ObservationsWithBird(a => a.ApplicationUser.UserName == username);

                _cache.Set("Observations", observations, _systemClock.GetEndOfToday);

                var viewModel = observations
                    .GroupBy(n => n.Bird)
                    .Select(n => new LifeListViewModel
                            {
                                BirdId = n.Key.BirdId,
                                EnglishName = n.Key.EnglishName,
                                Species = n.Key.Species,
                                PopulationSize = n.Key.PopulationSize,
                                BtoStatusInBritain = n.Key.BtoStatusInBritain,
                                ConservationStatus = n.Key.BirdConservationStatus.ConservationList,
                                Count = n.Count()
                            }).OrderByDescending(n => n.Count);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
