using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model;
public class ConservationStatus
{
    [Key]
    public int ConservationStatusId { get; set; }

    [Required]
    public string ConservationList { get; set; }

    public string ConservationListColourCode { get; set; }

    public string Description { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public ICollection<Bird> Birds { get; set; }
}