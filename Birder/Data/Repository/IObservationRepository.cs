using Birder.Data.Model;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<QueryResult<Observation>> GetObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
        Task<QueryResult<Observation>> GetObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex, int pageSize);
        Task<Observation> GetObservationAsync(int id, bool includeRelated);
    }
}
