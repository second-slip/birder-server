using Birder.Data.Repository;
using Birder.Helpers;
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
        private readonly IHomeRepository _homeRepository;
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> WakeUpDatabase()
        {
            try
            {
                var brilliant = await _homeRepository.GetFirstConservationListStatusAsync();
                return Ok(true);
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    return StatusCode(500, "an sql connection timeout error occurred"); 
                    //Console.WriteLine("Timeout occurred");
                }
                return StatusCode(500, "an sql connection error occurred");
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "an exception was raised");
                return StatusCode(500, "an unexpected error occurred");
            }
        }
    }
}
