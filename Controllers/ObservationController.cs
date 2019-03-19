using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//TODO: Observation controller
//    - tidy up return messages, et cetera
//    - repository
//    - logging

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
        private readonly IObservationRepository _observationRepository;

        public ObservationController(IMapper mapper
                                   , ApplicationDbContext context
                                   , UserManager<ApplicationUser> userManager
                                   , IObservationRepository observationRepository)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _observationRepository = observationRepository;
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

                    // // temporary
                    // newObservation.LocationLatitude = 54.972237;
                    // newObservation.LocationLongitude = -2.460856;
                    // //
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

            return BadRequest("An error occurred.  Could not add the observation.");
        }

        // PUT: api/Observation/5
        [HttpPut, Route("UpdateObservation")]
        public async Task<IActionResult> PutObservation(int id, ObservationViewModel model)
        {
            try
            {
                if (id != model.ObservationId)
                {
                    return BadRequest("An error occurred.  Could not edit the observation.");
                }

                if (ModelState.IsValid)
                {
                    //var editedObservation = _mapper.Map<ObservationViewModel, Observation>(model);

                    var username = User.Identity.Name;
                    var user = await _userManager.FindByNameAsync(username);

                    // we need to get the server observation anyway to check users are the same
                    var observation = await (from o in _context.Observations
                                    // .Include(o => o.Bird)
                                    .Include(u => u.ApplicationUser)
                                             where (o.ObservationId == id)
                                             select o).FirstOrDefaultAsync();
                    if (observation == null)
                    {
                        return NotFound();
                    }

                    if (user.Id != observation.ApplicationUser.Id)
                    {
                        return BadRequest("An error occurred.  You can only edit your own observations.");
                    }

                    var observedBird = await (from b in _context.Birds
                                              where (b.BirdId == model.BirdId)
                                              select b).FirstOrDefaultAsync();
                    observation.Bird = observedBird;

                    observation.LocationLatitude = model.LocationLatitude;
                    observation.LocationLongitude = model.LocationLongitude;
                    observation.NoteAppearance = model.NoteAppearance;
                    observation.NoteBehaviour = model.NoteBehaviour;
                    observation.NoteGeneral = model.NoteGeneral;
                    observation.NoteHabitat = model.NoteHabitat;
                    observation.NoteVocalisation = model.NoteVocalisation;
                    observation.NoteWeather = model.NoteWeather;
                    observation.ObservationDateTime = model.ObservationDateTime;
                    observation.Quantity = model.Quantity;

                    observation.LastUpdateDate = DateTime.Now;

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

                    var editedObservation = _mapper.Map<Observation, ObservationViewModel>(observation);

                    return Ok(editedObservation);
                }
            }
            catch (Exception ex)
            {
                // logging
            }
            return BadRequest("An error occurred.  Could not edit the observation.");
        }

        // DELETE: api/Observation/5
        [HttpDelete, Route("DeleteObservation")]
        public async Task<ActionResult<ObservationViewModel>> DeleteObservation(int id)
        {
            var observation = await _context.Observations.FindAsync(id);
            if (observation == null)
            {
                return NotFound();
            }

            _context.Observations.Remove(observation);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
        }

        private bool ObservationExists(int id)
        {
            return _context.Observations.Any(e => e.ObservationId == id);
        }
    }
}
