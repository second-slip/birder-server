namespace Birder.Data.Repository;

public interface IBirdRepository : IRepository<Bird> { }

public class BirdRepository : Repository<Bird>, IBirdRepository
{
    public BirdRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}