using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TweetsController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ISystemClock _systemClock;
        private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsController(ITweetDayRepository tweetDayRepository,
                                IMemoryCache memoryCache,
                                ILogger<TweetsController> logger,
                                ISystemClock systemClock,
                                IMapper mapper)
        {
            _cache = memoryCache;
            _mapper = mapper;
            _logger = logger;
            _systemClock = systemClock;
            _tweetDayRepository = tweetDayRepository;
        }

        [HttpGet, Route("GetTweetDay")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTweetDay()
        {
            try
            {
                if (_cache.TryGetValue(nameof(TweetDayViewModel), out TweetDayViewModel tweetDayCache))
                {
                    return Ok(tweetDayCache);
                }

                var tweetsTest = _tweetDayRepository.GetAll();

                var tweet = await _tweetDayRepository.GetTweetOfTheDayAsync(_systemClock.GetToday);

                if (tweet == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "An error occurred getting tweet with date: {Date}", _systemClock.GetToday);
                    return BadRequest();
                }

                var viewModel = _mapper.Map<TweetDay, TweetDayViewModel>(tweet);

                var cacheEntryExpiryDate = _systemClock.GetEndOfToday;

                _cache.Set(nameof(TweetDayViewModel), viewModel, cacheEntryExpiryDate);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting tweet with date: {Date}", _systemClock.GetToday);
                return BadRequest();
            }
        }
    }
}
