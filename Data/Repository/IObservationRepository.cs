using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<IEnumerable<Observation>> GetObservationsAsync(Expression<Func<Observation, bool>> predicate);
        Task<IEnumerable<Observation>> GetObservationsAsync();
        Task<Observation> GetObservationAsync(int id, bool includeRelated = true);
        Task<bool> ObservationExists(int id);
    }
}
