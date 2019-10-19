using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class ObservationFeedDto
    {
        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<ObservationViewModel> Items { get; set; }
    }
}
