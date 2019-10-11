using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class ObservationsAnalysisHelper
    {
        public static IEnumerable<LifeListViewModel> MapLifeList(IEnumerable<Observation> observations)
        {
            if (observations == null)
                throw new ArgumentNullException("observations", "The observations collection is null");

            return observations.GroupBy(n => n.BirdId)
                               .Select(n => new LifeListViewModel
                               {
                                    BirdId = n.FirstOrDefault().BirdId,
                                    EnglishName = n.FirstOrDefault().Bird?.EnglishName,
                                    Species = n.FirstOrDefault().Bird?.Species,
                                    PopulationSize = n.FirstOrDefault().Bird?.PopulationSize,
                                    BtoStatusInBritain = n.FirstOrDefault().Bird?.BtoStatusInBritain,
                                    ConservationStatus = n.FirstOrDefault().Bird?.BirdConservationStatus?.ConservationList,
                                    Count = n.Count()
                               })
                               .OrderByDescending(n => n.Count);
        }

        public static TopObservationsAnalysisViewModel MapTopObservations(IEnumerable<Observation> observations, DateTime startDate)
        {
            if (observations == null)
                throw new ArgumentNullException("observations", "The observations collection is null");

            var viewModel = new TopObservationsAnalysisViewModel();

            viewModel.TopObservations = observations
                .GroupBy(n => n.BirdId)
                .Select(n => new TopObservationsViewModel
                {
                    BirdId = n.FirstOrDefault().BirdId,
                    Name = n.FirstOrDefault().Bird?.EnglishName,
                    Count = n.Count()
                })
                .OrderByDescending(n => n.Count).Take(5);

            viewModel.TopMonthlyObservations = observations
                .Where(o => o.ObservationDateTime >= startDate)
                .GroupBy(n => n.BirdId)
                .Select(n => new TopObservationsViewModel
                {
                    BirdId = n.FirstOrDefault().BirdId,
                    Name = n.FirstOrDefault().Bird?.EnglishName,
                    Count = n.Count()
                })
                .OrderByDescending(n => n.Count).Take(5);

            return viewModel;
        }
    }
}
