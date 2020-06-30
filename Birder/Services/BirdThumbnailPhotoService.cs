using Birder.Data.Model;
using Birder.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Birder.Services
{

    public interface IBirdThumbnailPhotoService
    {
        IEnumerable<Observation> GetUrlForObservations(IEnumerable<Observation> observations);
        //Observation GetUrlForObservation(Observation observation);
    }

    // potentially use a distributed cache to limited hits on the external API?

    public class BirdThumbnailPhotoService : IBirdThumbnailPhotoService
    {
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly IFlickrService _flickrService;

        private const string defaultUrl = "https://farm1.staticflickr.com/908/28167626118_f9ed3a67cf_q.png";

        public BirdThumbnailPhotoService(IMemoryCache memoryCache
                                       , ILogger<BirdThumbnailPhotoService> logger
                                       , IFlickrService flickrService)
        {
            _cache = memoryCache;
            _flickrService = flickrService;
            _logger = logger;
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
                try
                {
                    if (_cache.TryGetValue(GetCacheEntryKey(observation.Bird.BirdId), out string cacheUrl))
                    {
                        observation.Bird.ThumbnailUrl = cacheUrl;
                    }
                    else
                    {
                        observation.Bird.ThumbnailUrl = _flickrService.GetThumbnailUrl(observation.Bird.Species);
                        AddResponseToCache(observation.Bird.BirdId, observation.Bird.ThumbnailUrl);
                    }
                }
                catch (Exception ex)
                {
                    observation.Bird.ThumbnailUrl = defaultUrl;
                    string message = $"An error occurred setting the thumbnail url for birdId '{observation.Bird.BirdId}'.";
                    _logger.LogError(LoggingEvents.GetItem, ex, message);
                }
            }

            return observations;
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
