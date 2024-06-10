using FinalProjectAPIBackend.Data;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProjectAPIBackend.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(FinalProjectAPIBackendDbContext context) : base(context)
        {
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            var events = await _context.Events.Include(e => e.Venue).Include(e => e.Performers).ToListAsync();
            //var events = await _context.Events.ToListAsync();

            return events;
        }

        public async Task<Event?> GetEventAsync(int eventId)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Venue).ThenInclude(v=>v.VenueAddress).Include(e => e.Performers)
                .FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventEntity == null) return null;
            return eventEntity;
        }

        //public async Task<Event?> GetEventByDateAndVenueAsync(DateTimeOffset date, string venue)
        //{
        //   return await _context.Events
        //        .Include(e => e.Venue).Include(e => e.Performers)
        //        .FirstOrDefaultAsync(e => e.Date == date && e.Venue!.Name == venue);
        //}

        public async Task<Event?> GetEventByTitleAsync(string title)
        {
            return await _context.Events
                .Include(e => e.Venue).ThenInclude(v => v!.VenueAddress).Include(e => e.Performers)
                .Where(e => e.Title!.Contains(title)).FirstOrDefaultAsync();
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

        public async Task<List<Event>> FindAllUpcomingEvents()
        {
            return await _context.Events
            .Include(e => e.Venue).ThenInclude(v=>v.VenueAddress).Include(e => e.Performers)
            .Where(e => e.Date >= DateOnly.FromDateTime(DateTime.UtcNow.Date)).OrderBy(e => e.Date).ToListAsync();
        }

        public async Task<List<Event>> FindAllPastEvents()
        {
            return await _context.Events
                .Include(e => e.Venue).Include(e => e.Performers)
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

        public async Task<List<DateOnly>> GetAllDatesWithEvents()
        {
            return await _context.Events.Where(e => e.Date.HasValue).Select(e => e.Date!.Value).ToListAsync();
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
    }
}
