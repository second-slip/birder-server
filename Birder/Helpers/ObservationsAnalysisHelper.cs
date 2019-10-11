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
            return observations.GroupBy(n => n.Bird.BirdId)
                                .Select(n => new LifeListViewModel
                                {
                                    BirdId = n.FirstOrDefault().Bird.BirdId,
                                    EnglishName = n.FirstOrDefault().Bird.EnglishName,
                                    Species = n.FirstOrDefault().Bird.Species,
                                    PopulationSize = n.FirstOrDefault().Bird.PopulationSize,
                                    BtoStatusInBritain = n.FirstOrDefault().Bird.BtoStatusInBritain,
                                    ConservationStatus = n.FirstOrDefault().Bird.BirdConservationStatus.ConservationList,
                                    Count = n.Count()
                                }).OrderByDescending(n => n.Count);
        }

        public static TopObservationsAnalysisViewModel MapTopObservations(IEnumerable<Observation> observations, DateTime startDate)
        {
            var viewModel = new TopObservationsAnalysisViewModel();

            viewModel.TopObservations = observations
                .GroupBy(n => n.Bird.BirdId)
                .Select(n => new TopObservationsViewModel
                {
                    BirdId = n.FirstOrDefault().Bird.BirdId,
                    Name = n.FirstOrDefault().Bird.EnglishName,
                    Count = n.Count()
                }).OrderByDescending(n => n.Count).Take(5);

            viewModel.TopMonthlyObservations = observations
                .Where(o => o.ObservationDateTime >= startDate)
                .GroupBy(n => n.Bird.BirdId)
                .Select(n => new TopObservationsViewModel
                {
                    BirdId = n.FirstOrDefault().Bird.BirdId,
                    Name = n.FirstOrDefault().Bird.EnglishName,
                    Count = n.Count()
                }).OrderByDescending(n => n.Count).Take(5);

            return viewModel;
        }
    }
}
