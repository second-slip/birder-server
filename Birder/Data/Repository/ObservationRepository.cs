using Birder.Data.Model;
using Birder.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<Observation> GetObservationAsync(int id, bool includeRelated);
    }

    public class ObservationRepository : Repository<Observation>, IObservationRepository
    {
        public ObservationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Observation> GetObservationAsync(int id, bool includeRelated)
        {
            if (!includeRelated)
            {
                return await _dbContext.Observations
                            .Include(au => au.ApplicationUser)
                                .SingleOrDefaultAsync(m => m.ObservationId == id);
            }

            return await _dbContext.Observations
                .Include(p => p.Position)
                .Include(n => n.Notes)
                .Include(au => au.ApplicationUser)
                .Include(b => b.Bird)
                .Include(ot => ot.ObservationTags)
                    .ThenInclude(t => t.Tag)
                        .SingleOrDefaultAsync(m => m.ObservationId == id);
        }
    }
}
