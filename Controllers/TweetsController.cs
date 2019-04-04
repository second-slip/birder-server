using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Birder.Data;
using Birder.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Birder.Data.Repository;
using Birder.ViewModels;
using AutoMapper;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TweetsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsController(ITweetDayRepository tweetDayRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tweetDayRepository = tweetDayRepository;
        }

        [HttpGet, Route("GetTweetDay")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTweetDay()
        {
            try
            {
                var tweet = await _tweetDayRepository.GetTweetOfTheDayAsync(DateTime.Today);

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
