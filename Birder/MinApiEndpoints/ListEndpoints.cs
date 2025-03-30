using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace Birder.MinApiEndpoints;

public class ListEndpoints
{
    public static async Task<Results<Ok<IReadOnlyList<TopObservationsViewModel>>, BadRequest>> GetTopObservationsAsync(IListService listService, ClaimsPrincipal user)
    {
        var topObservations = await listService.GetTopObservationsAsync(user.Identity.Name);
        if (topObservations is null) return TypedResults.BadRequest();
        return TypedResults.Ok(topObservations);
    }

    public static async Task<Results<Ok<IReadOnlyList<TopObservationsViewModel>>, BadRequest>> GetTopObservationsWithDateFilterAsync(IListService listService, ClaimsPrincipal user, int days)
    {
        var topObservations = await listService.GetTopObservationsAsync(user.Identity.Name, days);
        if (topObservations is null) return TypedResults.BadRequest();
        return TypedResults.Ok(topObservations);
    }

    public static async Task<Results<Ok<IReadOnlyList<LifeListViewModel>>, BadRequest>> GetLifeListAsync(IListService listService, ClaimsPrincipal user)
    {
        var list = await listService.GetLifeListAsync(a => a.ApplicationUser.UserName == user.Identity.Name);
        if (list is null) return TypedResults.BadRequest();
        return TypedResults.Ok(list);
    }
}