using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class LifeListViewModel
    {
    //     public IEnumerable<SpeciesSummaryViewModel> LifeList { get; set; }
    // }

    // public class SpeciesSummaryViewModel
    // {
        public int BirdId { get; set; }
        public string EnglishName { get; set; }
        public string Species { get; set; }
        public string PopulationSize { get; set; }
        public string BtoStatusInBritain { get; set; }
        public string ConservationStatus { get; set; }
        public int Count { get; set; }
    }
}
