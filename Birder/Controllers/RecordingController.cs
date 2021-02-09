using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class RecordingController : Controller
    {
        private readonly ILogger _logger;
        public RecordingController() { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
