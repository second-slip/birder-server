using Microsoft.AspNetCore.Http.HttpResults;

namespace Birder.MinApiEndpoints;

public class BirdEndpoints
{
    // to do integration tests on error response
    public static async Task<Results<Ok<IReadOnlyList<BirdSummaryDto>>, NotFound, StatusCodeHttpResult>> GetBirdsDdlAsync(ICachedBirdsDdlService service, ILogger<BirdEndpoints> logger)
    {
        var model = await service.GetAll();

        return model.Any()
            ? TypedResults.Ok(model)
            : TypedResults.NotFound();
    }
}
