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

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PhotographController : ControllerBase
    {
        private readonly IFileClient _fileClient;

        public PhotographController(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        // Index() and other actions
        [HttpPost]
        public async Task<IActionResult> UploadPhotograph([FromForm] UploadPhotographsDto model)
        {
            //try { }
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
                if (model.Files[i].Length > 0) //if (StorageExtension.IsImage(formFile))
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
     

        [HttpGet, Route("GetPhotographs")]
        public async Task<IActionResult> GetPhotographsByObservation(int observationId)
        {
            var urls = await _fileClient.GetAllFileUrl(observationId.ToString());
            var model = StorageHelpers.UpdatePhotographsDto(urls);
            return Ok(model);
        }
    }
}