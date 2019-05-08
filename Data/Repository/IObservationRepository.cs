using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        // ToDo: - remove Update/Add/Delete methods; use generic ones instead

        Task<IEnumerable<Observation>> ObservationsWithBird(Expression<Func<Observation, bool>> predicate);
        //Task<IEnumerable<Observation>> GetBirdObservations(int birdId);
        Task<IEnumerable<Observation>> GetUsersObservationsList(string userId);
        Task<IEnumerable<Observation>> GetUsersNetworkObservationsList(string userId);
        Task<IEnumerable<Observation>> GetPublicObservationsList();
        Task<Observation> GetObservation(int? id, bool includeRelated = true);
        //Task<Observation> GetObservationDetail(int? id);
        Task<bool> ObservationExists(int id);
    }
}
