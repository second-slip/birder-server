
namespace Birder.Data.Model
{
    public class ObservationTag
    {
        public int Id { get; set; }
        public Tag Tag { get; set; }
        public int ObervationId { get; set; }
        public Observation Observation { get; set; }
    }
}
