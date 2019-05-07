using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationsAnalysisRepository : IRepository<Observation>
    {
        Task<IEnumerable<Observation>> ObservationsWithBird(Expression<Func<Observation, bool>> predicate);
        Task<ObservationAnalysisViewModel> GetObservationsAnalysis(string username);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username);
        Task<TopObservationsAnalysisViewModel> gtAsync(string username, DateTime date);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username, DateTime date);
        IQueryable<SpeciesSummaryViewModel> GetLifeList(string userName);
    }
}
