using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ObservationController(IMapper mapper
                                   , ApplicationDbContext context
                                   , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Observation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObservationViewModel>>> GetObservations()
        {
            var username = User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(username);

            var observations = await (from o in _context.Observations
                                        .Include(o => o.Bird)
                                        .Include(u => u.ApplicationUser)
                                      where (o.ApplicationUser.UserName == username)
                                      select o).ToListAsync();

            var viewModel = _mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations);

            return Ok(viewModel);
        }

        // GET: api/Observation/5
        [HttpGet, Route("GetObservation")]
        public async Task<ActionResult<ObservationViewModel>> GetObservation(int id)
        {
            var observation = await (from o in _context.Observations
                                        .Include(o => o.Bird)
                                        .Include(u => u.ApplicationUser)
                                      where (o.ObservationId == id)
                                      select o).FirstOrDefaultAsync();

            var viewModel = _mapper.Map<Observation, ObservationViewModel>(observation);

            return Ok(viewModel);
        }

        // POST: api/Observation
        [HttpPost, Route("PostObservation")]
        public async Task<IActionResult> PostObservation(ObservationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newObservation = _mapper.Map<ObservationViewModel, Observation>(model);

                    var username = User.Identity.Name;
                    var user = await _userManager.FindByNameAsync(username);
                    newObservation.ApplicationUser = user;

                    var observedBird = await (from b in _context.Birds
                                              where (b.BirdId == newObservation.BirdId)
                                              select b).FirstOrDefaultAsync();
                    newObservation.Bird = observedBird;

                    // temporary
                    newObservation.LocationLatitude = 0;
                    newObservation.LocationLongitude = 0;
                    //
                    newObservation.CreationDate = DateTime.Now;
                    newObservation.LastUpdateDate = DateTime.Now;


                    _context.Observations.Add(newObservation);
                    await _context.SaveChangesAsync();

                    var test = _mapper.Map<Observation, ObservationViewModel>(newObservation);

                    // test.Bird = null;

                    return Ok(test);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Failed to save a new order: {ex}");
            }

            return BadRequest("Failed to save new order");
        }

        // PUT: api/Observation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObservation(int id, Observation observation)
        {
            if (id != observation.ObservationId)
            {
                return BadRequest();
            }

            _context.Entry(observation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Observation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Observation>> DeleteObservation(int id)
        {
            var observation = await _context.Observations.FindAsync(id);
            if (observation == null)
            {
                return NotFound();
            }

            _context.Observations.Remove(observation);
            await _context.SaveChangesAsync();

            return observation;
        }

        private bool ObservationExists(int id)
        {
            return _context.Observations.Any(e => e.ObservationId == id);
        }
    }
}
