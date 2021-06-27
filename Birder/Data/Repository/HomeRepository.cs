using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IHomeRepository
    {
        Task<string> GetFirstConservationListStatusAsync();
    }

    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public HomeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetFirstConservationListStatusAsync()
        {
            return await _dbContext.ConservationStatuses.Select(p => p.ConservationList).FirstOrDefaultAsync();
        }
    }
}
