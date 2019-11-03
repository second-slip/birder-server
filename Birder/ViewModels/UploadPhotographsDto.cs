using Microsoft.AspNetCore.Http;

namespace Birder.ViewModels
{
    public class UploadPhotographsDto
    {
        public int ObservationId { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
