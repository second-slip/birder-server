using Microsoft.AspNetCore.Http.HttpResults;

namespace Birder.MinApiEndpoints;

public class BirdEndpoints
{
    public static async Task<Results<Ok<IReadOnlyList<BirdSummaryDto>>, NotFound, StatusCodeHttpResult>> GetBirdsDdlAsync(ICachedBirdsDdlService service, ILogger<BirdEndpoints> logger)
    {
        try
        {
            var model = await service.GetAll();
            if (model is null) return TypedResults.NotFound();
            return TypedResults.Ok(model);
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}