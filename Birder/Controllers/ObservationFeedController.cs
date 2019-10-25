using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
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
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;

        public ObservationFeedController(IMapper mapper
                                       , IMemoryCache memoryCache
                                       , ILogger<ObservationController> logger
                                       , UserManager<ApplicationUser> userManager
                                       , IObservationRepository observationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _userManager = userManager;
            _observationRepository = observationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetObservationsFeedAsync(int pageIndex, ObservationFeedFilter filter)
        {
            try
            {
                //var username = User.Identity.Name;
                if (filter == ObservationFeedFilter.Own)
                {
                    var userObservations = await _observationRepository.GetObservationsFeedAsync(o => o.ApplicationUser.UserName == User.Identity.Name, pageIndex, pageSize);
                    if (userObservations.TotalItems > 0 || pageIndex > 1)
                    {
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
                        //logging
                        return NotFound("User not found");
                    }

                    var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(requestingUserAndNetwork.Following);

                    followingUsernamesList.Add(requestingUserAndNetwork.UserName);

                    var networkObservations = await _observationRepository.GetObservationsFeedAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), pageIndex, pageSize);
                    
                    if (networkObservations.TotalItems > 0 || pageIndex > 1)
                    {
                        var modelN = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(networkObservations);
                        if (filter == ObservationFeedFilter.Own)
                        {
                            modelN.ReturnFilter = ObservationFeedFilter.Network;
                            modelN.DisplayMessage = true;
                            modelN.Message = "You have not recorded any observations.  Showing the observations from your network";
                        }
                        modelN.ReturnFilter = ObservationFeedFilter.Network;
                        return Ok(modelN);
                    }
                }

                var publicObservations = await _observationRepository.GetObservationsFeedAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public, pageIndex, pageSize);

                if (publicObservations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Observations list is null");
                    return NotFound();
                }

                var model = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(publicObservations);
                
                if (filter != ObservationFeedFilter.Public)
                {
                    
                    model.DisplayMessage = true;
                    model.Message = "There are no observations in your network.  Showing the latest public observations";
                }
                model.ReturnFilter = ObservationFeedFilter.Public;
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
                return BadRequest("An error occurred getting the observations feed.");
            }
        }
    }
}
