using System;
using System.ComponentModel.DataAnnotations;

namespace Birder.ViewModels
{
    public abstract class ObservationDtoBase
    {
        public int ObservationId { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
        public int Quantity { get; set; }
        public string NoteGeneral { get; set; }
        public string NoteHabitat { get; set; }
        public string NoteWeather { get; set; }
        public string NoteAppearance { get; set; }
        public string NoteBehaviour { get; set; }
        public string NoteVocalisation { get; set; }
        public DateTime ObservationDateTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int BirdId { get; set; }
        public BirdSummaryViewModel Bird { get; set; }
        public int ObservationPositionId { get; set; } // check if this needed
        public ObservationPositionDto Position { get; set; }
        public UserViewModel User { get; set; }
    }

    public class ObservationDto : ObservationDtoBase { }

    public class ObservationEditDto : ObservationDtoBase { }
}
