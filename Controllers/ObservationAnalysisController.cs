using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Birder.Data;
using Birder.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Birder.Data.Repository;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationAnalysisController : ControllerBase
    {
        private readonly IObservationsAnalysisRepository _observationsAnalysisRepository;

        public ObservationAnalysisController(IObservationsAnalysisRepository observationsAnalysisRepository)
        {
            _observationsAnalysisRepository = observationsAnalysisRepository;

        }

        [HttpGet, Route("GetObservationAnalysis")]
        public async Task<IActionResult> GetObservationAnalysis()
        {
            var username = User.Identity.Name;

            if (username != null)
            {
                return Unauthorized();
            }

            var viewModel = await _observationsAnalysisRepository.GetObservationsAnalysis(username);

            return Ok(viewModel);
        }       
    }

    public class ObservationAnalysisViewModel
    {
        public int TotalObservationsCount { get; set; }

        public int UniqueSpeciesCount { get; set; }
    }
}
