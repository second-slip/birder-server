using Birder.Data;
using Birder.Data.Model;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IObservationQueryService
    {
        Task<ObservationsPagedDto> GetPagedObservations(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
    }
    public class ObservationQueryService : IObservationQueryService
    {
        private readonly ApplicationDbContext _dbContext;
        public ObservationQueryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ObservationsPagedDto> GetPagedObservations(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
        {
            var result = new ObservationsPagedDto();

            var query = _dbContext.Observations
                .AsNoTracking()
                .Where(predicate)
                .MapObservationToObservationViewDto()
                //.Include(y => y.Bird)
                //    .ThenInclude(u => u.BirdConservationStatus)
                //.Include(p => p.Position)
                //.Include(au => au.ApplicationUser)
                .AsQueryable();

            query = query.OrderByDescending(d => d.ObservationDateTime);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            result.Items = await query.ToListAsync();

            return result;
        }
    }
}
