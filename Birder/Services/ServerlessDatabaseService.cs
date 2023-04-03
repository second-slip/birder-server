using Microsoft.EntityFrameworkCore;

namespace Birder.Services
{
    public interface IServerlessDatabaseService
    {
        Task<string> GetFirstConservationListStatusAsync();
    }

    public class ServerlessDatabaseService : IServerlessDatabaseService
    {
        private readonly ApplicationDbContext _dbContext;
        public ServerlessDatabaseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetFirstConservationListStatusAsync()
        {
            return await _dbContext.ConservationStatuses.Select(p => p.ConservationList).FirstOrDefaultAsync();
        }
    }
}
