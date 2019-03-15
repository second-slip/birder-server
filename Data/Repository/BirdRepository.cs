using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class BirdRepository : IBirdRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BirdRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Bird> AllBirdsDropDownList()
        {
            var birds = _dbContext.Birds
                            .OrderBy(ob => ob.BirderStatus)
                                .ThenBy(a => a.EnglishName)
                                    .ToList();

            var defaultOption = new Bird()
            {
                BirdId = 0,
                EnglishName = "Choose a bird species..."
            };
            birds.Insert(0, defaultOption);

            return birds;
        }

        public IQueryable<Bird> AllBirdsList()
        {
            return _dbContext.Birds
                .Include(bcs => bcs.BirdConserverationStatus)
                     .OrderBy(bs => bs.BirderStatus)
                        .ThenBy(en => en.EnglishName)
                            .AsNoTracking();
        }

        public IQueryable<Bird> AllBirdsList(int birdId)
        {
            return _dbContext.Birds.Where(b => b.BirdId == birdId)
                .Include(bcs => bcs.BirdConserverationStatus)
                        .OrderByDescending(v => v.EnglishName)
                            .AsNoTracking();
        }

        public IQueryable<Bird> CommonBirdsList()
        {
            return _dbContext.Birds
                .Include(bcs => bcs.BirdConserverationStatus)
                    .Where(b => b.BirderStatus == BirderStatus.Common)
                     .OrderBy(bs => bs.BirderStatus)
                        .ThenBy(en => en.EnglishName)
                            .AsNoTracking();
        }

        public async Task<Bird> GetBirdDetails(int? id)
        {
            return await _dbContext.Birds
                            .Include(bcs => bcs.BirdConserverationStatus)
                                .SingleOrDefaultAsync(m => m.BirdId == id);
        }
    }
}
