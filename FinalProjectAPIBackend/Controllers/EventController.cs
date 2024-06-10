using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.Services;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FinalProjectAPIBackend.Controllers
{
    [ApiController]
    [Route("api/events")]

    public class EventController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EventController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<ActionResult<EventReadOnlyDTO>> CreateEventAsync(EventInsertDTO insertDTO)
        {
            //Console.WriteLine(insertDTO.ToString());
            Console.WriteLine("User id: " + insertDTO.UserId);

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
            Event? newEvent = await _applicationService.EventService.CreateEvent(insertDTO);

            if (newEvent is null)
            {
                throw new InvalidCreationException("Problem during creation");
            }
            var newEventReadOnlyDTO = _mapper.Map<EventReadOnlyDTO>(newEvent);
            Console.WriteLine("New Event:" + GetEventById(newEventReadOnlyDTO.EventId) + "Read DTO:" + newEventReadOnlyDTO.ToString());
            //return CreatedAtAction(nameof(GetEventById), new { Id = newEventReadOnlyDTO.EventId }, newEventReadOnlyDTO);
            return Ok(new { msg = "Event created successfully",  newEventReadOnlyDTO });
        }

        [HttpGet]
        public async Task<ActionResult<List<EventReadOnlyDTO>>> GetEvents()
        {
            //var eventsList = await _applicationService.EventService.FindAllUpcomingEvents();
            var eventsList = await _applicationService.EventService.FindAllEvents();

            if (eventsList.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No events found");
            }
            foreach (var eventEntity in eventsList)
            {
                Console.WriteLine(eventEntity.Title + " " + eventEntity.Date);
            }
            return Ok(eventsList);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<EventReadOnlyDTO>> GetEventById(int eventId)
        {
            var existingEvent = await _applicationService.EventService.FindEvent(eventId);
            if (existingEvent is null)
            {
                throw new EventNotFoundException("Event not found");
            }
            var eventReadOnlyDTO = _mapper.Map<EventReadOnlyDTO>(existingEvent);
            return Ok(eventReadOnlyDTO);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<EventReadOnlyDTO>>> GetUserEvents(int userId)
        {
            var events = await _applicationService.EventService.GetEventsByUserIdAsync(userId);
            if (events.IsNullOrEmpty())
            {
                throw new EventNotFoundException("Event not found");
            }
            //foreach(var eventEntity in events)
            //{
            //    Console.WriteLine(eventEntity);

            //}
            //return Ok( new { msg = "Events of user fetched successfully", events });
            return Ok(events);
        }

        [HttpPatch("update/{eventId}")]
        public async Task<ActionResult<EventReadOnlyDTO>> UpdateEventAsync(int eventId, EventUpdateDTO? eventDTO)
        {
            Console.WriteLine($"id: {eventId}");
            Console.WriteLine($"event: {JsonConvert.SerializeObject(eventDTO)}");
            var updatedEvent = await _applicationService.EventService.UpdateEvent(eventId, eventDTO!);
            Console.WriteLine($"updated: {JsonConvert.SerializeObject(updatedEvent)}");
            //if (updatedEvent is null)
            //{
            //    return BadRequest(new { msg = "Event update failed" });
            //}
            var returnedEventDTO = _mapper.Map<EventReadOnlyDTO>(updatedEvent);
            return Ok(new { msg = "Event updated successfully", backendEvent = returnedEventDTO });
        }

        [HttpDelete("delete/{eventId}")]
        public async Task<ActionResult> DeleteEvent(int eventId)
        {
            var deletedEvent = await _applicationService.EventService.DeleteEvent(eventId);
            if (deletedEvent == null)
            {
                return NotFound(new { msg = "Event not found" });
            }

            return NoContent();
        }

        [HttpGet("check-duplicate-venue")]
        public async Task<IActionResult> CheckDuplicateVenue(string venueName)
        {
            try
            {
                var existingVenue = await _applicationService.VenueService.FindVenueByName(venueName);
                if (existingVenue is not null)
                { 
                    return Ok(new { msg = "Venue name already in use" });
                }
                return Ok(new { msg = "Venue name not registered yet" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in checking venue");
                Console.WriteLine(ex);
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("filter-events")]
        public async Task<ActionResult<List<Object>>> FilterEvents(string filter)
        {
            switch (filter){
                case "venue":
                    return Ok(await _applicationService.VenueService.GetAllVenues());
                case "date":
                    return Ok(await _applicationService.EventService.GetAllDatesWithEvents());
                case "performer":
                    return Ok(await _applicationService.PerformerService.FindAllPerformers());
                default:
                    return Ok(await _applicationService.EventService.FindAllEvents());

            }
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<Event>>> GetUpcomingEventsAsync()
        {
            var upcomingEvents = await _applicationService.EventService.FindAllUpcomingEvents();
            if (upcomingEvents.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No upcoming events found");
            }
            foreach (var eventEntity in upcomingEvents) {
                Console.WriteLine("Venue: " + eventEntity.Venue!.Name + "Performer: " + eventEntity.Performers!.ToList());
            }
            return Ok(upcomingEvents);
        }

        [HttpGet("past")]
        public async Task<ActionResult<List<Event>>> GetPastEventsAsync()
        {
            var pastEvents = await _applicationService.EventService.FindAllPastEvents();
            if ( pastEvents.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No past events found");
            }
            return Ok(pastEvents);
        }

        [HttpGet("venues")]
        public async Task<ActionResult<List<Venue>>> GetRegisteredVenues()
        {
            var registeredVenues = await _applicationService.VenueService.GetAllVenues();
            if (registeredVenues.IsNullOrEmpty())
            {
                throw new VenueNotFoundException("No venues found");
            }
            return Ok(registeredVenues);

        }
    }
}
