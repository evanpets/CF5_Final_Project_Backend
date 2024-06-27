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
        /// Retrieves an event by ID.
        /// </summary>
        /// <param name="eventId">The ID of the event to get.</param>
        /// <returns>The event if found, otherwise null.</returns>
        Task<Event?> GetEventAsync(int eventId);

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        Task<List<Event>> GetAllEventsAsync();

        /// <summary>
        /// Retrieves all upcoming events.
        /// </summary>
        /// <returns>A list of upcoming events.</returns>
        Task<List<Event>> GetAllUpcomingEventsAsync();

        /// <summary>
        /// Retrieves all past events.
        /// </summary>
        /// <returns>A list of past events.</returns>
        Task<List<Event>> GetAllPastEventsAsync();

        /// <summary>
        /// Retrieves events with a specified title string.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <returns>A list of events with the given title.</returns>
        Task<List<Event>> GetAllEventsWithTitleAsync(string title);

        /// <summary>
        /// Retrieves events on a specific date.
        /// </summary>
        /// <param name="date">The date to search for.</param>
        /// <returns>A list of events on the given date.</returns>
        Task<List<Event>> GetAllEventsOnDateAsync(DateOnly date);

        /// <summary>
        /// Retrieves events by user ID.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>A list of events belonging to the given user.</returns>
        Task<List<Event>> GetEventsByUserIdAsync(int userId);

        /// <summary>
        /// Retrieves events by category.
        /// </summary>
        /// <param name="category">The category to search for.</param>
        /// <returns>A list of events in the given category.</returns>
        Task<List<Event>> GetEventsByCategoryAsync(string category);

        /// <summary>
        /// Retrieves events at a specific venue.
        /// </summary>
        /// <param name="venue">The venue to search for.</param>
        /// <returns>A list of events at the given venue.</returns>
        Task<List<Event>> GetAllEventsAtVenueAsync(string venue);
        /// <summary>
        /// Retrieves events by performer.
        /// </summary>
        /// <param name="performer">The performer to search for.</param>
        /// <returns>A list of events featuring the given performer.</returns>
        Task<List<Event>> GetAllEventsWithPerformerAsync(string performer);

        /// <summary>
        /// Retrieves all dates with events.
        /// </summary>
        /// <returns>A list of dates with events.</returns>
        Task<List<DateOnly?>> GetAllDatesWithEventsAsync();

        /// <summary>
        /// Retrieves saved events by user ID.
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
        /// Retrieves all events filtered by specified criteria.
        /// </summary>
        /// <param name="pageNumber">The page number of the results.</param>
        /// <param name="pageSize">The number of results per page.</param>
        /// <param name="filters">A list of filters to apply to the results.</param>
        /// <returns>A list of events that match the specified filters, paginated.</returns>
        Task<List<Event>> GetAllEventsFilteredAsync(int pageNumber, int pageSize, List<Func<Event, bool>> filters);

        /// <summary>
        /// Retrieves all events listed under a specified category.
        /// </summary>
        /// <param name="category">The event category to filter by.</param>
        /// <returns>The list of events with that event category.</returns>
        Task<List<Event>> GetAllUpcomingEventsInCategoryAsync(string category);

        /// <summary>
        /// Retrieves an saved event for a user.
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
        /// Removes the saved status from an event for a user.
        /// </summary>
        /// <param name="save">The save to remove.</param>
        void RemoveSave(EventSave save);

        /// <summary>
        /// Retrieves all saved events for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve saved events for.</param>
        /// <returns>A list of saved events for the specified user.</returns>
        Task<List<Event>> GetSavedEventsByUserIdAsync(int userId);


    }
}
