using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class ObservationFeedDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<ObservationViewModel> Items { get; set; }

        public ObservationFeedFilter ReturnFilter { get; set; }
        //public bool DisplayMessage { get; set; }
        //public string Message { get; set; }
    }
}
