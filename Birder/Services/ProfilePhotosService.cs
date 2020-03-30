using Birder.Data.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Birder.Services
{

    public interface IProfilePhotosService
    {
        IEnumerable<Observation> SetThumbnailUrl(IEnumerable<Observation> observations);
        Observation SetThumbnailUrl(Observation observation);
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
        /// Sets the bird thumbnail url from the Flickr API or the cache (collection of Observation)
        /// </summary>
        /// <param name="observations"></param>
        /// <returns></returns>
        public IEnumerable<Observation> SetThumbnailUrl(IEnumerable<Observation> observations)
        {
            // ToDo: add an extra step to check if observation.Bird.ThumbnailUrl is null or empty
            // Why?  Implement if we add some fixed image urls to the database...
            foreach (var observation in observations)
            {
                if (_cache.TryGetValue(GetCacheId(observation.Bird.BirdId), out string cacheUrl))
                {
                    observation.Bird.ThumbnailUrl = cacheUrl;
                }
                else
                {
                    // temp in dev to avoid hitting the API...
                    observation.Bird.ThumbnailUrl = "https://farm1.staticflickr.com/908/28167626118_f9ed3a67cf_q.png";
                    //observation.Bird.ThumbnailUrl = _flickrService.GetThumbnailUrl(observation.Bird.Species);
                    AddResponseToCache(observation.Bird.BirdId, observation.Bird.ThumbnailUrl);
                }
            }

            return observations;
        }

        /// <summary>
        /// /// Sets the bird thumbnail url from the Flickr API or the cache (single Observation)
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public Observation SetThumbnailUrl(Observation observation)
        {
            if (_cache.TryGetValue(GetCacheId(observation.Bird.BirdId), out string cacheUrl))
            {
                observation.Bird.ThumbnailUrl = cacheUrl;
            }
            else
            {
                // temp in dev to avoid hitting the API...
                observation.Bird.ThumbnailUrl = "https://farm1.staticflickr.com/908/28167626118_f9ed3a67cf_q.png";
                //observation.Bird.ThumbnailUrl = _flickrService.GetThumbnailUrl(observation.Bird.Species);
                AddResponseToCache(observation.Bird.BirdId, observation.Bird.ThumbnailUrl);
            }

            return observation;
        }

        public void AddResponseToCache(int birdId, string url)
        {
            string id = string.Concat("birdId-", birdId);
            _cache.Set(id, url, TimeSpan.FromDays(5));
        }

        public string GetCacheId(int birdId)
        {
            return string.Concat("birdId-", birdId);
        }
    }
}
