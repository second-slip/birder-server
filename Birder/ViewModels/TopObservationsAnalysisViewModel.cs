

namespace Birder.ViewModels
{
    public class TopObservationsAnalysisViewModel
    {
        public IEnumerable<TopObservationsViewModel> TopObservations { get; set; }
        public IEnumerable<TopObservationsViewModel> TopMonthlyObservations { get; set; }
    }

    public class TopObservationsViewModel
    {
        public int BirdId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
