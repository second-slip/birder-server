namespace Birder.Data.Repository;

    public interface IObservationNoteRepository : IRepository<ObservationNote> { }

    public class ObservationNoteRepository : Repository<ObservationNote>, IObservationNoteRepository
    {
        public ObservationNoteRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }