using Birder.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository
    {
        IQueryable<Observation> GetUsersObservationsList(string userId);
        IQueryable<Observation> GetUsersNetworkObservationsList(string userId);
        IQueryable<Observation> GetPublicObservationsList();
        Task<Observation> GetObservationDetails(int? id);
        Task<Observation> AddObservation(Observation observation);
        Task<Observation> UpdateObservation(Observation observation);
        Task<bool> ObservationExists(int id);
        Task<Observation> DeleteObservation(int id);
        //IQueryable<SpeciesSummaryViewModel> GetLifeList(string userId);
        //Task<int> TotalObservationsCount(ApplicationUser user);
        //Task<int> UniqueSpeciesCount(ApplicationUser user);
    }
}
