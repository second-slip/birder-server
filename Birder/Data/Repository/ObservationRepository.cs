using Birder.Data.Model;
using Birder.Helpers;
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

        public async Task<QueryResult<Observation>> GetObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
        {
            var result = new QueryResult<Observation>();

            var query = _dbContext.Observations
                .Include(y => y.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                .Include(p => p.Position)
                .Include(au => au.ApplicationUser)
                .AsNoTracking() // ????????
                .AsQueryable();

            //query = query.ApplyFiltering(queryObj);

            query = query.Where(predicate);

            query = query.OrderByDescending(d => d.ObservationDateTime);

            //var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            //{
            //    ["make"] = v => v.Model.Make.Name,
            //    ["model"] = v => v.Model.Name,
            //    ["contactName"] = v => v.ContactName
            //};
            //query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Observation>> GetShowcaseObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int quantity)
        {
            var result = new QueryResult<Observation>();

            var query = _dbContext.Observations
                .Include(y => y.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                 .Include(p => p.Position)
                .Include(au => au.ApplicationUser)
                .AsNoTracking() // ????????
                .AsQueryable();

            query = query.Where(predicate);

            query = query.Take(quantity);

            query = query.OrderByDescending(d => d.ObservationDateTime);

            result.TotalItems = await query.CountAsync();

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Observation>> GetObservationsAsync(Expression<Func<Observation, bool>> predicate)
        {
            return await _dbContext.Observations
                .Include(y => y.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                .Include(p => p.Position)
                .Include(au => au.ApplicationUser)
                .Where(predicate)
                .OrderByDescending(d => d.ObservationDateTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<QueryResult<Observation>> GetPagedObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
        {
            var result = new QueryResult<Observation>();

            var query = _dbContext.Observations
                .Include(y => y.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                .Include(p => p.Position)
                .Include(au => au.ApplicationUser)
                //.Where(predicate)
                //.OrderByDescending(d => d.ObservationDateTime)
                .AsNoTracking()
                .AsQueryable();
            //.ToListAsync();

            query = query.Where(predicate);

            query = query.OrderByDescending(d => d.ObservationDateTime);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            result.Items = await query.ToListAsync();

            return result;
        }
        

        public async Task<Observation> GetObservationAsync(int id, bool includeRelated)
        {
            if (!includeRelated)
            {
                return await _dbContext.Observations
                            .Include(au => au.ApplicationUser)
                                .SingleOrDefaultAsync(m => m.ObservationId == id);
            }

            return await _dbContext.Observations
                .Include(p => p.Position)
                .Include(au => au.ApplicationUser)
                .Include(b => b.Bird)
                .Include(ot => ot.ObservationTags)
                    .ThenInclude(t => t.Tag)
                        .SingleOrDefaultAsync(m => m.ObservationId == id);
        }




        //public async Task<bool> ObservationExists(int id)
        //{
        //    return await _dbContext.Observations.AnyAsync(e => e.ObservationId == id);
        //}
    }
}
