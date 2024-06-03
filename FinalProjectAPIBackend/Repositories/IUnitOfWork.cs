namespace FinalProjectAPIBackend.Repositories
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }
        public EventRepository EventRepository { get; }
        public VenueRepository VenueRepository { get; }
        public PerformerRepository PerformerRepository { get; }

        Task<bool> SaveAsync();
    }
}
