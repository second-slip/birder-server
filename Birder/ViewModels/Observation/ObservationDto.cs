using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels;

public class ObservationDto
{
    public int ObservationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
    public int Quantity { get; set; }

    public DateTime ObservationDateTime { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public int BirdId { get; set; }
    public BirdSummaryViewModel Bird { get; set; }
    public int ObservationPositionId { get; set; } // check if this needed
    public ObservationPositionDto Position { get; set; }
    public UserViewModel User { get; set; }
    public List<ObservationNoteDto> Notes { get; set; }
}