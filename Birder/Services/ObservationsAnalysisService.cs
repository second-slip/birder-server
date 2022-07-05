using Birder.Data;
using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Birder.Services;

public interface IObservationsAnalysisService
{
    Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate);
}

public class ObservationsAnalysisService : IObservationsAnalysisService
{
    private readonly ApplicationDbContext _dbContext;
    public ObservationsAnalysisService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate)
    {
        if (predicate is null)
            throw new ArgumentException("The argument is null or empty", nameof(predicate));

        var model = new ObservationAnalysisViewModel();

        var query = _dbContext.Observations
            .Include(au => au.ApplicationUser)
            .AsNoTracking()
            .AsQueryable();

        query = query.Where(predicate);

        model.TotalObservationsCount = await query.CountAsync();

        model.UniqueSpeciesCount = await query.Select(i => i.BirdId).Distinct().CountAsync();

        return model;
    }
}