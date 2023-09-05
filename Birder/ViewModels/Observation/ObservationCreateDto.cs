using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels;

public class ObservationCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
    public int Quantity { get; set; }
    public DateTime ObservationDateTime { get; set; }
    public BirdSummaryDto Bird { get; set; }
    public ObservationPositionDto Position { get; set; }
}