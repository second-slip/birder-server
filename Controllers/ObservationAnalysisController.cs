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

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationAnalysisController : ControllerBase
    {


        public ObservationAnalysisController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservations()
        {
            //var viewModel = new ObservationsAnalysisDto();
            //viewModel.TotalObservations = await (from observations in _dbContext.Observations
            //                                     where (observations.ApplicationUserId == user.Id)
            //                                     select observations).CountAsync();

            //viewModel.TotalSpecies = await (from observations in _dbContext.Observations
            //                                where (observations.ApplicationUserId == user.Id)
            //                                select observations.BirdId).Distinct().CountAsync();
            //return viewModel;
            return Ok(); // await _context.Observations.ToListAsync();
        }       
    }

    public class ObservationAnalysisViewModel
    {
        public int TotalObservationsCount { get; set; }

        public int UniqueSpeciesCount { get; set; }
    }
}
