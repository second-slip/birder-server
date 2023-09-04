using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Birder.Services;
public interface IObservationQueryService
{
    Task<ObservationViewDto> GetObservationViewAsync(int id);
    Task<ObservationsPagedDto> GetPagedObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
    Task<IEnumerable<ObservationFeedDto>> GetPagedObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
}
public class ObservationQueryService : IObservationQueryService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;
    private readonly IBirdThumbnailPhotoService _profilePhotosService;

    public ObservationQueryService(IMapper mapper, ApplicationDbContext dbContext, IBirdThumbnailPhotoService profilePhotosService)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _profilePhotosService = profilePhotosService;
    }

    public async Task<ObservationViewDto> GetObservationViewAsync(int id)
    {
        if (id == 0)
            throw new ArgumentException("method argument is invalid (zero)", nameof(id));

        // var query = _dbContext.Observations
        //     .AsNoTracking()
        //     .Where(o => o.ObservationId == id)
        //     .MapObservationToObservationViewDto()
        //     .AsQueryable();

        var query = _dbContext.Observations
            .AsNoTracking()
            .Where(o => o.ObservationId == id)
            .ProjectTo<ObservationViewDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        return await query.SingleOrDefaultAsync();
    }

    public async Task<ObservationsPagedDto> GetPagedObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
    {
        if (predicate is null)
            throw new ArgumentException("method argument is null or empty", nameof(predicate));

        var query = _dbContext.Observations
            .AsNoTracking()
            .Where(predicate)
            .MapObservationToObservationViewDto()
            .OrderByDescending(d => d.ObservationDateTime)
            .AsQueryable();

        var result = new ObservationsPagedDto();

        result.TotalItems = await query.CountAsync();

        query = query.ApplyPaging(pageIndex, pageSize);

        result.Items = await query.ToListAsync();

        return result;
    }

    public async Task<IEnumerable<ObservationFeedDto>> GetPagedObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize)
    {
        if (predicate is null)
            throw new ArgumentException("method argument is null or empty", nameof(predicate));

        var query = _dbContext.Observations
            .AsNoTracking()
            .Where(predicate)
            .MapObservationToObservationFeedDto()
            // .AsSplitQuery()
            .AsQueryable();

        query = query.OrderByDescending(d => d.ObservationDateTime);
        query = query.ApplyPaging(pageIndex, pageSize);

        var result = await query.ToListAsync();

        await _profilePhotosService.GetThumbnailUrl(result);

        return result;
    }
}