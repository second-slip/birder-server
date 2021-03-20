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
        //private readonly IListService _listService;
        private readonly ILogger _logger;
        //private readonly ISystemClockService _systemClock;
        private readonly IObservationsAnalysisService _observationsAnalysisService;

        public ObservationAnalysisController(ILogger<ObservationAnalysisController> logger
                                            //, ISystemClockService systemClock
                                            //, IListService listService
                                            , IObservationsAnalysisService observationsAnalysisService)
        {
            _observationsAnalysisService = observationsAnalysisService;
            _logger = logger;
            //_systemClock = systemClock;
            //_listService = listService;
        }

        [HttpGet, Route("GetObservationAnalysis")]
        public async Task<IActionResult> GetObservationAnalysisAsync(string requestedUsername)
        {
            if (string.IsNullOrEmpty(requestedUsername))
            {
                return BadRequest();
            }

            try
            {
                var viewModel = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestedUsername);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Observations Analysis");
                return BadRequest("An error occurred");
            }
        }
    }
}
