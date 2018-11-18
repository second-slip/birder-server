
namespace Birder.Models
{
    public class ObservationTag
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public int ObervationId { get; set; }
        public Observation Observation { get; set; }
    }
}
