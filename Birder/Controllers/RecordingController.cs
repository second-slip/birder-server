using Birder.Helpers;
using Birder.Infrastructure.CustomExceptions;
using Birder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RecordingController : Controller
    {
        private readonly IXenoCantoService _xenoCantoService;
        private readonly ILogger _logger;

        public RecordingController(ILogger<RecordingController> logger, IXenoCantoService xenoCantoService) 
        {
            _logger = logger;
            _xenoCantoService = xenoCantoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecordingsAsync(string species)
        {
            if (string.IsNullOrEmpty(species))
                return BadRequest("location parameter is missing"); // refactor

            try
            {
                var recordings = await _xenoCantoService.GetSpeciesRecordings(species);

                //if (recordings is null)
                //{
                //    _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                //    return StatusCode(500);
                //}

                return Ok(recordings);
            }
            catch (XenoCantoException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return BadRequest($"Xeno Canto Api not found.");
                else
                    return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred");
                return StatusCode(500);
            }
        }
    }
}
