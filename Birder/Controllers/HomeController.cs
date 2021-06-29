using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IServerlessDatabaseService _service;
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger, IServerlessDatabaseService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> WakeUpDatabase()
        {
            try
            {
                var result = await _service.GetFirstConservationListStatusAsync();
                return Ok(true);
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2) // connection timeout
                {
                    _logger.LogError(LoggingEvents.SqlServerConnectionTimeoutException, ex, "an sql server connection timeout exception was raised");
                    return StatusCode(500, "an sql server connection timeout error occurred"); 
                }
                _logger.LogError(LoggingEvents.SqlServerException, ex, "an sql server exception was raised");
                return StatusCode(500, "an sql server error occurred");
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.Exception, ex, "an exception was raised");
                return StatusCode(500, "an unexpected error occurred");
            }
        }
    }
}
