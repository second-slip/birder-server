using System.Collections.Generic;
using System.Threading.Tasks;
using Birder.Services;
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

        public async Task<IActionResult> UploadPhotograph(List<IFormFile> files)
        {
            //var fileName = file.FileName; //Sanitize("/" + UserId + "/" + file.FileName);

            int observationId = 1;

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Length > 0) //if (StorageExtension.IsImage(formFile))
                {
                    var fileName = files[i].FileName + "_" + (i + 1).ToString();
                    using (var stream = files[i].OpenReadStream())
                    {
                        await _fileClient.SaveFile(observationId.ToString(), fileName, stream);
                        //isUploaded = await StorageExtension.UploadFileToStorage(stream, observationId.ToString(), formFile.FileName, _config["BlobStorageKey"], _config["BlobStorage:Account"]);
                    }
                }
            }
            //foreach (var formFile in files)
            //{
            //    //if (StorageExtension.IsImage(formFile))
            //    //{
            //    if (formFile.Length > 0)
            //    {
            //         //Sanitize("/" + UserId + "/" + file.FileName);
            //        using (var stream = formFile.OpenReadStream())
            //        {
            //            await _fileClient.SaveFile(files.Insert, observationId.ToString(), stream);
            //            //isUploaded = await StorageExtension.UploadFileToStorage(stream, observationId.ToString(), formFile.FileName, _config["BlobStorageKey"], _config["BlobStorage:Account"]);
            //        }
            //    }
            //    //}
            //    //else
            //    //{
            //    //    return new UnsupportedMediaTypeResult();
            //    //}
            //}

            //using (var fileStream = file.OpenReadStream())
            //{
            //    await _fileClient.SaveFile("Contracts", fileName, fileStream);
            //}

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetPhotographsByObservation()
        {
            return Ok();
        }
    }
}