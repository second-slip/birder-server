using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Birder.Data.Repository
{
    public interface ITweetDayRepository : IRepository<TweetDay>
    {
        Task<TweetDay> GetTweetOfTheDayAsync(DateTime date);
        Task<QueryResult<TweetDay>> GetTweetArchiveAsync(int pageIndex, int pageSize, DateTime date);
    }

    public class TweetDayRepository: Repository<TweetDay>, ITweetDayRepository
    {
        public TweetDayRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<QueryResult<TweetDay>> GetTweetArchiveAsync(int pageIndex, int pageSize, DateTime date)
        {
            var result = new QueryResult<TweetDay>();

            var query = DbContext.TweetDays

                .Include(u => u.Bird)
                .Where(d => d.DisplayDay <= date)
                .AsNoTracking()
                .AsQueryable();

            query = query.OrderByDescending(s => s.DisplayDay);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<TweetDay> GetTweetOfTheDayAsync(DateTime date)
        {
            var tweet = await (from td in DbContext.TweetDays
                                   .Include(b => b.Bird)
                               where (td.DisplayDay == date)
                               select td).FirstOrDefaultAsync();

            if (tweet == null)
            {
                tweet = await (from td in DbContext.TweetDays
                               .Include(b => b.Bird)
                         select td).FirstOrDefaultAsync();
            }

            return tweet;
        }
    }
}