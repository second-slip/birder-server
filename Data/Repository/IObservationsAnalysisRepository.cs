using Birder.Controllers;
using Birder.Data.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationsAnalysisRepository
    {
        Task<ObservationAnalysisViewModel> GetObservationsAnalysis(string username);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username);
        IQueryable<TopObservationsViewModel> GetTopObservations(string username, DateTime date);
    }
}
