using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public class ObservationEditDto
    {
        public int ObservationId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
        public int Quantity { get; set; }
        public DateTime ObservationDateTime { get; set; }
        public int BirdId { get; set; }
        public BirdSummaryViewModel Bird { get; set; }
        public ObservationPositionDto Position { get; set; }
        public UserViewModel User { get; set; }
        public List<ObservationNoteDto> Notes { get; set; }
    }
}
