using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class ObservationFeedDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<ObservationDto> Items { get; set; }
        public ObservationFeedFilter ReturnFilter { get; set; }
    }
}
