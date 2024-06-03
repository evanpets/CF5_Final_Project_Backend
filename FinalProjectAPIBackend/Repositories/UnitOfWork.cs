
using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinalProjectAPIBackendDbContext _context;

        public UnitOfWork(FinalProjectAPIBackendDbContext context)
        {
            _context = context;
        }

        public UserRepository UserRepository => new(_context);

        public EventRepository EventRepository => new(_context);

        public VenueRepository VenueRepository => new (_context);

        public PerformerRepository PerformerRepository => new(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
