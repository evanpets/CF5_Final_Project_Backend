using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.DTO.Venue;
using FinalProjectAPIBackend.Services;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            Console.WriteLine(insertDTO.ToString());

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

            return CreatedAtAction(nameof(GetEventById), new { id = newEventReadOnlyDTO.EventId }, newEventReadOnlyDTO);
            //return newEventReadOnlyDTO;
        }

        [HttpGet("get")]
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

        [HttpGet("get/{eventId}")]
        public async Task<ActionResult<EventReadOnlyDTO>> GetEventById(int eventId)
        {
            var existingEvent = await _applicationService.EventService.FindEvent(eventId);
            if (existingEvent is null)
            {
                throw new EventNotFoundException("Event not found");
            }
            return Ok(existingEvent);
        }

        [HttpPut("update/{eventId}")]
        public async Task<ActionResult<EventInsertDTO>> UpdateEventAsync(int eventId, EventUpdateDTO? eventDTO)
        {

            var updatedEvent = await _applicationService.EventService.UpdateEvent(eventId, eventDTO!);
            var returnedEventDTO = _mapper.Map<EventInsertDTO>(updatedEvent);
            return Ok(returnedEventDTO);
        }

        [HttpDelete("delete/{eventId}")]
        public async Task<ActionResult<Event>> DeleteEventAsync(int eventId)
        {
            var deletedEvent = await _applicationService.EventService.DeleteEvent(eventId);
            return deletedEvent != null ? Ok(deletedEvent) : NotFound();
        }

        //[HttpGet] //path
        //public async Task<ActionResult<Venue>> SearchVenueByName(string username)
        //{
        //    var venueToReturn = await _applicationService.VenueService.FindVenueByName(username);
        //    return Ok(venueToReturn);
        //}

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

        //[HttpGet("search-event")]
        //public async Task<ActionResult<List<Event>>> SearchEvent(string searchValue)
        //{
        //    return await _applicationService.EventService.FindEventByTitleAsync(searchValue);
        //}

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<Event>>> GetUpcomingEventsAsync()
        {
            var upcomingEvents = await _applicationService.EventService.FindAllUpcomingEvents();
            if (upcomingEvents.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No upcoming events found");
            }
            return Ok(upcomingEvents);
        }

        [HttpGet("past")]
        public async Task<ActionResult<List<Event>>> GetPastEventsAsync()
        {
            var pastEvents = await _applicationService.EventService.FindAllPastEvents(); //stack overflow
            if ( pastEvents.IsNullOrEmpty())
            {
                throw new EventNotFoundException("No past events found");
            }
            return Ok(pastEvents);
        }
    }
}
