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

        [HttpGet, Route("GetObservation")]
        public async Task<IActionResult> GetObservationAsync(int id)
        {
            try
            {
                var observation = await _observationRepository.GetObservationAsync(id, true);

                if (observation == null)
                {
                    string message = $"Observation with id '{id}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, message);
                    return NotFound(message);
                }

                return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting observation with id '{id}'.";
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, message);
                return BadRequest("An error occurred");
            }
        }


        [HttpGet, Route("GetObservationsBySpecies")]
        public async Task<IActionResult> GetObservationsBySpeciesAsync(int birdId)
        {
            try
            {
                var observations = await _observationRepository.GetObservationsAsync(cs => cs.BirdId == birdId);

                if (observations == null)
                {
                    string message = $"Observations with birdId '{birdId}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                    return NotFound(message);
                }

                return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting Observations with birdId '{birdId}'.";
                _logger.LogError(LoggingEvents.GetListNotFound, ex, message);
                return BadRequest("An error occurred");
            }
        }


        [HttpPost, Route("CreateObservation")]
        public async Task<IActionResult> CreateObservationAsync(ObservationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "ObservationViewModel is invalid: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("An error occurred");
                }

                var requestingUser = await _userManager.FindByNameAsync(User.Identity.Name);

                if (requestingUser == null)
                {
                    _logger.LogError(LoggingEvents.GetItem, "Requesting user not found");
                    return NotFound("Requesting user not found");
                }

                var observedBirdSpecies = await _birdRepository.GetBirdAsync(model.BirdId);

                if (observedBirdSpecies == null)
                {
                    string message = $"Bird species with id '{model.BirdId}' was not found.";
                    _logger.LogError(LoggingEvents.GetItem, message);
                    return NotFound(message);
                }

                var observation = _mapper.Map<ObservationViewModel, Observation>(model);
                observation.ApplicationUser = requestingUser;
                observation.Bird = observedBirdSpecies;
                observation.CreationDate = _systemClock.GetNow;
                observation.LastUpdateDate = observation.CreationDate;

                TryValidateModel(observation);
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "Observation model state is invalid: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("An error occurred");
                }

                _observationRepository.Add(observation);
                await _unitOfWork.CompleteAsync();

                _cache.Remove(CacheEntries.ObservationsList);
                return CreatedAtAction(nameof(CreateObservationAsync), _mapper.Map<Observation, ObservationViewModel>(observation));
                //return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred creating an observation.");
                return BadRequest("An unexpected error occurred.");
            }
        }

        [HttpPut, Route("UpdateObservation")]
        public async Task<IActionResult> PutObservationAsync(int id, ObservationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "ObservationViewModel is invalid: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("An error occurred");
                }

                if (id != model.ObservationId)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, $"Id '{id}' not equal to model id '{model.ObservationId}'");
                    return BadRequest("An error occurred (id)");
                }

                var observation = await _observationRepository.GetObservationAsync(id, true);
                if (observation == null)
                {
                    string message = $"Observation with id '{model.ObservationId}' was not found.";
                    _logger.LogError(LoggingEvents.UpdateItem, message);
                    return NotFound(message);
                }

                var username = User.Identity.Name;

                if (username != observation.ApplicationUser.UserName)
                {
                    return Unauthorized("Requesting user is not allowed to edit this observation");
                }

                _mapper.Map<ObservationViewModel, Observation>(model, observation);

                observation.Bird = await _birdRepository.GetBirdAsync(model.BirdId);

                observation.LastUpdateDate = _systemClock.GetNow;

                TryValidateModel(observation);
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Observation has an invalid model state: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState), id);
                    return BadRequest("An error occurred");
                }

                await _unitOfWork.CompleteAsync();

                _cache.Remove(CacheEntries.ObservationsList);

                return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred updating (PUT) observation with id: {ID}", id);
                return BadRequest("An unexpected error occurred");
            }
        }

        [HttpDelete, Route("DeleteObservation")]
        public async Task<IActionResult> DeleteObservationAsync(int id)
        {
            try
            {
                var observation = await _observationRepository.GetAsync(id);
                
                if (observation == null)
                {
                    string message = $"Observation with id '{id}' not found";
                    _logger.LogError(LoggingEvents.UpdateItem, message);
                    return NotFound(message);
                }

                var requesterUsername = User.Identity.Name;

                if (requesterUsername != observation.ApplicationUser.UserName)
                {
                    return Unauthorized("Requesting user is not allowed to delete this observation");
                }

                _observationRepository.Remove(observation);
                
                await _unitOfWork.CompleteAsync();

                _cache.Remove(CacheEntries.ObservationsList);

                return Ok(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"An error occurred updating observation with id: {id}");
                return BadRequest("An unexpected error occurred");
            }
        }
    }
}
