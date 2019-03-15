using Birder.Data.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface ISideBarRepository
    {
        Task<TweetDay> GetTweetOfTheDayAsync(DateTime date);
        //IQueryable<TopObservationsViewModel> GetTopObservations(ApplicationUser user);
        //IQueryable<TopObservationsViewModel> GetTopObservations(ApplicationUser user, DateTime date);
    }
}
