using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Birder.Data.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<TEntity> _entities;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetAsync(int id)
    {
        return await _entities.FindAsync(id);
        //return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        //return await _dbContext.Set<TEntity>().ToListAsync();
        return await _entities.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        // return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        // return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        return await _entities.FirstOrDefaultAsync(predicate);
    }

    public void Add(TEntity entity)
    {
        // _dbContext.Set<TEntity>().Add(entity);
        _entities.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        // _dbContext.Set<TEntity>().AddRange(entities);
        _entities.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        // _dbContext.Set<TEntity>().Remove(entity);
        _entities.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        //_dbContext.Set<TEntity>().RemoveRange(entities);
        _entities.RemoveRange(entities);
    }
}