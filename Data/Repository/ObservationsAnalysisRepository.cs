using Birder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class ObservationsAnalysisRepository : IObservationsAnalysisRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ObservationsAnalysisRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ObservationAnalysisViewModel> GetObservationsAnalysis(string username)
        {
            var viewModel = new ObservationAnalysisViewModel();
            viewModel.TotalObservationsCount = await (from observations in _dbContext.Observations
                                                 where (observations.ApplicationUser.UserName == username)
                                                 select observations).CountAsync();

            viewModel.UniqueSpeciesCount = await (from observations in _dbContext.Observations
                                            where (observations.ApplicationUser.UserName == username)
                                            select observations.BirdId).Distinct().CountAsync();
            return viewModel;
        }

        //public async Task<TweetDay> GetTweetOfTheDayAsync(DateTime date)
        //{
        //    var tweet = await (from td in _dbContext.TweetDays
        //                           .Include(b => b.Bird)
        //                                where (td.DisplayDay == date)
        //                                    select td).FirstOrDefaultAsync();
        //    if (tweet == null)
        //    {
        //        tweet = await (from td in _dbContext.TweetDays
        //                           .Include(b => b.Bird)
        //                                select td).FirstOrDefaultAsync();
        //    }
        //    return tweet;
        //}

        public IQueryable<TopObservationsViewModel> GetTopObservations(string username)
        {
            return (from observations in _dbContext.Observations
                 .Include(b => b.Bird)
                 .Where(u => u.ApplicationUser.UserName == username)
                    group observations by observations.Bird into species
                    orderby species.Count() descending
                    select new TopObservationsViewModel
                    {
                        Name = species.FirstOrDefault().Bird.EnglishName,
                        Count = species.Count()
                    }).Take(5);
        }

        public IQueryable<TopObservationsViewModel> GetTopObservations(string username, DateTime date)
        {
            DateTime startDate = date.AddDays(-30);
            return (from observations in _dbContext.Observations
                    .Include(b => b.Bird)
                    where (observations.ApplicationUser.UserName == username && (observations.ObservationDateTime >= startDate))
                    group observations by observations.Bird into species
                    orderby species.Count() descending
                    select new TopObservationsViewModel
                    {
                        Name = species.FirstOrDefault().Bird.EnglishName,
                        Count = species.Count()
                    }).Take(5);
        }
    }
}
