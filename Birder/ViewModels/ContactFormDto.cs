using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class ContactFormDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
