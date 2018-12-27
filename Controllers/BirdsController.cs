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
        public IEnumerable<Bird> GetBirds()
        {
            return _context.Birds;
        }

        // GET: api/Birds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBird([FromRoute] int id)
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