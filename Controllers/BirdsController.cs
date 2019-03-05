using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Birder.Data;
using Birder.Models;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BirdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Birds
        [HttpGet]
        public async Task<IActionResult> GetBirds()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var birds = new List<Bird>();
            var bird1 = new Bird { BirdId = 1, EnglishName = "Oystercatcher" };
            birds.Add(bird1);
            var bird2 = new Bird { BirdId = 2, EnglishName = "Dipper" };
            birds.Add(bird2);
            var bird3 = new Bird { BirdId = 3, EnglishName = "Robin" };
            birds.Add(bird3);

            // if birds list == 0 does it show null or just empty?
            if (birds == null) 
            {
                return BadRequest();
            }

            return Ok(birds);
            //return _context.Birds;
        }

        // GET: api/Birds/5
        //public async Task<IActionResult> GetBird([FromRoute] int id)
        [HttpGet]
        [Route("GetBird")]
        public async Task<IActionResult> GetBird(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var birds = new List<Bird>();
            var bird1 = new Bird { BirdId = 1, EnglishName = "Oystercatcher" };
            birds.Add(bird1);
            var bird2 = new Bird { BirdId = 2, EnglishName = "Dipper" };
            birds.Add(bird2);
            var bird3 = new Bird { BirdId = 3, EnglishName = "Robin" };
            birds.Add(bird3);

            var bird = (from b in birds
                        where(b.BirdId == id)
                        select b).FirstOrDefault();

            //var bird = await _context.Birds.FindAsync(id);

            if (bird == null)
            {
                return NotFound();
            }

            return Ok(bird);
        }

        // PUT: api/Birds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBird([FromRoute] int id, [FromBody] Bird bird)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bird.BirdId)
            {
                return BadRequest();
            }

            _context.Entry(bird).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BirdExists(id))
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

        // POST: api/Birds
        [HttpPost]
        public async Task<IActionResult> PostBird([FromBody] Bird bird)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Birds.Add(bird);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBird", new { id = bird.BirdId }, bird);
        }

        // DELETE: api/Birds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBird([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bird = await _context.Birds.FindAsync(id);
            if (bird == null)
            {
                return NotFound();
            }

            _context.Birds.Remove(bird);
            await _context.SaveChangesAsync();

            return Ok(bird);
        }

        private bool BirdExists(int id)
        {
            return _context.Birds.Any(e => e.BirdId == id);
        }
    }
}