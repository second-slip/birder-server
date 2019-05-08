using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ISystemClock _systemClock;
        private readonly IBirdRepository _birdRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;

        public ObservationController(IMapper mapper
                                   , IMemoryCache memoryCache
                                   , ISystemClock systemClock
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

        [HttpGet]
        public async Task<IActionResult> GetObservationsAsync()
        {
            try
            {
                var username = User.Identity.Name;

                var observations = await _observationRepository.GetUsersObservationsList(username);

                if (observations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Observations list is null");
                    return BadRequest();
                }

                var viewModel = _mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
                return BadRequest("An error occurred getting the observations feed.");
            }
        }

        [HttpGet, Route("GetObservation")]
        public async Task<IActionResult> GetObservation(int id)
        {
            try
            {
                var observation = await _observationRepository.GetObservation(id, true);

                if (observation == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, "Observation with id: {ID} was not found.", id);
                    return BadRequest();
                }

                var viewModel = _mapper.Map<Observation, ObservationViewModel>(observation);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Observation with id: {ID} was not found.", id);
                return BadRequest("Observation was not found.");
            }
        }

        [HttpGet, Route("GetBirdObservations")]
        public async Task<IActionResult> GetBirdObservationsAsync(int birdId)
        {
            try
            {
                var observations = await _observationRepository.ObservationsWithBird(cs => cs.BirdId == birdId);

                if (observations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "GetBirdObservations({ID}) NOT FOUND", birdId);
                    return NotFound();
                }

                var viewModel = _mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the bird observations list");
                return BadRequest();
            }
        }

        [HttpPost, Route("CreateObservation")]
        public async Task<IActionResult> CreateObservation(ObservationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Identity.Name;

                    var newObservation = _mapper.Map<ObservationViewModel, Observation>(model);
                    newObservation.ApplicationUser = await _userManager.FindByNameAsync(username);
                    newObservation.Bird = await _birdRepository.GetBird(model.BirdId);
                    newObservation.CreationDate = _systemClock.GetNow;
                    newObservation.LastUpdateDate = _systemClock.GetNow;

                    _observationRepository.Add(newObservation);
                    await _unitOfWork.CompleteAsync();

                    _cache.Remove(CacheEntries.ObservationsList);

                    return Ok(_mapper.Map<Observation, ObservationViewModel>(newObservation));
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
        public async Task<IActionResult> PutObservation(int id, ObservationViewModel model)
        {
            try
            {
                if (id != model.ObservationId)
                {
                    return BadRequest("An error occurred.  Could not edit the observation.");
                }

                if (ModelState.IsValid)
                {
                    var editedObservation = _mapper.Map<ObservationViewModel, Observation>(model);

                    var observation = await _observationRepository.GetObservation(id, true);
                    if (observation == null)
                    {
                        return NotFound();
                    }

                    var username = User.Identity.Name;

                    if (username != observation.ApplicationUser.UserName)
                    {
                        return BadRequest("An error occurred.  You can only edit your own observations.");
                    }

                    if (observation.BirdId != model.BirdId) // only reset Bird if it is changed
                    {
                        observation.Bird = await _birdRepository.GetBird(model.BirdId);
                    }

                    observation.LocationLatitude = model.LocationLatitude;
                    observation.LocationLongitude = model.LocationLongitude;
                    observation.NoteAppearance = model.NoteAppearance;
                    observation.NoteBehaviour = model.NoteBehaviour;
                    observation.NoteGeneral = model.NoteGeneral;
                    observation.NoteHabitat = model.NoteHabitat;
                    observation.NoteVocalisation = model.NoteVocalisation;
                    observation.NoteWeather = model.NoteWeather;
                    observation.ObservationDateTime = model.ObservationDateTime;
                    observation.Quantity = model.Quantity;

                    observation.LastUpdateDate = _systemClock.GetNow;

                    await _unitOfWork.CompleteAsync();

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

                    _cache.Remove(CacheEntries.ObservationsList);

                    //var editedObservation = _mapper.Map<Observation, ObservationViewModel>(observation);

                    return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
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
        public async Task<ActionResult<ObservationViewModel>> DeleteObservation(int id)
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
