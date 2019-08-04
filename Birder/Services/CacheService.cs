using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface ICacheService
    {
        void Add();
    }
    public class CacheService : ICacheService
    {
        //private
        protected IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add()
        {
            throw new NotImplementedException();
        }
    }
}
