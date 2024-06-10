using FinalProjectAPIBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectAPIBackend.Repositories
{
    public class PerformerRepository : BaseRepository<Performer>, IPerformerRepository
    {
        public PerformerRepository(FinalProjectAPIBackendDbContext context) : base(context)
        {
        }

        public async Task<Performer?> GetPerformerAsync(int performerId)
        {
            var performer = await _context.Performers.FindAsync(performerId);
            return performer;
        }

        public async Task<Performer?> GetPerformerByNameAsync(string name)
        {
            return await _context.Performers.Where(p => p.Name == name).FirstOrDefaultAsync();
            
        }

        public async Task<List<Performer>> GetPerformersAsync(ICollection<int> performerIds) ///??? this or below?
        {
            return await _context.Performers
                .Where(p => performerIds.Contains(p.PerformerId)).ToListAsync();
        }

        public async Task<List<Performer>> GetAllPerformersAsync()
        {
            return await _context.Performers.ToListAsync();
        }

        public async Task<List<Performer>> GetAllPerformersWithNameAsync(string name)
        {
            return await _context.Performers.Where(p => p.Name!.Contains(name)).ToListAsync();
        }

        public async Task<List<Performer>> GetAllPerformersInEventAsync(int eventId)
        {
            return await _context.Performers
                .Include(p => p.Events)
                .Where(p => p.Events.Any(e => e.EventId == eventId))
                .ToListAsync();
        }

        public async Task<Performer?> UpdatePerformerInformationAsync(Performer updatedPerformer)
        {
            var existingPerformer = await _context.Performers.FindAsync(updatedPerformer.PerformerId);
            if (existingPerformer is null) return null;

            _context.Performers.Entry(existingPerformer).CurrentValues.SetValues(updatedPerformer);
            return updatedPerformer;

        }
    }
}
