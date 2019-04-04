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

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetDayRepository _tweetDayRepository;

        public TweetsController(ITweetDayRepository tweetDayRepository)
        {
            _tweetDayRepository = tweetDayRepository;
        }

        [HttpGet, Route("GetTweetDay")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTweetDay()
        {
            try
            {
                var viewModel = await _tweetDayRepository.GetTweetOfTheDayAsync(DateTime.Today);

                if (viewModel == null)
                {
                    return BadRequest();
                }
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
