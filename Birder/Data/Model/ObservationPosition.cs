using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model
{
    public class ObservationPosition
    {
        [Key]
        public int ObservationPositionId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedAddress { get; set; }
    }
}
