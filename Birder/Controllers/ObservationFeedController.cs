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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationFeedController : ControllerBase
    {
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

        //public enum

        public class Dto
        {
            public int PageIndex { get; set; }
        }
        [HttpGet, Route("Test")]
        public async Task<IActionResult> GetTestAsync(int page)
        {
            var username = User.Identity.Name;

            var userAndTheirNetwork = await _userManager.GetUserWithNetworkAsync(username);

            var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(userAndTheirNetwork.Following);

            followingUsernamesList.Add(username);

            var test = await _observationRepository.GetObs(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), page);

            var pageSize = 10;

            var model = _mapper.Map<QueryResult<Observation>, ObservationFeedDto>(test);
            //model.TotalPages = (int)Math.Ceiling(test.TotalItems / (double)pageSize);

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetObservationsFeedAsync(int page)
        {
            ObservationsFeedFilter filter = ObservationsFeedFilter.UserAndNetwork;
            try
            {
                var username = User.Identity.Name;

                if (filter == ObservationsFeedFilter.User)
                {
                    var userObservations = await _observationRepository.GetObservationsAsync(o => o.ApplicationUser.UserName == username);
                    if (userObservations.Count() > 0) // might have network obs...
                        return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(userObservations));
                }

                if (filter == ObservationsFeedFilter.UserAndNetwork)
                {
                    var userAndTheirNetwork = await _userManager.GetUserWithNetworkAsync(username);

                    var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(userAndTheirNetwork.Following);

                    followingUsernamesList.Add(username);

                    var networkObservations = await _observationRepository.GetPagedObservationsAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), page);
                    if (networkObservations.Count() > 0)
                        return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(networkObservations));
                }

                //string message = "";
                //if (filter != ObservationsFeedFilter.Public)
                //message = "There are no observations in your network.  Showing the latest public observations";

                var publicObservations = await _observationRepository.GetObservationsAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public);

                if (publicObservations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Observations list is null");
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(publicObservations));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
                return BadRequest("An error occurred getting the observations feed.");
            }
        }
    }
}