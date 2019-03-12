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
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

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
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservations()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);


            var obs = new List<Observation>();
            var bird = await (from b in _context.Birds
                              where (b.BirdId == 7)
                              select b).FirstOrDefaultAsync();
            
            var ob = new Observation() { ObservationId = 1, Quantity = 1, ObservationDateTime = DateTime.Now, Bird = bird, ApplicationUser = user };
            obs.Add(ob);

            var ob2 = new Observation() { ObservationId = 2, Quantity = 5, ObservationDateTime = DateTime.Now, Bird = bird, ApplicationUser = user };
            obs.Add(ob2);

            var x = _mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(obs);

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
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

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

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
    }

    public class ObservationViewModel
    {
        //ToDo: male/female/juvenile? Or is it too much?
        //[Key]
        public int ObservationId { get; set; }

        //public Geography LocationGeoTest { get; set; } ---> Not supported in EF Core 2.0
        public double LocationLatitude { get; set; }

        public double LocationLongitude { get; set; }

        //[Range(1, 1000, ErrorMessage = "The value must be greater than 0")]
        //[Display(Name = "Individuals")]
        public int Quantity { get; set; }

        //[Display(Name = "General notes")]
        public string NoteGeneral { get; set; }

        //[Display(Name = "Habitat notes")]
        public string NoteHabitat { get; set; }
        // Note plant life, water sources and vegetation conditions, as well as which of the plants the bird is preferring as you observe it.

        //[Display(Name = "Weather notes")]
        public string NoteWeather { get; set; }
        // Note temperature, visibility, wind, light level and any weather conditions that affect your observations.Rain, mist, snowfall, accumulated snow, drought and other factors can impact observations.

        //[Display(Name = "Appearance notes")]
        public string NoteAppearance { get; set; }
        // Take copious notes on the bird's appearance, including the brilliance of plumage, any peculiar markings and any outstanding or unusual features such as missing feathers, leucistic patches or signs of illness. Also record the bird's gender if possible.

        //[Display(Name = "Behaviour notes")]
        public string NoteBehaviour { get; set; }
        // Take notes on what the bird was doing as you observed it.Note general actions and specific reactions to changing conditions, such as the appearance of a predator or how the bird interacts with other birds.Note large actions such as preening, flight patterns and foraging habits as well as small movements such as tail bobs, head cocks or wing stretches.

        //[Display(Name = "Vocalisation notes")]
        public string NoteVocalisation { get; set; }
        // If the bird sang or made other sounds during your observation, use mnemonics or descriptions of how it sounded. Also note non-vocal sounds such as wing noises or drumming.

        //public bool HasPhotos { get; set; }

        //public PrivacyLevel SelectedPrivacyLevel { get; set; }

        [Required]
        //[Display(Name = "Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime ObservationDateTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        //[Display(Name = "Observed species")]
        public int BirdId { get; set; }
        //public string ApplicationUserId { get; set; }

        public Bird Bird { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public UserViewModel User { get; set; }
        //public ICollection<ObservationTag> ObservationTags { get; set; }
        //public ICollection<Photograph> Photographs { get; set; }
    }
}
