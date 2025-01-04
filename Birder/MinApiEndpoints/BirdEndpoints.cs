namespace Birder.MinApiEndpoints;

public class BirdEndpoints
{
    // to do integration tests on error response
    public static async Task<IResult> GetBirdsDdlAsync(ICachedBirdsDdlService service)//, ILogger<BirdEndpoints> logger)
    {
        var model = await service.GetAll();

        return model.Any()
            ? TypedResults.Ok(model)
            : TypedResults.NotFound();
    }

    public static async Task<IResult> GetBirdAsync(IBirdDataService service, int id)
    {
        if (service is null) return TypedResults.BadRequest();

        var model = await service.GetBirdAsync(id);

        if (model is null) return TypedResults.NotFound();

        return TypedResults.Ok(model);
    }

    public static async Task<IResult> GetBirdsAsync(IBirdDataService service, int pageIndex, int pageSize, BirderStatus speciesFilter)
    {
        if (service is null) return TypedResults.BadRequest();

        var model = await service.GetBirdsAsync(pageIndex, pageSize, speciesFilter);

        if (model is null) return TypedResults.NotFound();

        return TypedResults.Ok(model);
    }
}