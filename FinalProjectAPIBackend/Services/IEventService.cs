using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Event;

namespace FinalProjectAPIBackend.Services
{
    /// <summary>
    /// Interface for the event service.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="insertDTO">The event to create.</param>
        /// <returns>The created event, or null if the operation failed.</returns>
        Task<Event?> CreateEventAsync(EventInsertDTO insertDTO);

        /// <summary>
        /// Finds an event by ID.
        /// </summary>
        /// <param name="eventId">The ID of the event to find.</param>
        /// <returns>The event with the specified ID, or null if no such event exists.</returns>
        Task<Event?> FindEventAsync(int eventId);

        /// <summary>
        /// Finds all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        Task<List<Event>> FindAllEventsAsync();

        /// <summary>
        /// Finds all upcoming events.
        /// </summary>
        /// <returns>A list of all upcoming events.</returns>
        Task<List<Event>> FindAllUpcomingEventsAsync();

        /// <summary>
        /// Finds all past events.
        /// </summary>
        /// <returns>A list of all past events.</returns>
        Task<List<Event>> FindAllPastEventsAsync();

        /// <summary>
        /// Finds events by performer.
        /// </summary>
        /// <param name="title">The title of the performer.</param>
        /// <returns>A list of events performed by the specified performer.</returns>
        Task<List<Event>> FindAllEventsWithPerformerAsync(string title);

        /// <summary>
        /// Finds events on a specific date.
        /// </summary>
        /// <param name="date">The date to search for.</param>
        /// <returns>A list of events on the specified date.</returns>
        Task<List<Event>> FindAllEventsOnDateAsync(DateOnly date);

        /// <summary>
        /// Finds events at a specific venue.
        /// </summary>
        /// <param name="venueName">The name of the venue.</param>
        /// <returns>A list of events at the specified venue.</returns>
        Task<List<Event>> FindAllEventsAtVenueAsync(string venueName);

        /// <summary>
        /// Finds saved events for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of saved events for the specified user.</returns>
        Task<List<Event>> FindAllSavedEventsAsync(int userId);

        /// <summary>
        /// Finds events by title.
        /// </summary>
        /// <param name="title">The title of the event.</param>
        /// <returns>A list of events with the specified title.</returns>
        Task<List<Event>> FindAllEventsWithTitleAsync(string title);

        /// <summary>
        /// Finds all events filtered.
        /// </summary>
        /// <param name="pageNumber">The page number to fetch.</param>
        /// <param name="pageSize">The page size to fetch.</param>
        /// <param name="filtersDTO">The filters to apply.</param>
        /// <returns>A list of events filtered by the specified criteria.</returns>
        Task<List<Event>> GetAllEventsFilteredAsync(int pageNumber, int pageSize, EventFiltersDTO filtersDTO);

        /// <summary>
        /// Finds all dates with events.
        /// </summary>
        /// <returns>A list of dates with events.</returns>
        Task<List<DateOnly?>> FindAllDatesWithEventsAsync();

        /// <summary>
        /// Finds all events listed under a specified category.
        /// </summary>
        /// <param name="category">The event category to filter by.</param>
        /// <returns>The list of events with that event category.</returns>
        Task<List<Event>> FindAllUpcomingEventsInCategoryAsync(string category);

        /// <summary>
        /// Checks if an event is saved by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>true if the event is saved, false otherwise.</returns>
        Task<bool> IsEventSavedAsync(int userId, int eventId);

        /// <summary>
        /// Updates an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to update.</param>
        /// <param name="updateDTO">The event information to update.</param>
        /// <param name="eventImage">The new event image (optional).</param>
        /// <returns>The updated event, or null if the operation failed.</returns>
        Task<Event?> UpdateEventAsync(int eventId, EventUpdateDTO updateDTO, IFormFile eventImage);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to delete.</param>
        /// <returns>tThe deleted event, or null if the operation failed.</returns>
        Task<Event?> DeleteEventAsync(int eventId);

        /// <summary>
        /// Finds events by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of events associated with the specified user.</returns>
        Task<IEnumerable<EventReadOnlyDTO>> FindEventsByUserIdAsync(int userId);

        /// <summary>
        /// Saves an event for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="eventId">The ID of the event to save.</param>
        /// <returns>true if the event was saved, false otherwise.</returns>
        Task<bool> SaveEventAsync(int userId, int eventId);

        /// <summary>
        /// Un-saves an event for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="eventId">The ID of the event to un-save.</param>
        /// <returns>true if the event was un-saved, false otherwise.</returns>
        Task<bool> UnsaveEventAsync(int userId, int eventId);
    }
}
