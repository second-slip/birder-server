using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class ObservationRepository : Repository<Observation>, IObservationRepository
    {
        public ObservationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        //public async Task<IEnumerable<Observation>> GetBirdObservations(int birdId)
        //{
        //    return await _dbContext.Observations
        //        .Include(cs => cs.Bird)
        //        .Include(au => au.ApplicationUser)
        //        .Where(cs => cs.BirdId == birdId)
        //        .OrderByDescending(d => d.ObservationDateTime)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Observation>> ObservationsWithBird(Expression<Func<Observation, bool>> predicate)
        {
            return await _dbContext.Observations
                .Include(y => y.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                .Include(au => au.ApplicationUser)
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Observation>> GetUsersObservationsList(string username)
        {
            return await _dbContext.Observations
                .Where(o => o.ApplicationUser.UserName == username)
                    .Include(au => au.ApplicationUser)
                    .Include(b => b.Bird)
                    .OrderByDescending(d => d.ObservationDateTime)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<Observation>> GetPublicObservationsList()
        {
             return await _dbContext.Observations
                    .Include(au => au.ApplicationUser)
                    .Include(b => b.Bird)
                    .OrderByDescending(d => d.ObservationDateTime)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<Observation>> GetUsersNetworkObservationsList(string userId)
        {
            var loggedinUser = _dbContext.Users
                //.Include(x => x.Followers)
                //    .ThenInclude(x => x.Follower)
                .Include(y => y.Following)
                    .ThenInclude(r => r.ApplicationUser)
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            // PROBLEM WHEN FOLLOWERS = 0 -- cannot append own Id

            // TODO - why not just use if to check if following == 0?

            var userNetwork = (from p in loggedinUser.Following
                               select p.ApplicationUser.Id.ToString());
            //Therefore changed to less efficient || in LINQ WHERE

            var observations = _dbContext.Observations
                .Where(o => userNetwork.Contains(o.ApplicationUser.Id) || o.ApplicationUser.Id == loggedinUser.Id)
                    .Include(au => au.ApplicationUser)
                    .Include(b => b.Bird)
                    .Include(ot => ot.ObservationTags)
                        .ThenInclude(t => t.Tag)
                            .OrderByDescending(d => d.ObservationDateTime)
                                .AsNoTracking();
            return await observations.ToListAsync();
        }

        public async Task<Observation> GetObservation(int? id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await _dbContext.Observations
                            .Include(au => au.ApplicationUser)
                            .SingleOrDefaultAsync(m => m.ObservationId == id);
            }

            return await _dbContext.Observations
                .Include(au => au.ApplicationUser)
                .Include(b => b.Bird)
                .Include(ot => ot.ObservationTags)
                    .ThenInclude(t => t.Tag)
                        .SingleOrDefaultAsync(m => m.ObservationId == id);
        }

        public async Task<bool> ObservationExists(int id)
        {
            return await _dbContext.Observations.AnyAsync(e => e.ObservationId == id);
        }
    }
}
