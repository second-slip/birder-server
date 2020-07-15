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
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationFeedController : ControllerBase
    {
        private const int pageSize = 10;
        //private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;
        private readonly IBirdThumbnailPhotoService _profilePhotosService;
        private IMemoryCache _cache;
        private readonly ISystemClockService _systemClock;

        public ObservationFeedController(IMapper mapper
                                       , IMemoryCache memoryCache
                                       , ISystemClockService systemClock
                                       , ILogger<ObservationFeedController> logger
                                       , UserManager<ApplicationUser> userManager
                                       , IObservationRepository observationRepository
                                       , IBirdThumbnailPhotoService profilePhotosService)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _systemClock = systemClock;
            _userManager = userManager;
            _observationRepository = observationRepository;
            _profilePhotosService = profilePhotosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetObservationsFeedAsync(int pageIndex, ObservationFeedFilter filter)
        {
            try
            {
                if (filter == ObservationFeedFilter.Own)
                {
                    var userObservations = await _observationRepository.GetObservationsFeedAsync(o => o.ApplicationUser.UserName == User.Identity.Name, pageIndex, pageSize);

                    if (userObservations == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetListNotFound, "User Observations list was null at GetObservationsFeedAsync()");
                        return NotFound("Observations not found");
                    }

                    if (userObservations.TotalItems > 0 || pageIndex > 1)
                    {
                        _profilePhotosService.GetUrlForObservations(userObservations.Items);
                        var modelO = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(userObservations);
                        modelO.ReturnFilter = ObservationFeedFilter.Own;
                        return Ok(modelO);
                    }
                }

                if (filter == ObservationFeedFilter.Network || filter == ObservationFeedFilter.Own)
                {
                    var requestingUserAndNetwork = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);
                    if (requestingUserAndNetwork == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetItemNotFound, "Requesting user not found");
                        return NotFound("Requesting user not found");
                    }

                    var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(requestingUserAndNetwork.Following);

                    followingUsernamesList.Add(requestingUserAndNetwork.UserName);

                    var networkObservations = await _observationRepository.GetObservationsFeedAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), pageIndex, pageSize);

                    if (networkObservations == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetListNotFound, "Network observations list is null");
                        return NotFound("Observations not found");
                    }

                    if (networkObservations.TotalItems > 0 || pageIndex > 1)
                    {
                        _profilePhotosService.GetUrlForObservations(networkObservations.Items);
                        var modelN = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(networkObservations);
                        modelN.ReturnFilter = ObservationFeedFilter.Network;
                        return Ok(modelN);
                    }
                }

                var publicObservations = await _observationRepository.GetObservationsFeedAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public, pageIndex, pageSize);

                if (publicObservations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Observations list is null");
                    return NotFound("Observations not found");
                }

                _profilePhotosService.GetUrlForObservations(publicObservations.Items);

                var model = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(publicObservations);
                model.ReturnFilter = ObservationFeedFilter.Public;
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
                return BadRequest("An unexpected error occurred");
            }
        }

        [AllowAnonymous]
        [HttpGet, Route("GetShowcaseObservationsFeed")]
        public async Task<IActionResult> GetShowcaseObservationsFeedAsync(int quantity)
        {
            try
            {
                if (_cache.TryGetValue(CacheEntries.ShowcaseObservations, out ObservationFeedDto showcaseObservationsCache))
                {
                    return Ok(showcaseObservationsCache);
                }

                var observations = await _observationRepository.GetShowcaseObservationsFeedAsync(obs => obs.Bird.BirderStatus == BirderStatus.Uncommon, quantity);

                if (observations == null)
                {
                    string message = $"Showcase observations not found";
                    _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                    return NotFound(message);
                }

                _profilePhotosService.GetUrlForObservations(observations.Items);

                var showcaseObservations = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(observations);

                _cache.Set(CacheEntries.ShowcaseObservations, showcaseObservations, _systemClock.GetEndOfToday);

                return Ok(showcaseObservations);
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting the showcase observations.";
                _logger.LogError(LoggingEvents.GetListNotFound, ex, message);
                return BadRequest("An unexpected error occurred");
            }
        }
    }
}
