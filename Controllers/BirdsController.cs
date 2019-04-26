using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BirdsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IBirdRepository _birdRepository;

        public BirdsController(IMapper mapper
                             , ILogger<BirdsController> logger
                             , IBirdRepository birdRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _birdRepository = birdRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBirds(int pageIndex, int pageSize)
        {
            // TODO: Cache the birds list
            // The birds list is a prime candidate to be put in the cache.
            // The birds list is rarely updated.

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            if (pageSize == 0)
            {
                pageSize = 5;
            }

            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError(LoggingEvents.InvalidModelState, "BirdIndexOptions ModelState is invalid");
            //    return BadRequest(ModelState);
            //}

            try
            {
                // var birds = _birdRepository.GetBirdSummaryList(BirderStatus.Common);
                // var paged = _birdRepository.GetBirdSummaryList(BirderStatus.Common).GetPaged(1, 5);
                var viewModel = _birdRepository.GetBirdSummaryList(BirderStatus.Common).GetPaged<Bird, BirdSummaryViewModel>(pageIndex, pageSize, _mapper);

                if (viewModel.Results == null)
                {
                   _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                   return BadRequest();
                }

                return Ok(viewModel);
                // var viewmodel = _mapper.Map<PagedResult<Bird>, PagedResult<BirdSummaryViewModel>>(paged);
                // var viewmodel = _mapper.Map<List<Bird>, List<BirdSummaryViewModel>>(birds);
                // var viewModel = _birdRepository.GetBirdSummaryList(BirderStatus.Common).GetPaged<Bird, BirdSummaryViewModel>(1, 5, _mapper); 
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the birds list");
                return BadRequest();
            }
        }

        [HttpGet, Route("GetBird")]
        public async Task<IActionResult> GetBird(int id)
        {
            try
            {
                var bird = await _birdRepository.GetBird(id);

                if (bird == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetBird({ID}) NOT FOUND", id);
                    return NotFound();
                }

                var viewmodel = _mapper.Map<Bird, BirdDetailViewModel>(bird);

                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting bird with {ID}", id);
                return BadRequest();
            }
        }
    }

    public class BirdIndexOptions
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        //public BirderStatus FilterStatus { get; set; }
    }
}
