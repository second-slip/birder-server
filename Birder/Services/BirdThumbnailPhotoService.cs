using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Birder.Services
{

    public interface IBirdThumbnailPhotoService
    {
        IEnumerable<ObservationFeedDto> GetUrlForObservations(IEnumerable<ObservationFeedDto> observations);
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
        public IEnumerable<ObservationFeedDto> GetUrlForObservations(IEnumerable<ObservationFeedDto> observations)
        {
            if (observations == null)
                throw new ArgumentNullException("observations", "The observations collection is null");

            // ToDo: add an extra step to check if observation.Bird.ThumbnailUrl is null or empty
            // Why?  Implement if we add some fixed image urls to the database...
            foreach (var observation in observations)
            {
                try
                {
                    if (_cache.TryGetValue(GetCacheEntryKey(observation.BirdId), out string cacheUrl))
                    {
                        observation.ThumbnailUrl = cacheUrl;
                    }
                    else
                    {
                        observation.ThumbnailUrl = _flickrService.GetThumbnailUrl(observation.Species);
                        AddResponseToCache(observation.BirdId, observation.ThumbnailUrl);
                    }
                }
                catch (Exception ex)
                {
                    observation.ThumbnailUrl = defaultUrl;
                    string message = $"An error occurred setting the thumbnail url for birdId '{observation.BirdId}'.";
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
