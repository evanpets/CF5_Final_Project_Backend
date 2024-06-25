namespace FinalProjectAPIBackend.Repositories
{
    /// <summary>
    /// the unit of work,.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// The user repository.
        /// </summary>
        public UserRepository UserRepository { get; }

        /// <summary>
        /// The event repository.
        /// </summary>
        public EventRepository EventRepository { get; }

        /// <summary>
        /// The venue repository.
        /// </summary>
        public VenueRepository VenueRepository { get; }

        /// <summary>
        /// The performer repository.
        /// </summary>
        public PerformerRepository PerformerRepository { get; }

        /// <summary>
        /// Saves all changes to the database.
        /// </summary>
        /// <returns>True if the save operation was successful, false otherwise.</returns>
        Task<bool> SaveAsync();
    }
}
