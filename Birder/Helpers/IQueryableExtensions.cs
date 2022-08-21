using Birder.Data.Model;

namespace Birder.Helpers;
public static class QueryableExtensions
{
    public static IQueryable<ObservationFeedDto> MapObservationToObservationFeedDto(this IQueryable<Observation> observations)
    {
        return observations.Select(o => new ObservationFeedDto
        {
            ObservationId = o.ObservationId,
            Quantity = o.Quantity,
            ObservationDateTime = o.ObservationDateTime,
            BirdId = o.BirdId,
            Species = o.Bird.Species,
            EnglishName = o.Bird.EnglishName,
            ThumbnailUrl = o.Bird.ThumbnailUrl,
            Latitude = o.Position.Latitude,
            Longitude = o.Position.Longitude,
            FormattedAddress = o.Position.FormattedAddress,
            ShortAddress = o.Position.ShortAddress,
            Username = o.ApplicationUser.UserName,
            NotesCount = o.Notes.Count,
            CreationDate = o.CreationDate,
            LastUpdateDate = o.LastUpdateDate
        });
    }
    public static IQueryable<ObservationViewDto> MapObservationToObservationViewDto(this IQueryable<Observation> observations)
    {
        return observations.Select(o => new ObservationViewDto
        {
            ObservationId = o.ObservationId,
            Quantity = o.Quantity,
            ObservationDateTime = o.ObservationDateTime,
            BirdId = o.BirdId,
            Species = o.Bird.Species,
            EnglishName = o.Bird.EnglishName,
            Username = o.ApplicationUser.UserName,
            CreationDate = o.CreationDate,
            LastUpdateDate = o.LastUpdateDate
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