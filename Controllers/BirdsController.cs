using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Birder.Data;
using Microsoft.AspNetCore.Authorization;
using Birder.Data.Model;
using AutoMapper;
using Birder.ViewModels;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BirdsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public BirdsController(IMapper mapper
                             , ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Birds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bird>>> GetBirds()
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

        // GET: api/Birds/GetBirdGetBird?id={x}
        [HttpGet, Route("GetBird")]
        //public async Task<ActionResult<Bird>> GetBird(int id)
        public async Task<IActionResult> GetBird(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // nb lazy load Observations (see separate action)
            var bird = await (from b in _context.Birds
                                 .Include(cs => cs.BirdConserverationStatus)
                              where(b.BirdId == id)
                              select b).FirstOrDefaultAsync();

            if (bird == null)
            {
                return NotFound();
            }

            var viewmodel = _mapper.Map<Bird, BirdSummaryViewModel>(bird);

            return Ok(viewmodel);
        }
    }
}
