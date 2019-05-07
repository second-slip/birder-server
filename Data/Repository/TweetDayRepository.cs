using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class TweetDayRepository: Repository<TweetDay>, ITweetDayRepository
    {
        //private readonly ApplicationDbContext _dbContext;

        public TweetDayRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            //_dbContext = dbContext;
        }

        public async Task<TweetDay> GetTweetOfTheDayAsync(DateTime date)
        {
            var tweet = await (from td in _dbContext.TweetDays
                                   .Include(b => b.Bird)
                               where (td.DisplayDay == date)
                               select td).FirstOrDefaultAsync();

            if (tweet == null)
            {
                tweet = await (from td in _dbContext.TweetDays
                                   .Include(b => b.Bird)
                               select td).FirstOrDefaultAsync();
            }
            return tweet;
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return _dbContext as ApplicationDbContext; }
        }
    }
}