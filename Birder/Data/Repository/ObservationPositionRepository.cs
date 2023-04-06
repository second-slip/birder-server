namespace Birder.Data.Repository;

    public interface IObservationPositionRepository : IRepository<ObservationPosition> { }

    public class ObservationPositionRepository : Repository<ObservationPosition>, IObservationPositionRepository
    {
        public ObservationPositionRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }