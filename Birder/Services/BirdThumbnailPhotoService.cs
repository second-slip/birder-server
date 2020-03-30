using Birder.Data.Model;
using Birder.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Birder.Services
{

    public interface IBirdThumbnailPhotoService
    {
        IEnumerable<Observation> GetUrlForObservations(IEnumerable<Observation> observations);
        Observation GetUrlForObservation(Observation observation);
    }

    public class BirdThumbnailPhotoService : IBirdThumbnailPhotoService
    {
        private IMemoryCache _cache;
        private readonly IFlickrService _flickrService;

        public BirdThumbnailPhotoService(IMemoryCache memoryCache
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
        public IEnumerable<Observation> GetUrlForObservations(IEnumerable<Observation> observations)
        {
            if (observations == null)
                throw new ArgumentNullException("observations", "The observations collection is null");
            // ToDo: add an extra step to check if observation.Bird.ThumbnailUrl is null or empty
            // Why?  Implement if we add some fixed image urls to the database...
            foreach (var observation in observations)
            {
                if (_cache.TryGetValue(GetCacheEntryKey(observation.Bird.BirdId), out string cacheUrl))
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
        public Observation GetUrlForObservation(Observation observation)
        {
            if (observation == null)
                throw new ArgumentNullException("observation", "The observation is null");

            if (_cache.TryGetValue(GetCacheEntryKey(observation.Bird.BirdId), out string cacheUrl))
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
            _cache.Set(GetCacheEntryKey(birdId), url, TimeSpan.FromDays(5));
        }

        public string GetCacheEntryKey(int birdId)
        {
            return string.Concat(CacheEntryKeys.BirdThumbUrl, birdId);
        }
    }
}
