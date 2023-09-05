
using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels;

public class ObservationUpdateDto
{
    public int ObservationId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
    public int Quantity { get; set; }
    public DateTime ObservationDateTime { get; set; }
    public string Username { get; set; }
    public BirdSummaryDto Bird { get; set; }
    public ObservationPositionDto Position { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}