using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Event;

namespace FinalProjectAPIBackend.Services
{
    public interface IEventService
    {
        Task<Event?> CreateEvent(EventInsertDTO insertDTO);
        Task<Event?> FindEvent(int eventId);
        Task<List<Event>> FindAllEvents();
        Task<List<Event>> FindAllUpcomingEvents();
        Task<List<Event>> FindAllPastEvents();
        Task<List<Event>> FindEventsWithPerformer(string title);
        Task<List<Event>> GetAllEventsOnDate(DateOnly date);
        Task<List<Event>> GetAllEventsAtVenue(string venueName);
        Task<Event?> FindEventByTitle(string title);
        Task<List<Event>> GetAllEventsFiltered(int pageNumber, int pageSize, EventFiltersDTO filtersDTO);
        Task<List<DateOnly>> GetAllDatesWithEvents();
        Task<Event?> UpdateEvent(int eventId, EventUpdateDTO updateDTO);
        Task<Event?> DeleteEvent(int eventId);
    }
}
