using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.DTO.Venue;
using FinalProjectAPIBackend.Services;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FinalProjectAPIBackend.Controllers
{
    /// <summary>
    /// A controller for methods related to events.
    /// </summary>
    [ApiController]
    [Route("api/events")]
    public class EventController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        /// <summary>
        /// Injects the controller with the services needed.
        /// </summary>
        /// <param name="applicationService">The service layer.</param>
        /// <param name="mapper">Allows mapping entities to DTOs.</param>
        public EventController(IApplicationService applicationService, IMapper mapper)
            : base(applicationService)
        {
            _mapper = mapper;
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }

        /// <summary>
        /// Inserts an event to the database.
        /// </summary>
        /// <param name="eventImage">An image connected to the event.</param>
        /// <param name="eventEntity">The event to be inserted.</param>
        /// <returns>A creation success code and the created event.</returns>
        /// <exception cref="InvalidCreationException">A problem that occurs during the event's creation.</exception>
        /// <exception cref="ServerGenericException">A problem in accessing the server's services.</exception>
        [HttpPost("new")]
        public async Task<ActionResult<EventReadOnlyDTO>> CreateEvent(IFormFile eventImage, [FromForm] string eventEntity)
        {
            var insertDTO = JsonConvert.DeserializeObject<EventInsertDTO>(eventEntity);

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                throw new InvalidCreationException("Error In Event Creation: " + errors);
            }

            if (_applicationService == null)
            {
                throw new ServerGenericException("Application Service Null");
            }

            string? imageUrl = null;

            if (eventImage != null)
            {
                var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(eventImage.FileName)}";
                var imagePath = Path.Combine(_uploadFolderPath, imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await eventImage.CopyToAsync(stream);
                }

                imageUrl = Path.Combine("Uploads", imageFileName);
            }

            insertDTO!.ImageUrl = imageUrl;
            Event? newEvent = await _applicationService.EventService.CreateEventAsync(insertDTO);

            if (newEvent is null)
            {
                throw new InvalidCreationException("Problem during creation");
            }

            var newEventReadOnlyDTO = _mapper.Map<EventReadOnlyDTO>(newEvent);
            return Created("api/events/" + newEventReadOnlyDTO.EventId, newEventReadOnlyDTO);
        }

        /// <summary>
        /// Marks an event as saved (bookmarked).
        /// </summary>
        /// <param name="saveDto">The DTO containing the user and event the save is linked to.</param>
        /// <returns>A confirmation code for the save.</returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveEvent([FromBody] EventSaveDTO saveDto)
        {
            try
            {
                var userId = AppUser!.Id;
                var success = await _applicationService.EventService.SaveEventAsync(userId, saveDto.EventId);
                if (success)
                {
                    return Ok(new { message = "Event saved successfully" });
                }
                return BadRequest(new { message = "Failed to unsave event" });
            }
            catch (Exception) 
            {
                return BadRequest(new { message = "Failed to save event" });

            }

        }

        /// <summary>
        /// Marks an event as unsaved (bookmarked).
        /// </summary>
        /// <param name="saveDto">The DTO containing the user and event the save is linked to.</param>
        /// <returns>A confirmation code for the removal of the save status.</returns>
        [HttpPost("unsave")]
        public async Task<IActionResult> UnsaveEvent([FromBody] EventSaveDTO saveDto)
        {
            try
            {
                var userId = AppUser!.Id;
                var success = await _applicationService.EventService.UnsaveEventAsync(userId, saveDto.EventId);
                if (success)
                {
                    return Ok(new { message = "Event unsaved successfully" });
                }
                return BadRequest(new { message = "Failed to unsave event" });

            }
            catch (Exception) 
            {
                return BadRequest(new { message = "Failed to unsave event" });
            }
        }



        /// <summary>
        /// Updates an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to be updated.</param>
        /// <param name="eventToUpdate">The new information for the event.</param>
        /// <param name="eventImage">The image for the event.</param>
        /// <returns>The updated event.</returns>
        [HttpPatch("{eventId}")]
        public async Task<ActionResult<EventReadOnlyDTO>> UpdateEvent(int eventId, [FromForm] string eventToUpdate, IFormFile? eventImage)
        {
            try
            {
                var updateDTO = JsonConvert.DeserializeObject<EventUpdateDTO>(eventToUpdate);
                var updatedEvent = await _applicationService.EventService.UpdateEventAsync(eventId, updateDTO!, eventImage);

                if (updatedEvent == null)
                {
                    return NotFound();
                }

                var returnedEventDTO = _mapper.Map<EventReadOnlyDTO>(updatedEvent);
                return Ok(new { msg = "Event updated successfully", updatedEvent = returnedEventDTO });
            }
            catch (ServerGenericException ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the event", message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to be deleted.</param>
        /// <returns>A code message confirming the delete.</returns>
        [HttpDelete("{eventId}")]
        public async Task<ActionResult> DeleteEvent(int eventId)
        {
            var deletedEvent = await _applicationService.EventService.DeleteEventAsync(eventId);
            if (deletedEvent == null)
            {
                return NotFound(new { msg = "Event not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Fetches a list of all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        /// <exception cref="EventNotFoundException">If no events are found.</exception>
        [HttpGet]
        public async Task<ActionResult<List<EventReadOnlyDTO>>> GetAllEvents()
        {
            var eventsList = await _applicationService.EventService.FindAllEventsAsync();

            if (eventsList.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No events found");
            }

            return Ok(eventsList);
        }

        /// <summary>
        /// Checks if an event's saved (bookmarked) status is true or false for a given user.
        /// </summary>
        /// <param name="userId">The ID of the requesting user.</param>
        /// <param name="eventId">The event for which the request is being made.</param>
        /// <returns>A success code and the save status.</returns>
        [HttpGet("is-saved/{eventId}")]
        public async Task<ActionResult> IsEventSaved([FromQuery] int userId, [FromQuery] int eventId)
        {
            try
            {
                var isSaved = await _applicationService.EventService.IsEventSavedAsync(userId, eventId);
                return Ok(new { isSaved });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "An error occurred while checking if the event is saved.", error = ex.Message });
            }
        }

        /// <summary>
        /// Fetches a list of all the events saved (bookmarked) by a given user.
        /// </summary>
        /// <param name="userId">The ID of the requesting user.</param>
        /// <returns>A success message and the list of the saved events.</returns>
        [HttpGet("saved/{userId}")]
        public async Task<ActionResult<List<EventReadOnlyDTO>>> GetSavedEvents(int userId)
        {
            var savedEventsList = await _applicationService.EventService.FindAllSavedEventsAsync(userId);
            if (!savedEventsList.Any())
            {
                return StatusCode(400, new { msg = "No saved events found" });
            }

            return Ok(new { msg = "Saved events successfully fetched.", savedEventsList });
        }

        /// <summary>
        /// Fetches an event by its ID.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>The event with the request's ID.</returns>
        /// <exception cref="EventNotFoundException">If the event doesn't exist.</exception>
        [HttpGet("id/{eventId}")]
        public async Task<ActionResult<EventReadOnlyDTO>> GetEventById(int eventId)
        {
            var existingEvent = await _applicationService.EventService.FindEventAsync(eventId);
            if (existingEvent is null)
            {
                throw new EventNotFoundException("Event not found");
            }
            var eventReadOnlyDTO = _mapper.Map<EventReadOnlyDTO>(existingEvent);
            return Ok(eventReadOnlyDTO);
        }

        /// <summary>
        /// Fetches an venue by its ID.
        /// </summary>
        /// <param name="venueId">The ID of the venue.</param>
        /// <returns>The venue with the request's ID.</returns>
        /// <exception cref="VenueNotFoundException"></exception>
        [HttpGet("venues/{venueId}")]
        public async Task<ActionResult<VenueReadOnlyDTO>> GetVenueById(int venueId)
        {
            var existingVenue = await _applicationService.VenueService.FindVenueByIdAsync(venueId);
            if (existingVenue is null)
            {
                throw new VenueNotFoundException("Event not found");
            }
            var venueReadOnlyDTO = _mapper.Map<VenueReadOnlyDTO>(existingVenue);
            return Ok(venueReadOnlyDTO);
        }

        /// <summary>
        /// Fetches an venue by its name.
        /// </summary>
        /// <param name="name">The name of the venue.</param>
        /// <returns>The venue with the name input.</returns>
        /// <exception cref="EventNotFoundException">If a venue with the name given doesn't exist.</exception>
        [HttpGet("venues")]
        public async Task<ActionResult<VenueReadOnlyDTO>> GetVenueByName([FromQuery] string name)
        {
            var existingVenue = await _applicationService.VenueService.FindVenueByNameAsync(name);

            if (existingVenue is null)
            {
                throw new EventNotFoundException("Event not found");
            }
            var venueReadOnlyDTO = _mapper.Map<VenueReadOnlyDTO>(existingVenue);
            return Ok(venueReadOnlyDTO);
        }

        /// <summary>
        /// Fetches a list of all events whose title contains a user-given string.
        /// </summary>
        /// <param name="title">The string input by which the user is searching for an event.</param>
        /// <returns>A list of all the events containing the string.</returns>
        /// <exception cref="EventNotFoundException">If no events containing the string exist.</exception>
        [HttpGet("by-title/{title}")]
        public async Task<ActionResult<List<EventReadOnlyDTO>>> GetAllEventsWithTitle(string title)
        {
            var eventsWithTitle = await _applicationService.EventService.FindAllEventsWithTitleAsync(title);
            if (eventsWithTitle.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No events with title found");
            }
            var eventDTOs = new List<EventReadOnlyDTO>();
            foreach (var eventEntity in eventsWithTitle)
            {
                eventDTOs.Add(_mapper.Map<EventReadOnlyDTO>(eventEntity));
            }
            return Ok(eventDTOs);
        }

        /// <summary>
        /// Fetches a list of all events submitted by a given user.
        /// </summary>
        /// <param name="userId">The ID of the given user.</param>
        /// <returns>The list of the events submitted by that user.</returns>
        /// <exception cref="EventNotFoundException">If no events submitted by that user have been found.</exception>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<EventReadOnlyDTO>>> GetAllUserEvents(int userId)
        {
            var events = await _applicationService.EventService.FindEventsByUserIdAsync(userId);
            if (events.IsNullOrEmpty())
            {
                throw new EventNotFoundException("Event not found");
            }
            return Ok(events);
        }

        /// <summary>
        /// Fetches the name of the venue requested and validates it as unique for insertion or rejects it.
        /// </summary>
        /// <param name="venueName">The name for a venue.</param>
        /// <returns>A confirmation of the check and a message pertaining to success or failure of the request.</returns>
        [HttpGet("check-duplicate-venue")]
        public async Task<IActionResult> CheckDuplicateVenue(string venueName)
        {
            try
            {
                var existingVenue = await _applicationService.VenueService.FindVenueByNameAsync(venueName);
                if (existingVenue is not null)
                {
                    return Ok(new { msg = "Venue name already in use" });
                }
                return Ok(new { msg = "Venue name not registered yet" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        /// <summary>
        /// Returns a list of items depending on a filter chosen by the user.
        /// </summary>
        /// <param name="filter">The filter to be applied.</param>
        /// <returns>The list of items linked to the filter.</returns>
        [HttpGet("filter-events")]
        public async Task<ActionResult<List<Object>>> FilterEvents(string filter)
        {
            switch (filter.ToLower()) {
                case "venue":
                    return Ok(await _applicationService.VenueService.FindAllVenuesAsync());
                case "date":
                    return Ok(await _applicationService.EventService.FindAllDatesWithEventsAsync());
                case "performer":
                    return Ok(await _applicationService.PerformerService.FindAllPerformersAsync());
                default:
                    return Ok(await _applicationService.EventService.FindAllEventsAsync());

            }
        }
        /// <summary>
        /// Returns all events belonging to a specified category.
        /// </summary>
        /// <param name="category">The category by which to filter events.</param>
        /// <returns>A list of events listed under that category.</returns>
        [HttpGet("{category}")]
        public async Task<ActionResult<List<Event>>> GetAllUpcomingEventsInCategory(string category)
        {
            if (category == null)
            {
                return BadRequest("String is null");
            }
            var eventsList = await _applicationService.EventService.FindAllUpcomingEventsInCategoryAsync(category);

            if (eventsList.IsNullOrEmpty())
            {
                return BadRequest("No events were found matching the requested category");
            }

            return Ok( new { msg = $"Events were found under the category: \"{category}\"", eventsList });
        }

        /// <summary>
        /// Fetches all current or future events.
        /// </summary>
        /// <returns>A list of all upcoming events.</returns>
        /// <exception cref="EventNotFoundException">If no events fitting the criterion are found.</exception>
        [HttpGet("upcoming")]
        public async Task<ActionResult<List<Event>>> GetAllUpcomingEvents()
        {
            var upcomingEvents = await _applicationService.EventService.FindAllUpcomingEventsAsync();
            if (upcomingEvents.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No upcoming events found");
            }

            return Ok(upcomingEvents);
        }

        /// <summary>
        /// Fetches all past events.
        /// </summary>
        /// <returns>A list of all past events.</returns>
        /// <exception cref="EventNotFoundException">If no events fitting the criterion are found.</exception>
        [HttpGet("past")]
        public async Task<ActionResult<List<Event>>> GetAllPastEvents()
        {
            var pastEvents = await _applicationService.EventService.FindAllPastEventsAsync();
            if ( pastEvents.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No past events found");
            }
            return Ok(pastEvents);
        }

        /// <summary>
        /// Fetches a list of all unique venues inserted in the DB.
        /// </summary>
        /// <returns>A list of all inserted venues</returns>
        /// <exception cref="VenueNotFoundException">If no venues are found.</exception>
        [HttpGet("venues/registered")]
        public async Task<ActionResult<List<Venue>>> GetAllRegisteredVenues()
        {
            var registeredVenues = await _applicationService.VenueService.FindAllVenuesAsync();
            if (registeredVenues.IsNullOrEmpty())
            {
                throw new VenueNotFoundException("No venues found");
            }
            return Ok(registeredVenues);

        }
    }
}
