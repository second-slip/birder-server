using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IConservationStatusRepository
    {
        Task<string> GetFirstConservationListStatusAsync();
    }

    public class ConservationStatusRepository : IConservationStatusRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ConservationStatusRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetFirstConservationListStatusAsync()
        {
            return await _dbContext.ConservationStatuses.Select(p => p.ConservationList).FirstOrDefaultAsync();
        }
    }
}
