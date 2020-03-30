using Birder.Data.Model;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache _cache;
        private readonly IFlickrService _flickrService;

        public ProfilePhotosService(IMemoryCache memoryCache
                                   , IFlickrService flickrService)
        {
            _cache = memoryCache;
            _flickrService = flickrService;
        }

        public IEnumerable<Observation> GetProfilePhoto(IEnumerable<Observation> observations)
        {
            //
            foreach (var item in observations)
            {
                if (_cache.TryGetValue(string.Concat("thumb-", item.Bird.BirdId), out string cacheUrl))
                {
                    item.Bird.ThumbnailUrl = cacheUrl;
                }
                else
                {
                    item.Bird.ThumbnailUrl = _flickrService.GetThumbnailUrl(item.Bird.Species);
                    _cache.Set(string.Concat("thumb-", item.Bird.BirdId), item.Bird.ThumbnailUrl);
                }
            }
            return observations;
            //
        }
    }
}
