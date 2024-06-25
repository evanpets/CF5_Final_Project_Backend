using FinalProjectAPIBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectAPIBackend.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(FinalProjectAPIBackendDbContext context) : base(context)
        {
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            var events = await _context.Events.Include(e => e.Venue).Include(e => e.Performers).Include(e => e.Venue!).ThenInclude(v => v!.VenueAddress).ToListAsync();

            return events;
        }

        public async Task<Event?> GetEventAsync(int eventId)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Performers).Include(e => e.Venue).ThenInclude(v => v!.VenueAddress)
                .FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventEntity == null) return null;
            return eventEntity;
        }

        public async Task<List<Event>> GetAllEventsWithTitleAsync(string title)
        {
            return await _context.Events
                .Include(e => e.Performers).Include(e => e.Venue).ThenInclude(v => v!.VenueAddress)
                .Where(e => e.Title!.ToLower().Contains(title.ToLower())).ToListAsync();
        }

        public async Task<List<Event>> GetAllEventsOnDateAsync(DateOnly date)
        {
            return await _context.Events
                .Include(e => e.Venue).ThenInclude(v => v!.VenueAddress).Include(e => e.Performers)
                .Where(e => e.Date == date).ToListAsync();
        }

        public async Task<List<Event>> GetEventsByUserIdAsync(int userId)
        {
            return await _context.Events
                .Include(e => e.Venue)
                .ThenInclude(v => v!.VenueAddress)
                .Include(e => e.Performers)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Event>> GetAllUpcomingEventsAsync()
        {
            return await _context.Events
            .Include(e => e.Venue!).ThenInclude(v => v!.VenueAddress).Include(e => e.Performers)
            .Where(e => e.Date >= DateOnly.FromDateTime(DateTime.UtcNow.Date)).OrderBy(e => e.Date).ToListAsync();
        }

        public async Task<List<Event>> GetAllPastEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Venue!).ThenInclude(v => v.VenueAddress).Include(e => e.Performers)
                .Where(e => e.Date < DateOnly.FromDateTime(DateTime.UtcNow.Date)).ToListAsync();
        }

        public async Task<List<Event>> GetEventsByCategoryAsync(string category)
        {
            return await _context.Events
                .Include(e => e.Venue).Include(e => e.Performers)
                .Where(e => e.Category.ToString()!.Equals(category, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<List<Event>> GetAllEventsAtVenueAsync(string venue)
        {
            return await _context.Events
                .Include(e => e.Venue).Include(e => e.Performers)
                .Where(e => e.Venue!.Name == venue).ToListAsync();
        }

        public async Task<List<Event>> GetAllEventsWithPerformerAsync(string performer)
        {
            var existingPerformer = await _context.Performers.FirstOrDefaultAsync(p => p.Name!.Contains(performer));
            if (existingPerformer is null) return new List<Event>();

            return await _context.Events
                .Include(e => e.Venue).Include(e => e.Performers)
                .Where(e => e.Performers!.Any(p => p.Name == existingPerformer.Name)).ToListAsync();
        }

        public async Task<List<DateOnly?>> GetAllDatesWithEventsAsync()
        {
            return await _context.Events.Select(e => e.Date).Distinct().ToListAsync();
        }

        public async Task<List<Event>> GetAllSavedEventsAsync(int userId)
        {
            return await _context.Events.Include(e => e.EventSaves).Include(e => e.User).Where(e => e.EventSaves!.Any(l => l.UserId == userId)).ToListAsync(); 
        }
        public async Task<bool> IsEventSavedAsync(int userId, int eventId)
        {
            return await _context.EventSaves.AnyAsync(l => l.UserId == userId && l.EventId == eventId);
        }

        public async Task<Event?> UpdateEventAsync(Event updatedEvent)
        {
            var existingEvent = await _context.Events.FindAsync(updatedEvent.EventId);
            if (existingEvent is null) return null;

            _context.Entry(existingEvent).CurrentValues.SetValues(updatedEvent);
            return updatedEvent;
        }

        public async Task<List<Event>> GetAllEventsFilteredAsync(int pageNumber, int pageSize, List<Func<Event, bool>> filters)
        {
            int skip = pageSize * pageNumber;
            IQueryable<Event> query = _context.Events.Skip(skip).Take(pageSize);

            if (filters != null && filters.Any())
            {
                query = query.Where(e => filters.All(filter => filter(e)));
            }

            return await query.ToListAsync();
        }

        public async Task<EventSave?> GetSaveAsync(int userId, int eventId)
        {
            return await _context.EventSaves.FirstOrDefaultAsync(l => l.UserId == userId && l.EventId == eventId);
        }

        public async Task AddSaveAsync(EventSave save)
        {
            await _context.EventSaves.AddAsync(save);
        }

        public void RemoveSave(EventSave save)
        {
             _context.EventSaves.Remove(save);
        }
        public async Task<List<Event>> GetSavedEventsByUserIdAsync(int userId)
        {
            return await _context.Events
                .Include(e => e.EventSaves)
                .Where(e => e.EventSaves!.Any(l => l.UserId == userId))
                .ToListAsync();
        }
    }
}
