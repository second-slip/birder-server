using Birder.Data.Model;
using System;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface ITweetDayRepository : IRepository<TweetDay>
    {
        Task<TweetDay> GetTweetOfTheDayAsync(DateTime date);

        Task<QueryResult<TweetDay>> GetTweetArchiveAsync(int pageIndex, int pageSize);
    }
}
