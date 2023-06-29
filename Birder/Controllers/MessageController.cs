using Microsoft.Extensions.Options;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IEmailSender _emailSender;
    private ConfigOptions _options { get; }

    public MessageController(IEmailSender emailSender, ILogger<MessageController> logger, IOptions<ConfigOptions> optionsAccessor)
    {
        _logger = logger;
        _emailSender = emailSender;
        _options = optionsAccessor.Value;
    }

    [HttpPost, Route("send-contact-message")]
    public async Task<IActionResult> PostContactMessageAsync(ContactFormDto model)
    {
        try
        {
            var templateModel = new { name = model.Name, email = model.Email, message = model.Message };
            var msg = _emailSender.CreateMailMessage(SendGridTemplateId.ContactForm, _options.DevMail, templateModel);
            await _emailSender.SendMessageAsync(msg);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return StatusCode(500);
        }
    }
}

// model validation is a hassle in Min Api
// public static async Task<Results<NoContent, StatusCodeHttpResult>> PostContactMessageAsync(ContactFormDto model, IEmailSender emailSender, ILogger<MailEndpoints> logger, IConfiguration config)
// {
//     try
//     {
//         var templateModel = new { name = model.Name, email = model.Email, message = model.Message };
//         var msg = emailSender.CreateMailMessage(SendGridTemplateId.ContactForm, config["DevMail"], templateModel);
//         await emailSender.SendMessageAsync(msg);
//         return TypedResults.NoContent();
//     }
//     catch (Exception ex)
//     {
//         logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
//         return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
//     }
// }