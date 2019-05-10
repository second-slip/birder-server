using Birder.Data.Model;
using Birder.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class LifeListMappingHelper
    {
        public static IEnumerable<LifeListViewModel> MapLifeList(IEnumerable<Observation> observations)
        {
            return observations.GroupBy(n => n.Bird)
                                .Select(n => new LifeListViewModel
                                {
                                    BirdId = n.Key.BirdId,
                                    EnglishName = n.Key.EnglishName,
                                    Species = n.Key.Species,
                                    PopulationSize = n.Key.PopulationSize,
                                    BtoStatusInBritain = n.Key.BtoStatusInBritain,
                                    ConservationStatus = n.Key.BirdConservationStatus.ConservationList,
                                    Count = n.Count()
                                }).OrderByDescending(n => n.Count);

        }
    }
}
