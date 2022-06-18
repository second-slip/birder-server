using Birder.Data;
using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Birder.Services
{
    public interface IObservationQueryService
    {
        Task<ObservationsPagedDto> GetPagedObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
        Task<IEnumerable<ObservationFeedDto>> GetPagedObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
    }
    public class ObservationQueryService : IObservationQueryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBirdThumbnailPhotoService _profilePhotosService;

        public ObservationQueryService(ApplicationDbContext dbContext, IBirdThumbnailPhotoService profilePhotosService)
        {
            _dbContext = dbContext;
            _profilePhotosService = profilePhotosService;
        }

        public async Task<ObservationsPagedDto> GetPagedObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
        {
            var result = new ObservationsPagedDto();

            var query = _dbContext.Observations
                .AsNoTracking()
                .Where(predicate)
                .MapObservationToObservationViewDto()
                .AsQueryable();

            query = query.OrderByDescending(d => d.ObservationDateTime);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ObservationFeedDto>> GetPagedObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
        {
            //var result = new ObservationFeedPagedDto();

            var query = _dbContext.Observations
                .AsNoTracking()
                .Where(predicate)
                .MapObservationToObservationFeedDto()
                // .AsSplitQuery()
                .AsQueryable();

            //query = query.ApplyFiltering(queryObj);

            query = query.OrderByDescending(d => d.ObservationDateTime);

            //result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            // result.Items = await query.ToListAsync();
            var result = await query.ToListAsync();

            _profilePhotosService.GetThumbnailUrl(result);

            return result;
        }
    }
}
