using Birder.Controllers;
using Birder.Data.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationsAnalysisRepository
    {
        //Task<TweetDay> GetTweetOfTheDayAsync(DateTime date);
        Task<ObservationAnalysisViewModel> GetObservationsAnalysis(string username);
        //IQueryable<TopObservationsViewModel> GetTopObservations(ApplicationUser user);
        //IQueryable<TopObservationsViewModel> GetTopObservations(ApplicationUser user, DateTime date);
    }
}
