using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<IEnumerable<Observation>> ObservationsWithBird(Expression<Func<Observation, bool>> predicate);
        Task<IEnumerable<Observation>> GetUsersObservationsList(string userId);
        Task<IEnumerable<Observation>> GetUsersNetworkObservationsList(string userId);
        Task<IEnumerable<Observation>> GetPublicObservationsList();
        Task<Observation> GetObservation(int id, bool includeRelated = true);
        Task<bool> ObservationExists(int id);
    }
}
