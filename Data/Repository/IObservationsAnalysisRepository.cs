using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationsAnalysisRepository : IRepository<Observation>
    {
        Task<ObservationAnalysisViewModel> GetObservationsAnalysis(string username);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username);
        Task<TopObservationsAnalysisViewModel> gtAsync(string username, DateTime date);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username, DateTime date);
        IQueryable<SpeciesSummaryViewModel> GetLifeList(string userName);
    }
}
