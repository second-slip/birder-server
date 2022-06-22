using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using System.Collections.Generic;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemClockService _systemClock;
        private readonly IBirdRepository _birdRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;
        private readonly IObservationPositionRepository _observationPositionRepository;
        private readonly IObservationNoteRepository _observationNoteRepository;

        public ObservationController(IMapper mapper
                                   , ISystemClockService systemClock
                                   , IUnitOfWork unitOfWork
                                   , IBirdRepository birdRepository
                                   , ILogger<ObservationController> logger
                                   , UserManager<ApplicationUser> userManager
                                   , IObservationRepository observationRepository
                                   , IObservationPositionRepository observationPositionRepository
                                   , IObservationNoteRepository observationNoteRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _systemClock = systemClock;
            _birdRepository = birdRepository;
            _observationRepository = observationRepository;
            _observationPositionRepository = observationPositionRepository;
            _observationNoteRepository = observationNoteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetObservationAsync(int id)
        {
            try
            {
                var observation = await _observationRepository.GetObservationAsync(id, true);

                if (observation is null)
                {
                    string message = $"Observation with id '{id}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, message);
                    return StatusCode(500, message);
                }

                return Ok(_mapper.Map<Observation, ObservationDto>(observation));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting observation with id '{id}'.";
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, message);
                return StatusCode(500, "an unexpected error occurred");
            }
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> CreateObservationAsync(ObservationAddDto model)
        {
            try
            {
                var requestingUser = await _userManager.FindByNameAsync(User.Identity.Name);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, "requesting user not found");
                    return StatusCode(500, "requesting user not found");
                }

                var observedBirdSpecies = await _birdRepository.GetBirdAsync(model.Bird.BirdId);

                if (observedBirdSpecies is null)
                {
                    string message = $"Bird species with id '{model.BirdId}' was not found.";
                    _logger.LogError(LoggingEvents.GetItem, message);
                    return StatusCode(500, message);
                }

                // date from client model is already Utc....
                // model.ObservationDateTime = model.ObservationDateTime.ToLocalTime();
                // var x = model.ObservationDateTime;
                // var y = model.ObservationDateTime = model.ObservationDateTime.ToLocalTime();
                // var z = model.ObservationDateTime = model.ObservationDateTime.ToUniversalTime();

                var observation = _mapper.Map<ObservationAddDto, Observation>(model);

                DateTime createdDate = _systemClock.GetNow;

                observation.ApplicationUser = requestingUser;
                observation.Bird = observedBirdSpecies;
                observation.CreationDate = createdDate;
                observation.LastUpdateDate = createdDate;

                //rerun validation on observation model
                if (!TryValidateModel(observation, nameof(observation)))
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "Observation model state is invalid: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return StatusCode(500, "observation ModelState is invalid");
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
                return StatusCode(500, "an unexpected error occurred");
            }
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> PutObservationAsync(int id, ObservationEditDto model)
        {
            try
            {
                if (id != model.ObservationId)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, $"Id '{id}' not equal to model id '{model.ObservationId}'");
                    return BadRequest("An error occurred (id)");
                }

                var observation = await _observationRepository.GetObservationAsync(id, false);
                if (observation is null)
                {
                    string message = $"observation with id '{model.ObservationId}' was not found.";
                    _logger.LogError(LoggingEvents.UpdateItem, message);
                    return StatusCode(500, message);
                }

                var username = User.Identity.Name;

                if (username != observation.ApplicationUser.UserName)
                {
                    return Unauthorized("Requesting user is not allowed to edit this observation");
                }

                // date from client model is already Utc....
                // model.ObservationDateTime = model.ObservationDateTime.ToLocalTime();
                // var x = model.ObservationDateTime;
                // var y = model.ObservationDateTime = model.ObservationDateTime.ToLocalTime();
                // var z = model.ObservationDateTime = model.ObservationDateTime.ToUniversalTime();

                _mapper.Map<ObservationEditDto, Observation>(model, observation);

                var bird = await _birdRepository.GetBirdAsync(model.Bird.BirdId);
                if (bird is null)
                {
                    string message = $"The observed bird could not be found for observation with id '{model.ObservationId}'.";
                    _logger.LogError(LoggingEvents.UpdateItem, message);
                    return StatusCode(500, message);
                }

                observation.Bird = bird;

                var position = await _observationPositionRepository.SingleOrDefaultAsync(o => o.ObservationId == observation.ObservationId);
                if (position is null)
                {
                    string message = $"The position could not be found for observation with id '{model.ObservationId}'.";
                    _logger.LogError(LoggingEvents.UpdateItem, message);
                    return StatusCode(500, message);
                }

                position.Latitude = model.Position.Latitude;
                position.Longitude = model.Position.Longitude;
                position.FormattedAddress = model.Position.FormattedAddress;
                position.ShortAddress = model.Position.ShortAddress;

                // ToDo: separate ObservationNotesController to handle this stuff.  
                // ...need to redesign UI first...
                var notes = await _observationNoteRepository.FindAsync(x => x.Observation.ObservationId == id);
              
                var notesDeleted = ObservationNotesHelper.GetDeletedNotes(notes, model.Notes);
                if (notesDeleted.Any())
                {
                    _observationNoteRepository.RemoveRange(notesDeleted);
                }

                var notesAdded = ObservationNotesHelper.GetAddedNotes(model.Notes);
                if (notesAdded.Any())
                {
                    var added = _mapper.Map(notesAdded, new List<ObservationNote>());
                    added.ForEach(a => a.Observation = observation);
                    _observationNoteRepository.AddRange(added);
                }

                // ToDo: is the condition necessary here?
                if (notes.Any())
                {
                    _mapper.Map<List<ObservationNoteDto>, IEnumerable<ObservationNote>>(model.Notes, notes);
                }

                observation.LastUpdateDate = _systemClock.GetNow;

                if (!TryValidateModel(observation, nameof(observation)))
                {
                    _logger.LogError(LoggingEvents.UpdateItemNotFound, "Observation has an invalid model state: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState), id);
                    return StatusCode(500, "observation ModelState is invalid");
                }

                await _unitOfWork.CompleteAsync();

                return Ok(_mapper.Map<Observation, ObservationEditDto>(observation));

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred updating (PUT) observation with id: {ID}", id);
                return StatusCode(500, "an unexpected error occurred");
            }
        }

        [HttpDelete, Route("Delete")]
        public async Task<IActionResult> DeleteObservationAsync(int id)
        {
            try
            {
                var observation = await _observationRepository.GetObservationAsync(id, false);
                
                if (observation is null)
                {
                    string message = $"Observation with id '{id}' was not found";
                    _logger.LogError(LoggingEvents.UpdateItem, message);
                    return StatusCode(500, message);
                }

                var requesterUsername = User.Identity.Name;

                if (requesterUsername != observation.ApplicationUser.UserName)
                {
                    return Unauthorized("Requesting user is not allowed to delete this observation");
                }

                var notes = await _observationNoteRepository.FindAsync(x => x.Observation.ObservationId == id);

                _observationNoteRepository.RemoveRange(notes);

                _observationRepository.Remove(observation);
                
                await _unitOfWork.CompleteAsync();

                return Ok(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"An error occurred updating observation with id: {id}");
                return StatusCode(500, "an unexpected error occurred");
            }
        }
    }
}
