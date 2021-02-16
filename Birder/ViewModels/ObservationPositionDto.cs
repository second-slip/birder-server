namespace Birder.ViewModels
{
    public class ObservationPositionDto
    {
        public int ObservationPositionId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedAddress { get; set; }
        public string ShortAddress { get; set; }
    }
}