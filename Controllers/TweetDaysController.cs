using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Birder.Data;
using Birder.Data.Model;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetDaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TweetDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TweetDays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TweetDay>>> GetTweetDays()
        {
            return await _context.TweetDays.ToListAsync();
        }

        // GET: api/TweetDays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TweetDay>> GetTweetDay(int id)
        {
            var tweetDay = await _context.TweetDays.FindAsync(id);

            if (tweetDay == null)
            {
                return NotFound();
            }

            return tweetDay;
        }

        // PUT: api/TweetDays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTweetDay(int id, TweetDay tweetDay)
        {
            if (id != tweetDay.TweetDayId)
            {
                return BadRequest();
            }

            _context.Entry(tweetDay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetDayExists(id))
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

        // POST: api/TweetDays
        [HttpPost]
        public async Task<ActionResult<TweetDay>> PostTweetDay(TweetDay tweetDay)
        {
            _context.TweetDays.Add(tweetDay);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTweetDay", new { id = tweetDay.TweetDayId }, tweetDay);
        }

        // DELETE: api/TweetDays/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TweetDay>> DeleteTweetDay(int id)
        {
            var tweetDay = await _context.TweetDays.FindAsync(id);
            if (tweetDay == null)
            {
                return NotFound();
            }

            _context.TweetDays.Remove(tweetDay);
            await _context.SaveChangesAsync();

            return tweetDay;
        }

        private bool TweetDayExists(int id)
        {
            return _context.TweetDays.Any(e => e.TweetDayId == id);
        }
    }
}
