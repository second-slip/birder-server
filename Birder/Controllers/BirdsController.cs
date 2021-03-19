using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BirdsController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IBirdRepository _birdRepository;

        public BirdsController(IMapper mapper
                             , IMemoryCache memoryCache
                             , ILogger<BirdsController> logger
                             , IBirdRepository birdRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _birdRepository = birdRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter)
        {
            try
            {
                var birds = await _birdRepository.GetBirdsAsync(pageIndex, pageSize, speciesFilter);

                if (birds == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                    return NotFound();
                }

                var viewModel = _mapper.Map<QueryResult<Bird>, BirdsListDto>(birds);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the birds list");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("BirdsList")]
        public async Task<IActionResult> GetBirdsDdlAsync() //BirderStatus filter)
        {
            try
            {
                if (_cache.TryGetValue(CacheEntries.BirdsSummaryList, out IEnumerable<BirdSummaryViewModel> birdsCache))
                {
                    return Ok(birdsCache);
                }
                else
                {
                    var birds = await _birdRepository.GetBirdsDdlAsync();
                    //var viewModel = await _birdRepository.GetBirdsDdlAsync();

                    if (birds == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                        return NotFound();
                    }

                    var viewModel = _mapper.Map<IEnumerable<Bird>, IEnumerable<BirdSummaryViewModel>>(birds);

                    _cache.Set(CacheEntries.BirdsSummaryList, viewModel, TimeSpan.FromDays(1));

                    return Ok(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the birds list");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("GetBird")]
        public async Task<IActionResult> GetBirdAsync(int id)
        {
            try
            {
                var bird = await _birdRepository.GetBirdAsync(id);

                if (bird == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetBird({ID}) NOT FOUND", id);
                    return NotFound();
                }

                return Ok(_mapper.Map<Bird, BirdDetailDto>(bird));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting bird with {ID}", id);
                return BadRequest("An error occurred");
            }
        }
    }
}