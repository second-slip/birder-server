using Birder.Helpers;
using Birder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ListController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IListService _listService;
        private readonly ISystemClockService _systemClock;

        public ListController(ILogger<ListController> logger
                            , ISystemClockService systemClock
                            , IListService listService)
        {
            _logger = logger;
            _systemClock = systemClock;
            _listService = listService;
        }

        [HttpGet, Route("GetTopObservationsList")]
        public async Task<IActionResult> GetTopObservationsListAsync()
        {
            try
            {
                var username = User.Identity.Name;

                if (username is null)
                {
                    return Unauthorized();
                }

                //var observations = await _observationRepository.GetObservationsAsync(a => a.ApplicationUser.UserName == username);

                // observations is null check?

                //var viewModel = ObservationsAnalysisHelper.MapTopObservations(observations, _systemClock.GetToday.AddDays(-30));
                var viewModel = await _listService.GetTopObservationsAsync(username, _systemClock.GetToday.AddDays(-30));

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Top Observations Analysis");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("GetLifeList")]
        public async Task<IActionResult> GetLifeListAsync()
        {
            try
            {
                var username = User.Identity.Name;

                if (username is null)
                {
                    return Unauthorized();
                }

                var viewModel = await _listService.GetLifeListsAsync(a => a.ApplicationUser.UserName == username);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Life List");
                return BadRequest("An error occurred");
            }
        }
    }
}
