using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class TopObservationsAnalysisViewModel
    {
        public IEnumerable<TopObservationsViewModel> TopObservations { get; set; }
        public IEnumerable<TopObservationsViewModel> TopMonthlyObservations { get; set; }
    }

    public class TopObservationsViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
