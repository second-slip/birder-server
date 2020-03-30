using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Services
{

    public interface IProfilePhotosService
    {
        IEnumerable<Observation> GetProfilePhoto(IEnumerable<Observation> observations);
    }

    public class ProfilePhotosService : IProfilePhotosService
    {
        public IEnumerable<Observation> GetProfilePhoto(IEnumerable<Observation> observations)
        {
            throw new NotImplementedException();
        }
    }
}
