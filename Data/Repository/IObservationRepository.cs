using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<IEnumerable<Observation>> ObservationsWithBird(Expression<Func<Observation, bool>> predicate);
        IQueryable<Observation> GetUsersObservationsList(string userId);
        IQueryable<Observation> GetUsersNetworkObservationsList(string userId);
        IQueryable<Observation> GetPublicObservationsList();

        Task<Observation> GetObservation(int? id);
        Task<Observation> GetObservationDetail(int? id);
        Task<Observation> AddObservation(Observation observation);
        Task<Observation> UpdateObservation(Observation observation);
        Task<bool> ObservationExists(int id);
        Task<Observation> DeleteObservation(Observation observation);
        //IQueryable<SpeciesSummaryViewModel> GetLifeList(string userId);
        //Task<int> TotalObservationsCount(ApplicationUser user);
        //Task<int> UniqueSpeciesCount(ApplicationUser user);
    }
}
