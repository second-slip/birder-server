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
    public class ObservationQueryController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IObservationQueryService _observationQueryService;

        public ObservationQueryController(ILogger<ObservationQueryController> logger
                            , IObservationQueryService observationQueryService)
        {
            _logger = logger;
            _observationQueryService = observationQueryService;
        }


        [HttpGet, Route("GetObservationsByBirdSpecies")]
        public async Task<IActionResult> GetObservationsByBirdSpeciesAsync(int birdId, int pageIndex, int pageSize)
        {
            try
            {
                var viewModel = await _observationQueryService.GetPagedObservationsAsync(cs => cs.BirdId == birdId, pageIndex, pageSize);

                if (viewModel == null)
                {
                    string message = $"Observations with birdId '{birdId}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                    return NotFound(message);
                }

                return Ok(viewModel);
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
                var viewModel = await _observationQueryService.GetPagedObservationsAsync(o => o.ApplicationUser.UserName == username, pageIndex, pageSize);

                if (viewModel == null)
                {
                    string message = $"Observations with username '{username}' was not found.";
                    _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                    return NotFound(message);
                }

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                string message = $"An error occurred getting observations with username '{username}'.";
                _logger.LogError(LoggingEvents.GetListNotFound, ex, message);
                return BadRequest("An error occurred");
            }
        }
    }
}
