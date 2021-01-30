using Birder.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IBirdRepository
    {
        Task<IEnumerable<Bird>> GetBirdsDdlAsync();
        Task<Bird> GetBirdAsync(int id);
        Task<QueryResult<Bird>> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter);
    }
}
