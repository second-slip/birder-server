using Birder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<SpeciesSummaryViewModel>> GetLifeList(string userName)
        {
            var viewModel = new LifeListViewModel();

            var lifeList = (from observations in _dbContext.Observations
                 .Include(b => b.Bird)
                    .ThenInclude(u => u.BirdConserverationStatus)
                 .Where(u => u.ApplicationUser.UserName == userName)
                                  group observations by observations.Bird into species
                                  orderby species.Count() descending
                                  select new SpeciesSummaryViewModel
                                  {
                                      EnglishName = species.FirstOrDefault().Bird.EnglishName,
                                      Species = species.FirstOrDefault().Bird.Species,
                                      PopulationSize = species.FirstOrDefault().Bird.PopulationSize,
                                      BtoStatusInBritain = species.FirstOrDefault().Bird.BtoStatusInBritain,
                                      ConservationStatus = species.FirstOrDefault().Bird.BirdConserverationStatus.ConservationStatus,
                                      Count = species.Count()
                                  }).ToListAsync();

            return await lifeList;
        }
    }

    public class LifeListViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<SpeciesSummaryViewModel> LifeList { get; set; }
        public ObservationAnalysisViewModel ObservationsAnalysis { get; set; }
        //public int TotalObservations { get; set; }
        //public int TotalSpecies { get; set; }
    }

    public class SpeciesSummaryViewModel
    {
        public string EnglishName { get; set; }
        public string Species { get; set; }
        public string PopulationSize { get; set; }
        public string BtoStatusInBritain { get; set; }
        public string ConservationStatus { get; set; }
        public int Count { get; set; }
    }
}
