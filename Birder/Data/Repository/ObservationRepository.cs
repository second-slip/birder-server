using Microsoft.EntityFrameworkCore;

namespace Birder.Data.Repository;

public interface IObservationRepository : IRepository<Observation>
{
    Task<Observation> GetObservationAsync(int id);
}

public class ObservationRepository : Repository<Observation>, IObservationRepository
{
    public ObservationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Observation> GetObservationAsync(int id)
    {
        if (id == 0)
            throw new ArgumentException("method argument is invalid (zero)", nameof(id));

        return await _dbContext.Observations
            .Include(p => p.Position)
            .Include(b => b.Bird)
            .Include(au => au.ApplicationUser)
            .SingleOrDefaultAsync(m => m.ObservationId == id);
    }
}