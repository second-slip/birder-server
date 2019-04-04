using Birder.Data.Model;
using System;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface ITweetDayRepository
    {
        Task<TweetDay> GetTweetOfTheDayAsync(DateTime date);
    }
}
