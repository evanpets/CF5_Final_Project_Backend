using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Repositories
{
    /// <summary>
    /// Interface for the event repository.
    /// </summary>
    public interface IEventRepository
    {
        /// <summary>
        /// Gets an event by ID.
        /// </summary>
        /// <param name="eventId">The ID of the event to get.</param>
        /// <returns>The event if found, otherwise null.</returns>
        Task<Event?> GetEventAsync(int eventId);

        /// <summary>
        /// Gets all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        Task<List<Event>> GetAllEventsAsync();

        /// <summary>
        /// Gets all upcoming events.
        /// </summary>
        /// <returns>A list of upcoming events.</returns>
        Task<List<Event>> GetAllUpcomingEventsAsync();

        /// <summary>
        /// Gets all past events.
        /// </summary>
        /// <returns>A list of past events.</returns>
        Task<List<Event>> GetAllPastEventsAsync();

        /// <summary>
        /// Gets events by title.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <returns>A list of events with the given title.</returns>
        Task<List<Event>> GetAllEventsWithTitleAsync(string title);

        /// <summary>
        /// Gets events on a specific date.
        /// </summary>
        /// <param name="date">The date to search for.</param>
        /// <returns>A list of events on the given date.</returns>
        Task<List<Event>> GetAllEventsOnDateAsync(DateOnly date);

        /// <summary>
        /// Gets events by user ID.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>A list of events belonging to the given user.</returns>
        Task<List<Event>> GetEventsByUserIdAsync(int userId);

        /// <summary>
        /// Gets events by category.
        /// </summary>
        /// <param name="category">The category to search for.</param>
        /// <returns>A list of events in the given category.</returns>
        Task<List<Event>> GetEventsByCategoryAsync(string category);

        /// <summary>
        /// Gets events at a specific venue.
        /// </summary>
        /// <param name="venue">The venue to search for.</param>
        /// <returns>A list of events at the given venue.</returns>
        Task<List<Event>> GetAllEventsAtVenueAsync(string venue);
        /// <summary>
        /// Gets events by performer.
        /// </summary>
        /// <param name="performer">The performer to search for.</param>
        /// <returns>A list of events featuring the given performer.</returns>
        Task<List<Event>> GetAllEventsWithPerformerAsync(string performer);

        /// <summary>
        /// Gets all dates with events.
        /// </summary>
        /// <returns>A list of dates with events.</returns>
        Task<List<DateOnly?>> GetAllDatesWithEventsAsync();

        /// <summary>
        /// Gets saved events by user ID.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>A list of saved events belonging to the given user.</returns>
        Task<List<Event>> GetAllSavedEventsAsync(int userId);

        /// <summary>
        /// Checks if an event is saved by a user.
        /// </summary>
        /// <param name="userId">The user ID to check.</param>
        /// <param name="eventId">The event ID to check.</param>
        /// <returns>True if the event is saved, otherwise false.</returns>
        Task<bool> IsEventSavedAsync(int userId, int eventId);

        /// <summary>
        /// Updates an event.
        /// </summary>
        /// <param name="updatedEvent">The updated event.</param>
        /// <returns>The updated event.</returns>
        Task<Event?> UpdateEventAsync(Event updatedEvent);

        /// <summary>
        /// Gets all events filtered by specified criteria.
        /// </summary>
        /// <param name="pageNumber">The page number of the results.</param>
        /// <param name="pageSize">The number of results per page.</param>
        /// <param name="filters">A list of filters to apply to the results.</param>
        /// <returns>A list of events that match the specified filters, paginated.</returns>
        Task<List<Event>> GetAllEventsFilteredAsync(int pageNumber, int pageSize, List<Func<Event, bool>> filters);

        /// <summary>
        /// Fetches an saved event for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>The save status.</returns>
        Task<EventSave?> GetSaveAsync(int userId, int eventId);

        /// <summary>
        /// Saves an event for a user.
        /// </summary>
        /// <param name="save">The save to add or update.</param>
        Task AddSaveAsync(EventSave save);

        /// <summary>
        /// Removes a save.
        /// </summary>
        /// <param name="save">The save to remove.</param>
        void RemoveSave(EventSave save);

        /// <summary>
        /// Gets all saved events for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve saved events for.</param>
        /// <returns>A list of saved events for the specified user.</returns>
        Task<List<Event>> GetSavedEventsByUserIdAsync(int userId);


    }
}
