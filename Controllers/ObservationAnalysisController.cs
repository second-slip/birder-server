using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Birder.Data.Repository;
using Birder.ViewModels;

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
}
