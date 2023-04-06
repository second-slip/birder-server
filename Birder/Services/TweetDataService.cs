using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Birder.Services;

public interface ITweetDataService
{
    Task<TweetDayDto> GetTweetOfTheDayAsync(DateTime date);
    Task<IEnumerable<TweetDayDto>> GetTweetArchiveAsync(int pageIndex, int pageSize, DateTime date);
}

public class TweetDataService : ITweetDataService
{
    private readonly ApplicationDbContext _dbContext;

    public TweetDataService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TweetDayDto>> GetTweetArchiveAsync(int pageIndex, int pageSize, DateTime date)
    {
        var query = _dbContext.TweetDays
            .MapTweetDaytoDto()
            .Where(d => d.DisplayDay <= date)
            .AsNoTracking()
            .AsQueryable();

        query = query.OrderByDescending(s => s.DisplayDay);
        query = query.ApplyPaging(pageIndex, pageSize);

        return await query.ToListAsync();
    }

    public async Task<TweetDayDto> GetTweetOfTheDayAsync(DateTime date)
    {
        var query = _dbContext.TweetDays
            .MapTweetDaytoDto()
            .AsNoTracking()
            .Where(t => t.DisplayDay == date);

        var result = await query.FirstOrDefaultAsync();

        // test this explicitly.....................................
        // .........................................................

        if (result is null)
        {
            query = _dbContext.TweetDays
            .MapTweetDaytoDto()
            .AsNoTracking()
            .Take(1);

            result = await query.FirstOrDefaultAsync();
        }

        return result;
    }
}