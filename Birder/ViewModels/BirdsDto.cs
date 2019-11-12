using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class BirdsDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<BirdSummaryViewModel> Items { get; set; }
    }
}
