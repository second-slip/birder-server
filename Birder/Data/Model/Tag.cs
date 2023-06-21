using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model;

public class Tag
{
    [Key]
    public int TagId { get; set; }

    [Required]
    //[RegularExpression(@"^[^0-9\\s]+$")] //----> No spaces!  Perhaps?
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    public string Name { get; set; }

    //public string Description { get; set; }

    public ICollection<ObservationTag> ObservationTags { get; set; }
}