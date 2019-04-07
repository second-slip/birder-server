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
                        BirdId = species.FirstOrDefault().Bird.BirdId,
                        Name = species.FirstOrDefault().Bird.EnglishName,
                        Count = species.Count()
                    }).Take(5);
        }

        public IQueryable<TopObservationsViewModel> GetTopObservations(string username, DateTime date)
        {
            return (from observations in _dbContext.Observations
                    .Include(b => b.Bird)
                    where (observations.ApplicationUser.UserName == username && (observations.ObservationDateTime >= date))
                    group observations by observations.Bird into species
                    orderby species.Count() descending
                    select new TopObservationsViewModel
                    {
                        BirdId = species.FirstOrDefault().Bird.BirdId,
                        Name = species.FirstOrDefault().Bird.EnglishName,
                        Count = species.Count()
                    }).Take(5);
        }

        public IQueryable<SpeciesSummaryViewModel> GetLifeList(string userName)
        {
            var lifeList = (from observations in _dbContext.Observations
                 .Include(b => b.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                 .Where(u => u.ApplicationUser.UserName == userName)
                            group observations by observations.Bird into species
                            orderby species.Count() descending
                            select new SpeciesSummaryViewModel
                            {
                                BirdId = species.FirstOrDefault().Bird.BirdId,
                                EnglishName = species.FirstOrDefault().Bird.EnglishName,
                                Species = species.FirstOrDefault().Bird.Species,
                                PopulationSize = species.FirstOrDefault().Bird.PopulationSize,
                                BtoStatusInBritain = species.FirstOrDefault().Bird.BtoStatusInBritain,
                                ConservationStatus = species.FirstOrDefault().Bird.BirdConservationStatus.ConservationList,
                                Count = species.Count()
                            });

            return lifeList;
        }
    }
}
