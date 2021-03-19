using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class ObservationsAnalysisHelper
    {
        //public static IQueryable<LifeListViewModel> MapLifeListDto(this IQueryable<Observation> observations)
        //{

        //    var x = observations.AsQueryable().GroupBy(
        //                        n => n.BirdId,
        //                        n => n.Bird, (o, p)
        //                        => new LifeListViewModel
        //                        {
        //                            BirdId = p.First().BirdId,
        //                            EnglishName = p.First().EnglishName,
        //                            Species = p.First().Species,
        //                            PopulationSize = p.First().PopulationSize,
        //                            BtoStatusInBritain = p.First().BtoStatusInBritain,
        //                            ConservationStatus = p.First().BirdConservationStatus.ConservationList,
        //                            ConservationListColourCode = p.First().BirdConservationStatus.ConservationListColourCode,
        //                            Count = p.Count()
        //                        });

        //    return x;
        //}


        //public static IEnumerable<LifeListViewModel> MapLifeList(IEnumerable<Observation> observations)
        //{
        //    if (observations == null)
        //        throw new ArgumentNullException("observations", "The observations collection is null");


        //    return observations.GroupBy(n => n.BirdId)
        //                       .Select(n => new LifeListViewModel
        //                       {
        //                            BirdId = n.FirstOrDefault().BirdId,
        //                            EnglishName = n.FirstOrDefault().Bird?.EnglishName,
        //                            Species = n.FirstOrDefault().Bird?.Species,
        //                            PopulationSize = n.FirstOrDefault().Bird?.PopulationSize,
        //                            BtoStatusInBritain = n.FirstOrDefault().Bird?.BtoStatusInBritain,
        //                            ConservationStatus = n.FirstOrDefault().Bird?.BirdConservationStatus?.ConservationList,
        //                            ConservationListColourCode = n.FirstOrDefault().Bird?.BirdConservationStatus?.ConservationListColourCode,
        //                            Count = n.Count()
        //                       })
        //                       .OrderByDescending(n => n.Count);
        //}

        //public static TopObservationsAnalysisViewModel MapTopObservations(IEnumerable<Observation> observations, DateTime startDate)
        //{
        //    if(observations == null)
        //        throw new ArgumentNullException("observations", "The observations collection is null");

        //    var viewModel = new TopObservationsAnalysisViewModel();

        //    viewModel.TopObservations = observations
        //        .GroupBy(n => n.BirdId)
        //        .Select(n => new TopObservationsViewModel
        //        {
        //            //BirdId = n.FirstOrDefault().BirdId,
        //            Name = n.FirstOrDefault().Bird?.EnglishName,
        //            Count = n.Count()
        //        })
        //        .OrderByDescending(n => n.Count).Take(5);

        //    viewModel.TopMonthlyObservations = observations
        //        .Where(o => o.ObservationDateTime >= startDate)
        //        .GroupBy(n => n.BirdId)
        //        .Select(n => new TopObservationsViewModel
        //        {
        //            //BirdId = n.FirstOrDefault().BirdId,
        //            Name = n.FirstOrDefault().Bird?.EnglishName,
        //            Count = n.Count()
        //        })
        //        .OrderByDescending(n => n.Count).Take(5);

        //    return viewModel;
        //}
    }
}
