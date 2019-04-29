using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ISystemClock _systemClock;
        private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsController(ITweetDayRepository tweetDayRepository,
                                ISystemClock systemClock,
                                ILogger<TweetsController> logger,
                                IMapper mapper)
        {
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
                var tweet = await _tweetDayRepository.GetTweetOfTheDayAsync(_systemClock.Today);

                if (tweet == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "An error occurred getting tweet with date: {Date}", _systemClock.Today);
                    return BadRequest();
                }

                var viewModel = _mapper.Map<TweetDay, TweetDayViewModel>(tweet);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting tweet with date: {Date}", _systemClock.Today);
                return BadRequest();
            }
        }
    }
}
