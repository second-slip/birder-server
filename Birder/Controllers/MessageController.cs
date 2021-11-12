using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public MessageController(IEmailSender emailSender, ILogger<MessageController> logger) 
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost, Route("SendContactMessage")]
        public async Task<IActionResult> PostContactMessageAsync(ContactFormDto model)
        {
            try
            {
                var templateModel = new { name = model.Name, email = model.Email, message = model.Message };
                await _emailSender.SendTemplateEmail("d-55cc650c1ace438d81e5aa53df1aaf4d", "andrew-stuart-cross@outlook.com", templateModel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "an exception was raised");
                return StatusCode(500, "an unexpected error occurred");
            }
        }
    }
}
