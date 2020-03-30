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
        IEnumerable<Observation> GetThumbnailsUrls(IEnumerable<Observation> observations);
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

        /// <summary>
        /// Sets the bird thumbnail url from the Flickr API or the cache
        /// </summary>
        /// <param name="observations"></param>
        /// <returns></returns>
        public IEnumerable<Observation> GetThumbnailsUrls(IEnumerable<Observation> observations)
        {
            // ToDo: cache entry expiry date
            //
            foreach (var observation in observations)
            {
                if (_cache.TryGetValue(string.Concat("thumb-", observation.Bird.BirdId), out string cacheUrl))
                {
                    observation.Bird.ThumbnailUrl = cacheUrl;
                }
                else
                {
                    observation.Bird.ThumbnailUrl = _flickrService.GetThumbnailUrl(observation.Bird.Species);
                    _cache.Set(string.Concat("thumb-", observation.Bird.BirdId), observation.Bird.ThumbnailUrl);
                }
            }

            return observations;
        }
    }
}
