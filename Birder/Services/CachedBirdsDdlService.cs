using Microsoft.Extensions.Caching.Memory;

namespace Birder.Services;

public interface ICachedBirdsDdlService
{
    Task<IReadOnlyList<BirdSummaryDto>> GetAll();
}

public class CachedBirdsDdlService : ICachedBirdsDdlService
{
    public const string CacheKey = nameof(CachedBirdsDdlService);
    private readonly IMemoryCache _cache;
    private readonly IBirdDataService _service;

    public CachedBirdsDdlService(IMemoryCache cache, IBirdDataService service)
    {
        _cache = cache;
        _service = service;
    }

    public async Task<IReadOnlyList<BirdSummaryDto>> GetAll()
    {
        if (!_cache.TryGetValue(CacheKey, out IReadOnlyList<BirdSummaryDto> result))
        {
            result = await _service.GetBirdsDropDownListAsync();

            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
            _cache.Set(CacheKey, result, options);
        }

        return result;
    }
}