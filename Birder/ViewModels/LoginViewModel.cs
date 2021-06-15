using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        //[EmailAddress]
        public string UserName { get; set; }
        //public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
