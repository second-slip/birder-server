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

        public async Task<LifeListViewModel> GetLifeList(string userName)
        {
            var viewModel = new LifeListViewModel();
            viewModel.UserName = userName;
            viewModel.TotalObservations = (from observations in _dbContext.Observations
                                           where (observations.ApplicationUser.UserName == userName)
                                           select observations).Count();

            viewModel.TotalSpecies = (from observations in _dbContext.Observations
                                      where (observations.ApplicationUser.UserName == userName)
                                      select observations.BirdId).Distinct().Count();

            viewModel.LifeList = (from observations in _dbContext.Observations
                 .Include(b => b.Bird)
                    .ThenInclude(u => u.BirdConserverationStatus)
                 .Where(u => u.ApplicationUser.UserName == userName)
                                  group observations by observations.Bird into species
                                  orderby species.Count() descending
                                  select new SpeciesSummaryViewModel
                                  {
                                      Vernacular = species.FirstOrDefault().Bird.EnglishName,
                                      ScientificName = species.FirstOrDefault().Bird.Species,
                                      PopSize = species.FirstOrDefault().Bird.PopulationSize,
                                      BtoStatus = species.FirstOrDefault().Bird.BtoStatusInBritain,
                                      ConservationStatus = species.FirstOrDefault().Bird.BirdConserverationStatus.ConservationStatus,
                                      Count = species.Count()
                                  });

            return viewModel;
        }
    }

    public class LifeListViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<SpeciesSummaryViewModel> LifeList { get; set; }
        public int TotalObservations { get; set; }
        public int TotalSpecies { get; set; }
    }

    public class SpeciesSummaryViewModel
    {
        public string Vernacular { get; set; }
        public string ScientificName { get; set; }
        public string PopSize { get; set; }
        public string BtoStatus { get; set; }
        public string ConservationStatus { get; set; }
        public int Count { get; set; }
    }
}
