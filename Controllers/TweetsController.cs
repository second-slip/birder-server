using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ISystemClock _systemClock;
        private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsController(ITweetDayRepository tweetDayRepository,
                                ISystemClock systemClock,
                                IMapper mapper)
        {
            _mapper = mapper;
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
                    return BadRequest();
                }

                var viewModel = _mapper.Map<TweetDay, TweetDayViewModel>(tweet);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
