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
    public class BirdsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BirdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Birds
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetBirds()
        {
            // TODO: Cache the birds list
            // The birds list is a prime candidate to be put in the cache.
            // The birds list is rarely updated.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var birds = _context.Birds.Where(x => x.BirderStatus == BirderStatus.Common);

            if (birds == null) 
            {
                return BadRequest();
            }

            return Ok(birds);
        }

        [HttpGet]
        [Route("GetBird")]
        public async Task<IActionResult> GetBird(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bird = (from b in _context.Birds
                        where(b.BirdId == id)
                        select b).FirstOrDefault();

            //var bird = await _context.Birds.FindAsync(id);

            if (bird == null)
            {
                return NotFound();
            }

            return Ok(bird);
        }
    }
}
