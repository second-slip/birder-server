//using Birder.Helpers;
//using Birder.Services;
//using Birder.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Birder.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(AuthenticationSchemes = "Bearer")]
//    public class PhotographController : ControllerBase
//    {
//        private readonly ILogger _logger;
//        private readonly IFileClient _fileClient;

//        public PhotographController(ILogger<PhotographController> logger
//                                  , IFileClient fileClient)
                                    
//        {
//            _logger = logger;
//            _fileClient = fileClient;
//        }

//        [HttpPost]
//        public async Task<IActionResult> UploadPhotographAsync([FromForm] UploadPhotographsDto model)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    _logger.LogError(LoggingEvents.GetListNotFound, $"Invalid ModelState at UploadPhotographAsync");
//                    return BadRequest();
//                }

//                if (model.Files.Count == 0)
//                {
//                    _logger.LogError(LoggingEvents.GetListNotFound, $"Empty Files argument at UploadPhotographAsync");
//                    return BadRequest("No files received from the upload");
//                }

//                if (model.ObservationId == 0)
//                {
//                    _logger.LogError(LoggingEvents.GetListNotFound, $"Invalid ObservationId argument at UploadPhotographAsync");
//                    return BadRequest("No observationId is supplied");
//                }

//                foreach (IFormFile file in model.Files)
//                {
//                    if (file.Length > 0 && StorageHelpers.IsImage(file))
//                    {
//                        string filename = StorageHelpers.GetFileName(file);

//                        using (var stream = file.OpenReadStream())
//                        {
//                            await _fileClient.SaveFile(model.ObservationId.ToString(), filename, stream);
//                            //isUploaded = await StorageExtension.UploadFileToStorage(stream, observationId.ToString(), formFile.FileName, _config["BlobStorageKey"], _config["BlobStorage:Account"]);
//                        }
//                    }
//                }

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(LoggingEvents.GetListNotFound, ex, $"An error uploading photographs for ObservationId: {model.ObservationId}");
//                return BadRequest("An unexpected error occurred");
//            }
//        }
     
//        [HttpGet, Route("GetPhotographs")]
//        public async Task<IActionResult> GetPhotographsByObservationAsync(int observationId)
//        {
//            try
//            {
//                if (observationId == 0)
//                {
//                    _logger.LogError(LoggingEvents.GetListNotFound, $"Invalid ObservationId argument at GetPhotographsByObservationAsync");
//                    return BadRequest("No observationId is supplied");
//                }

//                var urls = await _fileClient.GetAllFileUrl(observationId.ToString());
//                List<PhotographDto> model = StorageHelpers.UpdatePhotographsDto(urls);
                
//                return Ok(model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(LoggingEvents.GetListNotFound, ex, $"An error occurred getting the photographs for ObservationId: {observationId}");
//                return BadRequest("An unexpected error occurred");
//            }
//        }

//        [HttpPost, Route("DeletePhotograph")]
//        public async Task<IActionResult> PostDeletePhotographAsync([FromForm] int observationId, 
//                                                                   [FromForm] string filename)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(filename))
//                {
//                    _logger.LogError(LoggingEvents.GetListNotFound, $"Invalid filename argument at PostDeletePhotographAsync");
//                    return BadRequest("No filename is supplied");
//                }

//                if (observationId == 0)
//                {
//                    _logger.LogError(LoggingEvents.GetListNotFound, $"Invalid ObservationId argument at PostDeletePhotographAsync");
//                    return BadRequest("No observationId is supplied");
//                }

//                await _fileClient.DeleteFile(observationId.ToString(), filename);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(LoggingEvents.GetListNotFound, ex, $"An error occurred deleting the photograph with id: {filename}");
//                return BadRequest("An unexpected error occurred");
//            }
//        }
//    }
//}