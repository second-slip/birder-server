using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class ManageProfileViewModel
    {
        [Required]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IFormFile ProfileImage { get; set; }

        //public string StatusMessage { get; set; }
    }
}
