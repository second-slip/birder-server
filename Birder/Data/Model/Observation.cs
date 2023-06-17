
using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model;

public class Observation
{
    [Key]
    public int ObservationId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
    public int Quantity { get; set; }
    public bool HasPhotos { get; set; }
    public PrivacyLevel SelectedPrivacyLevel { get; set; }
    //[Required]
    public DateTime ObservationDateTime { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    //[Range(1, int.MaxValue, ErrorMessage = "You must choose the bird species you observed")]
    public int BirdId { get; set; }
    public Bird Bird { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public ObservationPosition Position { get; set; }
    public ICollection<ObservationTag> ObservationTags { get; set; }
    public ICollection<ObservationNote> Notes { get; set; }
    //public ICollection<Photograph> Photographs { get; set; }
}