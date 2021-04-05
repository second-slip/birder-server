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
    public class ObservationAnalysisController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IObservationsAnalysisService _observationsAnalysisService;

        public ObservationAnalysisController(ILogger<ObservationAnalysisController> logger
                                            , IObservationsAnalysisService observationsAnalysisService)
        {
            _observationsAnalysisService = observationsAnalysisService;
            _logger = logger;
        }

        [HttpGet, Route("GetObservationAnalysis")]
        public async Task<IActionResult> GetObservationAnalysisAsync(string requestedUsername)
        {
            if (string.IsNullOrEmpty(requestedUsername))
            {
                return BadRequest("requestedUsername is missing");
            }

            try
            {
                var viewModel = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestedUsername);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Observations Analysis");
                return StatusCode(500, "an unexpected error occurred");
            }
        }
    }
}
