using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class SetLocationViewModel
    {
        [Required]
        public double DefaultLocationLatitude { get; set; }

        [Required]
        public double DefaultLocationLongitude { get; set; }
    }
}
