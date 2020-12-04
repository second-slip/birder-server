﻿using AutoMapper;
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
using System.Collections.Generic;
using System.Linq;

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
        private readonly IObservationPositionRepository _observationPositionRepository;
        private readonly IObservationNoteRepository _observationNoteRepository;
        //private readonly IProfilePhotosService _profilePhotosService;

        public ObservationController(IMapper mapper
                                   , IMemoryCache memoryCache
                                   , ISystemClockService systemClock
                                   , IUnitOfWork unitOfWork
                                   , IBirdRepository birdRepository
                                   , ILogger<ObservationController> logger
                                   , UserManager<ApplicationUser> userManager
                                   , IObservationRepository observationRepository
                                   , IObservationPositionRepository observationPositionRepository
                                   , IObservationNoteRepository observationNoteRepository)
                                   //, IProfilePhotosService profilePhotosService)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _systemClock = systemClock;
            _birdRepository = birdRepository;
            _observationRepository = observationRepository;
            _observationPositionRepository = observationPositionRepository;
            _observationNoteRepository = observationNoteRepository;
            //_profilePhotosService = profilePhotosService;
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

                return Ok(_mapper.Map<Observation, ObservationDto>(observation));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting observation with id '{id}'.";
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, message);
                return BadRequest("An error occurred");
            }
        }


        [HttpGet, Route("GetObservationsByBirdSpecies")]
        public async Task<IActionResult> GetObservationsByBirdSpeciesAsync(int birdId, int pageIndex, int pageSize)
        {
            try
            {
                var observations = await _observationRepository.GetPagedObservationsAsync(cs => cs.BirdId == birdId, pageIndex, pageSize);

                if (observations == null)
                {
                    string message = $"Observations with birdId '{birdId}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                    return NotFound(message);
                }

                return Ok(_mapper.Map<QueryResult<Observation>, ObservationFeedDto>(observations));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting Observations with birdId '{birdId}'.";
                _logger.LogError(LoggingEvents.GetListNotFound, ex, message);
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("GetObservationsByUser")]
        public async Task<IActionResult> GetObservationsByUserAsync(string username, int pageIndex, int pageSize)
        {
            try
            {
                var observations = await _observationRepository.GetPagedObservationsAsync(o => o.ApplicationUser.UserName == username, pageIndex, pageSize);

                if (observations == null)
                {
                    string message = $"Observations with username '{username}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                    return NotFound(message);
                }

                //_profilePhotosService.GetThumbnailsUrl(observations.Items);

                return Ok(_mapper.Map<QueryResult<Observation>, ObservationFeedDto>(observations));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting observations with username '{username}'.";
                _logger.LogError(LoggingEvents.GetListNotFound, ex, message);
                return BadRequest("An error occurred");
            }
        }

        [HttpPost, Route("CreateObservation")]
        public async Task<IActionResult> CreateObservationAsync(ObservationDto model)
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

                var observedBirdSpecies = await _birdRepository.GetBirdAsync(model.Bird.BirdId);

                if (observedBirdSpecies == null)
                {
                    string message = $"Bird species with id '{model.BirdId}' was not found.";
                    _logger.LogError(LoggingEvents.GetItem, message);
                    return NotFound(message);
                }

                var observation = _mapper.Map<ObservationDto, Observation>(model);

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

                _observationPositionRepository.Add(observation.Position);
                _observationRepository.Add(observation);
                _observationNoteRepository.AddRange(observation.Notes);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(CreateObservationAsync), _mapper.Map<Observation, ObservationDto>(observation));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred creating an observation.");
                return BadRequest("An unexpected error occurred.");
            }
        }

        [HttpPut, Route("UpdateObservation")]
        public async Task<IActionResult> PutObservationAsync(int id, ObservationEditDto model)
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

                var observation = await _observationRepository.GetObservationAsync(id, false);
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

                _mapper.Map<ObservationEditDto, Observation>(model, observation);

                var bird = await _birdRepository.GetBirdAsync(model.Bird.BirdId);
                observation.Bird = bird;

                var position = await _observationPositionRepository.GetAsync(id);
                position.Latitude = model.Position.Latitude;
                position.Longitude = model.Position.Longitude;
                position.FormattedAddress = model.Position.FormattedAddress;

                //
                var notes = await _observationNoteRepository.FindAsync(x => x.Observation.ObservationId == id);

                //foreach (var item in notes)
                //{
                //    item.Note = model.Notes.First().Note;
                //    item.NoteType = ObservationNoteType.Appearance;
                //}

                //var deleted = from c in notes
                //              where (from o in model.Notes
                //                      select o.Id)
                //              .Contains(c.Id)
                //              select c;

                HashSet<int> oldIds = new HashSet<int>(model.Notes.Select(s => s.Id));

                var results = notes.Where(m => !oldIds.Contains(m.Id));

                var c = results.Count();

                var List2 = notes.Where(item => !model.Notes.Any(item2 => item2.Id == item.Id));

                var c1 = List2.Count();
                //if (deleted.Count() > 0)
                //{
                //    _observationNoteRepository.RemoveRange(deleted);
                //}


                foreach (var item in model.Notes)
                {
                    if(item.Id == 0)
                    {
                        _observationNoteRepository.Add(new ObservationNote()
                        {
                            Id = 0, //
                            NoteType = ObservationNoteType.Appearance,
                            Note = item.Note,
                            Observation = observation
                        });
                    }
                }


                // notes = _mapper.Map<List<ObservationNoteDto>, ICollection<ObservationNote>>(model.Notes);

                // _observationNoteRepository.AddRange(notes);
                // observation.Notes = t;
                // var added = observation.Notes.Where(x => x.Id == 0);
                // if(added.Count() > 0)
                // {
                //     _observationNoteRepository.AddRange(added);
                // }

                // var exIds = (from n in notes
                //                     select n.Id).ToList();







                observation.LastUpdateDate = _systemClock.GetNow;

                //
                TryValidateModel(observation);
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Observation has an invalid model state: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState), id);
                    return BadRequest("An error occurred");
                }

                await _unitOfWork.CompleteAsync();

                return Ok(_mapper.Map<Observation, ObservationEditDto>(observation));

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
                var observation = await _observationRepository.GetObservationAsync(id, false);
                
                if (observation == null)
                {
                    string message = $"Observation with id '{id}' was not found";
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

                return Ok(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"An error occurred updating observation with id: {id}");
                return BadRequest("An unexpected error occurred");
            }
        }

        private void ClearCache()
        {
            _cache.Remove(CacheEntries.ObservationsList);
            _cache.Remove(CacheEntries.ObservationsSummary);
        }
    }
}
