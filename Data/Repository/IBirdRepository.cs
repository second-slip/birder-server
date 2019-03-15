using Birder.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IBirdRepository
    {
        IEnumerable<Bird> AllBirdsDropDownList();
        IQueryable<Bird> AllBirdsList();
        IQueryable<Bird> AllBirdsList(int birdId);
        IQueryable<Bird> CommonBirdsList();
        Task<Bird> GetBirdDetails(int? id);
    }
}
