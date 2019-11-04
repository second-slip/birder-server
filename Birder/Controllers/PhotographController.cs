using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PhotographController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFileClient _fileClient;

        public PhotographController(ILogger<PhotographController> logger
                                  , IFileClient fileClient)
                                    
        {
            _logger = logger;
            _fileClient = fileClient;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotographAsync([FromForm] UploadPhotographsDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //logging
                    return BadRequest();
                }

                if (model.Files.Count == 0)
                {
                    //logging
                    return BadRequest("No files received from the upload");
                }

                if (model.ObservationId == 0)
                {
                    //logging
                    return BadRequest("No observationId is supplied");
                }

                //var fileName = file.FileName; //Sanitize("/" + UserId + "/" + file.FileName);

                for (int i = 0; i < model.Files.Count; i++)
                {
                    if (model.Files[i].Length > 0 && StorageHelpers.IsImage(model.Files[i]))
                    {
                        // add helpers for these...
                        var fileExt = Path.GetExtension(model.Files[i].FileName);
                        var fileName = string.Concat(Guid.NewGuid(), fileExt);
                        using (var stream = model.Files[i].OpenReadStream())
                        {
                            await _fileClient.SaveFile(model.ObservationId.ToString(), fileName, stream);
                            //isUploaded = await StorageExtension.UploadFileToStorage(stream, observationId.ToString(), formFile.FileName, _config["BlobStorageKey"], _config["BlobStorage:Account"]);
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, $"An error uploading photographs for ObservationId: {model.ObservationId}");
                return BadRequest("An unexpected error occurred");
            }
        }
     

        [HttpGet, Route("GetPhotographs")]
        public async Task<IActionResult> GetPhotographsByObservationAsync(int observationId)
        {
            try
            {
                var urls = await _fileClient.GetAllFileUrl(observationId.ToString());
                List<PhotographDto> model = StorageHelpers.UpdatePhotographsDto(urls);
                
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, $"An error occurred getting the photographs for ObservationId: {observationId}");
                return BadRequest("An unexpected error occurred");
            }
        }

        [HttpPost, Route("DeletePhotograph")]
        public async Task<IActionResult> PostDeletePhotographAsync([FromForm] int observationId, 
                                                                   [FromForm] string filename)
        {
            try
            {
                await _fileClient.DeleteFile(observationId.ToString(), filename);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, $"An error occurred deleting the photograph with id: {filename}");
                return BadRequest("An unexpected error occurred");
            }
        }
    }
}