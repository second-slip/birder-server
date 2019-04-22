using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
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
        private readonly ISystemClock _systemClock;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBirdRepository _birdRepository;
        private readonly IObservationRepository _observationRepository;

        public ObservationController(IMapper mapper
                                   , ISystemClock systemClock
                                   //, ApplicationDbContext context
                                   , UserManager<ApplicationUser> userManager
                                   , IBirdRepository birdRepository
                                   , IObservationRepository observationRepository)
        {
            _mapper = mapper;
            _systemClock = systemClock;
            //_context = context;
            _userManager = userManager;
            _birdRepository = birdRepository;
            _observationRepository = observationRepository;
        }

        // GET: api/Observation
        [HttpGet]
        public async Task<IActionResult> GetObservations()
        {
            var username = User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(username);

            var observations =_observationRepository.GetUsersObservationsList(username);

            var viewModel = _mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations);

            return Ok(viewModel);
        }

        // GET: api/Observation/5
        [HttpGet, Route("GetObservation")]
        public async Task<IActionResult> GetObservation(int id)
        {
            var observation = await _observationRepository.GetObservationDetail(id);

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

                    //var username = User.Identity.Name;
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    //check if user == null
                    newObservation.ApplicationUser = user;

                    //var observedBird = await (from b in _context.Birds
                    //                          where (b.BirdId == model.BirdId)
                    //                          select b).FirstOrDefaultAsync();
                    var observedBird = await _birdRepository.GetBirdDetail(model.BirdId);
                    newObservation.Bird = observedBird;

                    newObservation.CreationDate = _systemClock.Now;
                    newObservation.LastUpdateDate = _systemClock.Now;

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
                    //var observation = await (from o in _context.Observations
                    //                // .Include(o => o.Bird)
                    //                .Include(u => u.ApplicationUser)
                    //                         where (o.ObservationId == id)
                    //                         select o).FirstOrDefaultAsync();
                    var observation = await _observationRepository.GetObservation(id);
                    if (observation == null)
                    {
                        return NotFound();
                    }

                    if (user.Id != observation.ApplicationUser.Id)
                    {
                        return BadRequest("An error occurred.  You can only edit your own observations.");
                    }

                    var observedBird = await _birdRepository.GetBirdDetail(model.BirdId);
                    // check if bird == null
                    //var observedBird = await (from b in _context.Birds
                    //                          where (b.BirdId == model.BirdId)
                    //                          select b).FirstOrDefaultAsync();
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

                    observation.LastUpdateDate = _systemClock.Now;

                    //_context.Entry(observation).State = EntityState.Modified;

                    try
                    {
                        //await _context.SaveChangesAsync();
                        await _observationRepository.UpdateObservation(observation);
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
            var observation = await _observationRepository.GetObservation(id);
            if (observation == null)
            {
                return NotFound();
            }

            //_context.Observations.Remove(observation);
            var operation = _observationRepository.DeleteObservation(observation);

            //await _context.SaveChangesAsync();
            if (!operation.IsCompletedSuccessfully)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
        }

        private bool ObservationExists(int id)
        {
            return _context.Observations.Any(e => e.ObservationId == id);
        }
    }
}
