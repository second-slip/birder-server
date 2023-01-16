using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Birder.Services;

public interface IBirdThumbnailPhotoService
{
    Task<IEnumerable<ObservationFeedDto>> GetThumbnailUrl(IEnumerable<ObservationFeedDto> observations);
}

// potentially use a distributed cache to limit hits on the external API?

public class BirdThumbnailPhotoService : IBirdThumbnailPhotoService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;
    private readonly IFlickrService _flickrService;

    private const string DefaultUrl = "https://farm1.staticflickr.com/908/28167626118_f9ed3a67cf_q.png";

    public BirdThumbnailPhotoService(IMemoryCache memoryCache
                                   , ILogger<BirdThumbnailPhotoService> logger
                                   , IFlickrService flickrService)
    {
        _cache = memoryCache;
        _flickrService = flickrService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a bird thumbnail url from the Flickr API or the cache
    /// </summary>
    /// <param name="observations"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ObservationFeedDto>> GetThumbnailUrl(IEnumerable<ObservationFeedDto> observations)
    {
        if (observations is null)
            throw new ArgumentNullException(nameof(observations), "The observations collection is null");

        foreach (var observation in observations)
        {
            try
            {
                if (_cache.TryGetValue(GenerateCacheEntryKey(observation.BirdId), out string cacheUrl))
                {
                    observation.ThumbnailUrl = cacheUrl;
                }
                else
                {
                    observation.ThumbnailUrl = await _flickrService.GetThumbnailUrl(observation.Species);
                    AddResponseToCache(observation.BirdId, observation.ThumbnailUrl);
                }
            }
            catch (Exception ex)
            {
                observation.ThumbnailUrl = DefaultUrl;
                string message = $"An error occurred setting the thumbnail url for birdId '{observation.BirdId}'.";
                _logger.LogError(LoggingEvents.GetItem, ex, message);
            }
        }

        return observations;
    }

    private void AddResponseToCache(int birdId, string url)
    {
        _cache.Set(GenerateCacheEntryKey(birdId), url, TimeSpan.FromDays(5));
    }

    private string GenerateCacheEntryKey(int birdId)
    {
        return string.Concat(CacheEntryKeys.BirdThumbUrl, birdId);
    }
}