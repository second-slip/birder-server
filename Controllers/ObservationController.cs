using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Birder.Data;
using Birder.Models;
using Microsoft.AspNetCore.Authorization;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Observation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservations()
        {
            var obs = new List<Observation>();
            var bird = await (from b in _context.Birds
                              where (b.BirdId == 7)
                              select b).FirstOrDefaultAsync();
            
            var ob = new Observation() { ObservationId = 1, Quantity = 1, ObservationDateTime = DateTime.Now, Bird = bird };

            obs.Add(ob);

            var ob2 = new Observation() { ObservationId = 2, Quantity = 5, ObservationDateTime = DateTime.Now, Bird = bird };

            obs.Add(ob2);

            return Ok(obs);

            // return await _context.Observations.ToListAsync();
        }

        // GET: api/Observation/5
        [HttpGet("{id}")]
        [Route("GetObservation")]
        public async Task<ActionResult<Observation>> GetObservation(int id)
        {
            var observation = await _context.Observations.FindAsync(id);
            var bird = await (from b in _context.Birds
                              where (b.BirdId == 7)
                              select b).FirstOrDefaultAsync();
            
            var ob = new Observation() { ObservationId = 1, Quantity = 1, ObservationDateTime = DateTime.Now, Bird = bird };

            // if (observation == null)
            // {
            //     return NotFound();
            // }

            return Ok(ob);

            //return observation;
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

        // POST: api/Observation
        [HttpPost]
        public async Task<ActionResult<Observation>> PostObservation(Observation observation)
        {
            _context.Observations.Add(observation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObservation", new { id = observation.ObservationId }, observation);
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
