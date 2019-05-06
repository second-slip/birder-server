using Birder.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationsAnalysisRepository
    {
        Task<ObservationAnalysisViewModel> GetObservationsAnalysis(string username);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username);
        Task<TopObservationsAnalysisViewModel> gtAsync(string username, DateTime date);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username, DateTime date);
        IQueryable<SpeciesSummaryViewModel> GetLifeList(string userName);
    }
}
