namespace Birder.Data.Repository;
public interface IUnitOfWork
    {
        Task CompleteAsync();
    }

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task CompleteAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
