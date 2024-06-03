using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Repositories
{
    public interface IEventRepository
    {
        Task<Event?> GetEventAsync(int id);
        Task<List<Event>> GetAllEventsAsync();
        Task<List<Event>> FindAllUpcomingEvents();
        Task<List<Event>> FindAllPastEvents();
        Task<Event?> GetEventByTitleAsync(string title);
        Task<List<Event>> GetAllEventsOnDateAsync(DateOnly date);
        //Task<Event?> GetEventByDateAndVenueAsync(DateTimeOffset date, string venue);
        Task<List<Event>> GetEventsByCategoryAsync(string category);
        Task<List<Event>> GetAllEventsAtVenueAsync(string venue);
        Task<List<Event>> GetAllEventsWithPerformerAsync(string performer);
        Task<List<DateOnly>> GetAllDatesWithEvents();
        Task<Event?> UpdateEventAsync(Event updatedEvent);
        Task<List<Event>> GetAllEventsFilteredAsync(int pageNumber, int pageSize, List<Func<Event, bool>> filters);

    }
}
