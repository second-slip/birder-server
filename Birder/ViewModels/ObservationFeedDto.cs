using Birder.Data.Model;
using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class ObservationFeedDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<Observation> Items { get; set; }
    }
}
