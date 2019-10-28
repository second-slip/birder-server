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

        public async Task<IActionResult> UploadPhotograph(IFormFile file)
        {
            var fileName = file.FileName; //Sanitize("/" + UserId + "/" + file.FileName);

            using (var fileStream = file.OpenReadStream())
            {
                await _fileClient.SaveFile("Contracts", fileName, fileStream);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetPhotographsByObservation()
        {
            return Ok();
        }
    }
}