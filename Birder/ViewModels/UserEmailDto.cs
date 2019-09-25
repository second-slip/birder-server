using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class UserEmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
