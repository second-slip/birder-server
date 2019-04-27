using Birder.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IBirdRepository
    {
        Task<IEnumerable<Bird>> GetBirdSummaryList(BirderStatus birderStatusFilter);
        Task<Bird> GetBird(int id);
    }
}
