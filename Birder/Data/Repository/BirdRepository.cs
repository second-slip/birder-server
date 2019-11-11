using Birder.Data.Model;
using Birder.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class BirdRepository : IBirdRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BirdRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Bird> GetBirdAsync(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }

            return (from b in _dbContext.Birds
                    .Include(cs => cs.BirdConservationStatus)
                    where (b.BirdId == id)
                    select b).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Bird>> GetBirdsDdlAsync()
        {
            return await _dbContext.Birds
                    .Include(cs => cs.BirdConservationStatus)
                    .OrderBy(ob => ob.BirderStatus)
                    .ThenBy(a => a.EnglishName)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<QueryResult<Bird>> GetBirdsAsync(int pageIndex, int pageSize)
        {
            var result = new QueryResult<Bird>();

            var query = _dbContext.Birds
                .Include(u => u.BirdConservationStatus)
                .AsNoTracking()
                .AsQueryable();

            query = query.OrderBy(s => s.BirderStatus)
                         .ThenBy(n => n.EnglishName);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(pageIndex, pageSize);

            result.Items = await query.ToListAsync();

            return result;
        }
    }
}