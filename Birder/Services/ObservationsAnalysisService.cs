using Birder.Data;
using Birder.Data.Model;
using Birder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Birder.Services
{
    public interface IObservationsAnalysisService
    {
        Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate);

        Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate, DateTime year);

        //Task<TopObservationsAnalysisViewModel> GetTop5(Expression<Func<Observation, bool>> predicate, DateTime startDate);
    }

    // Using the 'thin' controller I was retrieving the user's full list of Observation
    // then analysing the in memory collection.  
    // This is the better alternative.  Counts are executed on the db.

    public class ObservationsAnalysisService: IObservationsAnalysisService
    {
        private readonly ApplicationDbContext _dbContext;
        public ObservationsAnalysisService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

//        public async Task<TopObservationsAnalysisViewModel> GetTop5(Expression<Func<Observation, bool>> predicate, DateTime startDate)
//        {
//            var result = new TopObservationsAnalysisViewModel();

//            var query = _dbContext.Observations
//                .Include(y => y.Bird)
//                    //.ThenInclude(u => u.BirdConservationStatus)
//                .Include(au => au.ApplicationUser)
//                .AsNoTracking()
//                .AsQueryable();

//            query = query.Where(predicate);

//            //         var firstProducts = query
//            //.GroupBy(p => p.BirdId)
//            //.Select(g => g.OrderBy(p => p.BirdId).Take(5))
//            //.ToListAsync();

////            result.TopObservations = query
////.GroupBy(p => p.BirdId)
////.Select(n => new TopObservationsViewModel {
////    BirdId = n.FirstOrDefault().BirdId,
////    Name = n.FirstOrDefault().Bird.EnglishName, //Name = n.FirstOrDefault().Bird?.EnglishName,
////    Count = n.Count()
////}).OrderByDescending(g => g.Count).Take(5).ToListAsync();
//////.ToListAsync();

//            //result.TopObservations = query.GroupBy(b => b.BirdId)
//            //                    .Select(n => new TopObservationsViewModel
//            //                    {
//            //                        BirdId = n.FirstOrDefault().BirdId,
//            //                        Name = n.FirstOrDefault().Bird.EnglishName, //Name = n.FirstOrDefault().Bird?.EnglishName,
//            //                        Count = n.Count()
//            //                    })
//            //                    .OrderByDescending(n => n.Count)
//            //                    .Take(5);

//           return result;


//        }

        public async Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate)
        {
            if (predicate is null)
                throw new ArgumentException("The argument is null or empty", nameof(predicate));

            var model = new ObservationAnalysisViewModel();

            var query = _dbContext.Observations
                .Include(y => y.Bird)
                .Include(au => au.ApplicationUser)
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(predicate);

            model.TotalObservationsCount = await query.CountAsync();

            model.UniqueSpeciesCount = await query.Select(i => i.BirdId).Distinct().CountAsync();

            return model;
        }

        public async Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate, DateTime year)
        {
            if (predicate is null)
                throw new ArgumentException("The argument is null or empty", nameof(predicate));

            var model = new ObservationAnalysisViewModel();

            var query = _dbContext.Observations
                .Include(y => y.Bird)
                .Include(au => au.ApplicationUser)
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(predicate);

            query = query.Where(d => d.ObservationDateTime >= year);

            model.TotalObservationsCount = await query.CountAsync();

            model.UniqueSpeciesCount = await query.Select(i => i.BirdId).Distinct().CountAsync();

            return model;
        }
    }
}
