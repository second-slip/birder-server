using Birder.Data.Model;
using Birder.ViewModels;
using System.Linq;

namespace Birder.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<BirdSummaryViewModel> MapBirdToDto(this IQueryable<Bird> birds)
        {
            return birds.Select(b => new BirdSummaryViewModel
            {
                BirdId = b.BirdId,

                Species = b.Species,

                EnglishName = b.EnglishName,

                PopulationSize = b.PopulationSize,

                BtoStatusInBritain = b.BtoStatusInBritain,

                ThumbnailUrl = b.ThumbnailUrl,

                ConservationStatus = b.BirdConservationStatus.ConservationList,

                ConservationListColourCode = b.BirdConservationStatus.ConservationListColourCode,

                BirderStatus = b.BirderStatus.ToString()
            });
        }



        //  public static IQueryable<Vehicle> ApplyFiltering(this IQueryable<Vehicle> query, VehicleQuery queryObj)
        //{
        //    if (queryObj.MakeId.HasValue)
        //        query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);

        //    if (queryObj.ModelId.HasValue)
        //        query = query.Where(v => v.ModelId == queryObj.ModelId.Value);

        //    return query;
        //}

        //public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        //{
        //    if (String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
        //        return query;

        //    if (queryObj.IsSortAscending)
        //        return query.OrderBy(columnsMap[queryObj.SortBy]);
        //    else
        //        return query.OrderByDescending(columnsMap[queryObj.SortBy]);
        //}

        //public static IQueryable<Observation> ApplyGrouping(this IQueryable<Observation> query) //, VehicleQuery queryObj)
        //{

        //    return query.GroupBy(b => b.BirdId);
        //    //if (queryObj.MakeId.HasValue)
        //    //    query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);

        //    //if (queryObj.ModelId.HasValue)
        //    //    query = query.Where(v => v.ModelId == queryObj.ModelId.Value);

        //    //return query;
        //}

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize) // IQueryObject queryObj)
        {
            if (page <= 0)
                page = 1;

            if (pageSize <= 0)
                pageSize = 10;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}

