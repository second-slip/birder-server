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

        public static TopObservationsAnalysisViewModel MapTopObservations(IEnumerable<Observation> observations, DateTime date)
        {
            var viewModel = new TopObservationsAnalysisViewModel();

            viewModel.TopObservations = observations
                .GroupBy(n => n.Bird)
                .Select(n => new TopObservationsViewModel
                {
                    BirdId = n.Key.BirdId,
                    Name = n.Key.EnglishName,
                    Count = n.Count()
                }).OrderByDescending(n => n.Count).Take(5);

            viewModel.TopMonthlyObservations = observations
                .Where(o => o.ObservationDateTime >= date)
                .GroupBy(n => n.Bird)
                .Select(n => new TopObservationsViewModel
                {
                    BirdId = n.Key.BirdId,
                    Name = n.Key.EnglishName,
                    Count = n.Count()
                }).OrderByDescending(n => n.Count).Take(5);

            return viewModel;

        }
    }
}
