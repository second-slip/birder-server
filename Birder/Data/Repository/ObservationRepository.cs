using Microsoft.EntityFrameworkCore;

namespace Birder.Data.Repository;

public interface IObservationRepository : IRepository<Observation>
{
    Task<Observation> GetObservationAsync(int id); //, bool includeRelated);
}

public class ObservationRepository : Repository<Observation>, IObservationRepository
{
    public ObservationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Observation> GetObservationAsync(int id) //, bool includeRelated)
    {
        // if (!includeRelated)
        // {
        return await _dbContext.Observations
            .Include(p => p.Position)
            .Include(b => b.Bird)
            .Include(au => au.ApplicationUser)
            .SingleOrDefaultAsync(m => m.ObservationId == id);
        // }

        // todo: replace with QueryObject
        // return await _dbContext.Observations
        //     .Include(au => au.ApplicationUser)
        //     .Include(p => p.Position)
        //     .Include(n => n.Notes)
        //     .Include(b => b.Bird)
        //     .Include(ot => ot.ObservationTags)
        //         .ThenInclude(t => t.Tag)
        //     .SingleOrDefaultAsync(m => m.ObservationId == id);
    }
}