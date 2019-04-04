using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BirdsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBirdRepository _birdRepository;
        private readonly ApplicationDbContext _context;

        public BirdsController(IMapper mapper
                             , IBirdRepository birdRepository
                             , ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
            _birdRepository = birdRepository;
        }

        // GET: api/Birds
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Bird>>> GetBirds()
        public async Task<IActionResult> GetBirds()
        {
            // TODO: Cache the birds list
            // The birds list is a prime candidate to be put in the cache.
            // The birds list is rarely updated.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // var birds = await _context.Birds
            //             .Where(x => x.BirderStatus == BirderStatus.Common)
            //             .ToListAsync();

            var birds = await _birdRepository.GetBirdSummaryList(BirderStatus.Common);

            if (birds == null) 
            {
                return BadRequest();
            }

            var viewmodel = _mapper.Map<List<Bird>, List<BirdDetailViewModel>>(birds);

            return Ok(viewmodel);
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
            var bird = await _birdRepository.GetBirdDetail(id);
            // var bird = await (from b in _context.Birds
            //                      .Include(cs => cs.BirdConserverationStatus)
            //                   where(b.BirdId == id)
            //                   select b).FirstOrDefaultAsync();

            if (bird == null)
            {
                return NotFound();
            }

            var viewmodel = _mapper.Map<Bird, BirdDetailViewModel>(bird);

            return Ok(viewmodel);
        }
    }
}
