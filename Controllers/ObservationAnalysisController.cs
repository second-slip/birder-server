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
using System.Data;

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

            if (username == null)
            {
                return Unauthorized();
            }

            var viewModel = await _observationsAnalysisRepository.GetObservationsAnalysis(username);

            return Ok(viewModel);
        }

        [HttpGet, Route("GetTopObservationAnalysis")]
        public async Task<IActionResult> GetTopObservationAnalysis()
        {
            var username = User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            var viewModel = new TopObservationsAnalysisViewModel()
            {
                TopObservations = _observationsAnalysisRepository.GetTopObservations(username),
                TopMonthlyObservations = _observationsAnalysisRepository.GetTopObservations(username, DateTime.Today) // _systemClock.Today)
            };

            return Ok(viewModel);
        }
    }

    public class ObservationAnalysisViewModel
    {
        public int TotalObservationsCount { get; set; }

        public int UniqueSpeciesCount { get; set; }
    }

    public class TopObservationsAnalysisViewModel
    {
        public IEnumerable<TopObservationsViewModel> TopObservations { get; set; }
        public IEnumerable<TopObservationsViewModel> TopMonthlyObservations { get; set; }
    }

    public class TopObservationsViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
