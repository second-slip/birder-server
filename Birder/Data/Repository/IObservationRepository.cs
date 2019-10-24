using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<QueryResult<Observation>> GetObservationsFeedAsync(Expression<Func<Observation, bool>> predicate, int page);
        //Task<IEnumerable<Observation>> GetPagedObservationsAsync(Expression<Func<Observation, bool>> predicate, int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<Observation>> GetObservationsAsync(Expression<Func<Observation, bool>> predicate);
        Task<Observation> GetObservationAsync(int id, bool includeRelated);
    }
}
