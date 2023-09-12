using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Birder.Services;
public interface IListService
{
    Task<IEnumerable<LifeListViewModel>> GetLifeListAsync(Expression<Func<Observation, bool>> predicate);
    Task<List<TopObservationsViewModel>> GetTopObservationsAsync(string username);
    Task<List<TopObservationsViewModel>> GetTopObservationsAsync(string username, DateTime startDate);
}

public class ListService : IListService
{
    private readonly ApplicationDbContext _dbContext;
    public ListService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LifeListViewModel>> GetLifeListAsync(Expression<Func<Observation, bool>> predicate)
    {
        if (predicate is null)
            throw new ArgumentException("method argument is null or empty", nameof(predicate));

        var query = await _dbContext.Observations
                 .AsNoTracking()
                 .Where(predicate)
                 .GroupBy(b => new { b.BirdId, b.Bird.EnglishName, b.Bird.Species, b.Bird.PopulationSize, b.Bird.BtoStatusInBritain, b.Bird.BirdConservationStatus.ConservationList, b.Bird.BirdConservationStatus.ConservationListColourCode })
                 .Select(b => new LifeListViewModel
                 {
                     BirdId = b.Key.BirdId,
                     EnglishName = b.Key.EnglishName,
                     Species = b.Key.Species,
                     PopulationSize = b.Key.PopulationSize,
                     BtoStatusInBritain = b.Key.BtoStatusInBritain,
                     ConservationStatus = b.Key.ConservationList,
                     ConservationListColourCode = b.Key.ConservationListColourCode,
                     Count = b.Count()
                 })
                 .OrderByDescending(n => n.Count)
                 .AsQueryable()
                 .ToListAsync();

        return query;
    }

    // All time (no date filter)
    public async Task<List<TopObservationsViewModel>> GetTopObservationsAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("method argument is null or empty", nameof(username));

        var model = await _dbContext.Observations
                 .AsNoTracking()
                 .Where(a => a.ApplicationUser.UserName == username)
                 .GroupBy(b => new { b.Bird.BirdId, b.Bird.EnglishName })
                 .Select(b => new TopObservationsViewModel
                 {
                     BirdId = b.Key.BirdId,
                     Name = b.Key.EnglishName,
                     Count = b.Count()
                 })
                 .OrderByDescending(c => c.Count)
                 .Take(5)
                 .AsQueryable()
                 .ToListAsync();

        return model;
    }

    // Last 30 days
    public async Task<List<TopObservationsViewModel>> GetTopObservationsAsync(string username, DateTime startDate)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("method argument is null or empty", nameof(username));

        var model = await _dbContext.Observations
                 .AsNoTracking()
                 .Where(a => a.ApplicationUser.UserName == username && a.ObservationDateTime >= startDate)
                 .GroupBy(b => new { b.Bird.BirdId, b.Bird.EnglishName })
                 .Select(b => new TopObservationsViewModel
                 {
                     BirdId = b.Key.BirdId,
                     Name = b.Key.EnglishName,
                     Count = b.Count()
                 })
                 .OrderByDescending(c => c.Count)
                 .Take(5)
                 .AsQueryable()
                 .ToListAsync();

        return model;
    }
}