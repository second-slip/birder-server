using AutoMapper;

namespace Birder.Controllers;

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

    [HttpPost, Route("Create")]
    public async Task<IActionResult> CreateObservationAsync(ObservationCreateDto model)
    {
        try
        {
            var observation = _mapper.Map<ObservationCreateDto, Observation>(model);

            var requestingUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (requestingUser is null)
            {
                _logger.LogError(LoggingEvents.GetItem, "requesting user not found");
                return StatusCode(500);
            }
            observation.ApplicationUser = requestingUser;

            var observedBirdSpecies = await _birdRepository.GetAsync(model.Bird.BirdId);

            if (observedBirdSpecies is null)
            {
                _logger.LogError(LoggingEvents.GetItem, $"Bird species with id '{model.Bird.BirdId}' was not found.");
                return StatusCode(500);
            }
            observation.Bird = observedBirdSpecies;

            var createdDate = _systemClock.GetNow;
            observation.CreationDate = createdDate;
            observation.LastUpdateDate = createdDate;

            if (!TryValidateModel(observation, nameof(observation))) // rerun validation on observation model
            {
                _logger.LogError(LoggingEvents.UpdateItem, "Observation model state is invalid: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                return StatusCode(500);
            }

            _observationPositionRepository.Add(observation.Position);
            _observationRepository.Add(observation);
            await _unitOfWork.CompleteAsync();

            return StatusCode(201, new { observationId = observation.ObservationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "an error occurred creating an observation.");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    // todo: this needs to be EXTENSIVELY TESTED -- well mapping profile does
    [HttpPut, Route("update")]
    public async Task<IActionResult> PutObservationAsync(int id, ObservationUpdateDto model)
    {
        if (id != model.ObservationId)
        {
            _logger.LogError(LoggingEvents.UpdateItem, $"Id '{id}' not equal to model id '{model.ObservationId}'");
            return BadRequest("An error occurred (id)");
        }

        try
        {
            var observation = await _observationRepository.GetObservationAsync(id); //, false);

            if (observation is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, $"observation with id '{model.ObservationId}' was not found.");
                return StatusCode(500);
            }

            var username = User.Identity.Name;

            if (username != observation.ApplicationUser.UserName)
            {
                _logger.LogError(LoggingEvents.UpdateItem, $"unauthorised user (not record owner) tried to update the record with id: '{model.ObservationId}'"); // perhaps record requesting user id?
                return StatusCode(401);
            }

            _mapper.Map<ObservationUpdateDto, Observation>(model, observation);

            var bird = await _birdRepository.GetAsync(model.Bird.BirdId);
            if (bird is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, $"the observed bird could not be found for observation with id '{model.ObservationId}'.");
                return StatusCode(500);
            }

            observation.Bird = bird;

            var position = await _observationPositionRepository.SingleOrDefaultAsync(o => o.ObservationId == observation.ObservationId);

            if (position is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, $"the position could not be found for observation with id '{model.ObservationId}'.");
                return StatusCode(500);
            }

            position.Latitude = model.Position.Latitude;
            position.Longitude = model.Position.Longitude;
            position.FormattedAddress = model.Position.FormattedAddress;
            position.ShortAddress = model.Position.ShortAddress;

            observation.LastUpdateDate = _systemClock.GetNow;

            if (!TryValidateModel(observation, nameof(observation)))
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, "observation has an invalid model state: " + ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState), id);
                return StatusCode(500);
            }

            await _unitOfWork.CompleteAsync();

            return StatusCode(200, new { observationId = observation.ObservationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "an error occurred updating (PUT) observation with id: {ID}", id);
            return StatusCode(500);
        }
    }

    [HttpDelete, Route("delete")]
    public async Task<IActionResult> DeleteObservationAsync(int id)
    {
        if (id == 0)
        {
            _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(id)} argument is 0");
            return StatusCode(400);
        }

        try
        {
            var observation = await _observationRepository.GetObservationAsync(id);

            if (observation is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, $"observation with id '{id}' was not found");
                return StatusCode(500);
            }

            var requesterUsername = User.Identity.Name;

            if (requesterUsername != observation.ApplicationUser.UserName)
            {
                _logger.LogError(LoggingEvents.UpdateItem, $"unauthorised user (not record owner) tried to DELETE the record with id: '{observation.ObservationId}'"); // perhaps record requesting user id?
                return StatusCode(401);
            }

            var notes = await _observationNoteRepository.FindAsync(x => x.Observation.ObservationId == id);
            _observationNoteRepository.RemoveRange(notes);

            _observationRepository.Remove(observation);

            await _unitOfWork.CompleteAsync();

            return StatusCode(200, new { observationId = observation.ObservationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, $"an error occurred updating observation with id: {id}");
            return StatusCode(500);
        }
    }
}