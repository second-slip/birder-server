﻿using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class ObservationRepository : IObservationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ObservationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Observation> GetUsersObservationsList(string username)
        {
            var observations = _dbContext.Observations
                .Where(o => o.ApplicationUser.UserName == username)
                    .Include(au => au.ApplicationUser)
                    .Include(b => b.Bird)
                    //.Include(ot => ot.ObservationTags)
                    //    .ThenInclude(t => t.Tag)
                    .OrderByDescending(d => d.ObservationDateTime)
                    .AsNoTracking();
            return observations;
        }

        public IQueryable<Observation> GetPublicObservationsList()
        {
            var observations = _dbContext.Observations
                    .Include(au => au.ApplicationUser)
                    .Include(b => b.Bird)
                    //.Include(ot => ot.ObservationTags)
                    //    .ThenInclude(t => t.Tag)
                    .OrderByDescending(d => d.ObservationDateTime)
                    .AsNoTracking();
                    //.Take(100);
            return observations;
        }

        public IQueryable<Observation> GetUsersNetworkObservationsList(string userId)
        {
            var loggedinUser = _dbContext.Users
                //.Include(x => x.Followers)
                //    .ThenInclude(x => x.Follower)
                .Include(y => y.Following)
                    .ThenInclude(r => r.ApplicationUser)
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            // PROBLEM WHEN FOLLOWERS = 0 -- cannot append own Id

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
            return observations;
        }

        public async Task<Observation> GetObservationDetail(int? id)
        {
            return await _dbContext.Observations
                .Include(b => b.Bird)
                .Include(au => au.ApplicationUser)
                .Include(ot => ot.ObservationTags)
                    .ThenInclude(t => t.Tag)
                        .SingleOrDefaultAsync(m => m.ObservationId == id);
        }

        public async Task<Observation> AddObservation(Observation observation)
        {
            _dbContext.Observations.Add(observation);
            await _dbContext.SaveChangesAsync();
            return(observation);  
        }

        public async Task<Observation> UpdateObservation(Observation observation)
        {
            _dbContext.Observations.Update(observation);
            await _dbContext.SaveChangesAsync();
            return(observation);
        }

        public async Task<bool> ObservationExists(int id)
        {
            return await _dbContext.Observations.AnyAsync(e => e.ObservationId == id);
        }

        public async Task<Observation> DeleteObservation(int id)
        {
            var observation = await _dbContext.Observations.SingleOrDefaultAsync(m => m.ObservationId == id);
            _dbContext.Observations.Remove(observation);
            await _dbContext.SaveChangesAsync();
            return observation;
        }
    }
}
