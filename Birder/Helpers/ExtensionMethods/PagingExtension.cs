using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Birder.Helpers
{
    public static class PagingExtension
    {
        // var paged = _birdRepository.GetBirdSummaryList(BirderStatus.Common).GetPaged(1, 5);
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        // var viewModel = _birdRepository.GetBirdSummaryList(BirderStatus.Common).GetPaged<Bird, BirdSummaryDto>(1, 5, _mapper); 
        public static PagedResult<TU> GetPaged<T, TU>(this IQueryable<T> query,
                                            int page, int pageSize, IMapper mapper) where TU : class
        {
            var result = new PagedResult<TU>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip)
                                  .Take(pageSize)
                                  .ProjectTo<TU>(mapper.ConfigurationProvider)
                                  .ToList();

            return result;
        }
    }
}
