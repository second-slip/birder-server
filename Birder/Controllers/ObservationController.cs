using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemClockService _systemClock;
        private readonly IBirdRepository _birdRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;

        public ObservationController(IMapper mapper
                                   , IMemoryCache memoryCache
                                   , ISystemClockService systemClock
                                   , IUnitOfWork unitOfWork
                                   , IBirdRepository birdRepository
                                   , ILogger<ObservationController> logger
                                   , UserManager<ApplicationUser> userManager
                                   , IObservationRepository observationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _systemClock = systemClock;
            _birdRepository = birdRepository;
            _observationRepository = observationRepository;
        }

        [HttpGet, Route("GetObservationsBySpecies")]
        public async Task<IActionResult> GetObservationsBySpeciesAsync(int birdId)
        {
            try
            {
                var observations = await _observationRepository.GetObservationsAsync(cs => cs.BirdId == birdId);

                if (observations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "GetBirdObservations({ID}) NOT FOUND", birdId);
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the bird observations list");
                return BadRequest();
            }
        }

        [HttpGet, Route("GetObservation")]
        public async Task<IActionResult> GetObservationAsync(int id)
        {
            try
            {
                var observation = await _observationRepository.GetObservationAsync(id, true);

                if (observation == null)
                {
                    var message = $"Observation with id '{id}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, message);
                    return NotFound(message);
                }

                return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, $"Observation with id '{id}' was not found.");
                return BadRequest("Observation was not found.");
            }
        }

        [HttpPost, Route("CreateObservation")]
        public async Task<IActionResult> CreateObservationAsync(ObservationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Identity.Name;
                    //user null check

                    // Bird and null check

                    var observation = _mapper.Map<ObservationViewModel, Observation>(model);
                    observation.ApplicationUser = await _userManager.FindByNameAsync(username);
                    observation.Bird = await _birdRepository.GetBirdAsync(model.BirdId);
                    observation.CreationDate = _systemClock.GetNow;
                    observation.LastUpdateDate = observation.CreationDate;

                    TryValidateModel(observation);
                    if (!ModelState.IsValid)
                    {
                        // logging
                        return BadRequest(ModelState);
                    }

                    _observationRepository.Add(observation);
                    await _unitOfWork.CompleteAsync();

                    _cache.Remove(CacheEntries.ObservationsList);

                    return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
                return BadRequest("An error occurred.  Could not add the observation.");
            }
        }

        [HttpPut, Route("UpdateObservation")]
        public async Task<IActionResult> PutObservationAsync(int id, ObservationViewModel model)
        {
            try
            {
                if (id != model.ObservationId)
                {
                    return BadRequest("An error occurred.  Could not edit the observation.");
                }

                if (ModelState.IsValid)
                {
                    var observation = await _observationRepository.GetObservationAsync(id, true);
                    if (observation == null)
                    {
                        return NotFound();
                    }

                    var username = User.Identity.Name;

                    if (username != observation.ApplicationUser.UserName)
                    {
                        return BadRequest("An error occurred.  You can only edit your own observations.");
                    }

                    _mapper.Map<ObservationViewModel, Observation>(model, observation);

                    observation.Bird = await _birdRepository.GetBirdAsync(model.BirdId);

                    observation.LastUpdateDate = _systemClock.GetNow;

                    TryValidateModel(observation);
                    if (!ModelState.IsValid)
                    {
                        // logging
                        return BadRequest(ModelState);
                    }

                    await _unitOfWork.CompleteAsync();

                    _cache.Remove(CacheEntries.ObservationsList);

                    return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));

                    //try
                    //{
                    //    await _observationRepository.UpdateObservation(observation);
                    //}
                    //catch (DbUpdateConcurrencyException)
                    //{
                    //    if (await _observationRepository.ObservationExists(id)) // !ObservationExists(id))
                    //    {
                    //        return NotFound();
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}

                }
                return BadRequest("An error occurred.  Could not edit the observation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred updating (PUT) observation with id: {ID}", id);
                return BadRequest("An error occurred.  Could not edit the observation.");
            }
        }

        [HttpDelete, Route("DeleteObservation")]
        public async Task<IActionResult> DeleteObservationAsync(int id)
        {
            var observation = await _observationRepository.GetAsync(id);
            if (observation == null)
            {
                return NotFound();
            }

            _observationRepository.Remove(observation);
            await _unitOfWork.CompleteAsync();

            _cache.Remove(CacheEntries.ObservationsList);

            return Ok(id);
        }
    }
}
