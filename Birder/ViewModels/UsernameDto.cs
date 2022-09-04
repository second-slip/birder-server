using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels;

public class UsernameDto
{
    [Required]
    public string Username { get; set; }
}
