using AutoMapper;
using Birder.Data.Model;
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
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationQueryService _observationQueryService;
        private readonly IBirdThumbnailPhotoService _profilePhotosService;
        private IMemoryCache _cache;

        public ObservationFeedController(IMemoryCache memoryCache
                                       , ILogger<ObservationFeedController> logger
                                       , UserManager<ApplicationUser> userManager
                                       , IObservationQueryService observationQueryService
                                       , IBirdThumbnailPhotoService profilePhotosService)
        {
            _logger = logger;
            _cache = memoryCache;
            _userManager = userManager;
            _observationQueryService = observationQueryService;
            _profilePhotosService = profilePhotosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetObservationsFeedAsync(int pageIndex, ObservationFeedFilter filter)
        {
            try
            {
                if (filter == ObservationFeedFilter.Own)
                {
                    //var userObservations = await _observationRepository.GetObservationsFeedAsync(o => o.ApplicationUser.UserName == User.Identity.Name, pageIndex, pageSize);
                    var modelO = await _observationQueryService.GetPagedObservationsFeedAsync(o => o.ApplicationUser.UserName == User.Identity.Name, pageIndex, pageSize);
                    if (modelO == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetListNotFound, "User Observations list was null at GetObservationsFeedAsync()");
                        return NotFound("Observations not found");
                    }

                    if (modelO.TotalItems > 0 || pageIndex > 1)
                    {
                        _profilePhotosService.GetUrlForObservations(modelO.Items);
                        //var modelO = _mapper.Map<QueryResult<Observation>, ObservationFeedPagedDto>(userObservations);
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

                    //var networkObservations = await _observationRepository.GetObservationsFeedAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), pageIndex, pageSize);
                    var modelN = await _observationQueryService.GetPagedObservationsFeedAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), pageIndex, pageSize);

                    if (modelN == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetListNotFound, "Network observations list is null");
                        return NotFound("Observations not found");
                    }

                    if (modelN.TotalItems > 0 || pageIndex > 1)
                    {
                        _profilePhotosService.GetUrlForObservations(modelN.Items);
                        //var modelN = _mapper.Map<QueryResult<Observation>, ObservationFeedPagedDto>(networkObservations);
                        modelN.ReturnFilter = ObservationFeedFilter.Network;
                        return Ok(modelN);
                    }
                }

                //var publicObservations = await _observationRepository.GetObservationsFeedAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public, pageIndex, pageSize);
                var model = await _observationQueryService.GetPagedObservationsFeedAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public, pageIndex, pageSize);
                
                if (model == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Observations list is null");
                    return NotFound("Observations not found");
                }

                _profilePhotosService.GetUrlForObservations(model.Items);

                //var model = _mapper.Map<QueryResult<Observation>, ObservationFeedPagedDto>(publicObservations);
                model.ReturnFilter = ObservationFeedFilter.Public;
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
                return BadRequest("An unexpected error occurred");
            }
        }
    }
}
