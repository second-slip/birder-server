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

        public Task<Bird> GetBird(int id)
        {
            return (from b in _dbContext.Birds
                    .Include(cs => cs.BirdConservationStatus)
                    where (b.BirdId == id)
                    select b).FirstOrDefaultAsync();
            //return await _dbContext.Birds.SingleOrDefaultAsync(m => m.BirdId == id);
        }

        public async Task<IEnumerable<Observation>> GetBirdObservationsAsync(int birdId)
        {
            return await _dbContext.Observations
                .Include(cs => cs.Bird)
                .Include(au => au.ApplicationUser)
                .Where(cs => cs.BirdId == birdId)
                .OrderByDescending(d => d.ObservationDateTime)
                .ToListAsync();
        }

        //public async Task<IEnumerable<Bird>> GetBirdSummaryList(BirderStatus birderStatusFilter)
        public IQueryable<Bird> GetBirdSummaryList(BirderStatus birderStatusFilter)
        {
            if (birderStatusFilter == BirderStatus.Common)
            {
                //        var observations = _dbContext.Observations
                //.Include(au => au.ApplicationUser)
                //.Include(b => b.Bird)
                ////.Include(ot => ot.ObservationTags)
                ////    .ThenInclude(t => t.Tag)
                //.OrderByDescending(d => d.ObservationDateTime)
                //.AsNoTracking();
                //        //.Take(100);
                //        return observations;
                //return _dbContext.Birds
                //                .Include(cs => cs.BirdConservationStatus)
                //                .Where(f => f.BirderStatus == birderStatusFilter)
                //                .OrderBy(a => a.EnglishName)
                //                .ToListAsync();
                return _dbContext.Birds
                .Include(cs => cs.BirdConservationStatus)
                .Where(f => f.BirderStatus == birderStatusFilter)
                .OrderBy(a => a.EnglishName)
                .AsNoTracking();
            }
            else
            {
                //return await _dbContext.Birds
                //                .Include(cs => cs.BirdConservationStatus)
                //                .OrderBy(ob => ob.BirderStatus)
                //                .ThenBy(a => a.EnglishName)
                //                .ToListAsync();
                return _dbContext.Birds
                                .Include(cs => cs.BirdConservationStatus)
                                .OrderBy(ob => ob.BirderStatus)
                                .ThenBy(a => a.EnglishName)
                                .AsNoTracking();
            }
        }
    }
}
