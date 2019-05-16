using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class ManageProfileViewModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //public string Avatar { get; set; }

        //public string StatusMessage { get; set; }
    }
}
