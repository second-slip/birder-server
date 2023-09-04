using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels;

public class ObservationAddDto
{
    [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
    public int Quantity { get; set; }
    public DateTime ObservationDateTime { get; set; }
    public BirdSummaryDto Bird { get; set; }
    public ObservationPositionDto Position { get; set; }
    public List<ObservationNoteDto> Notes { get; set; }
}