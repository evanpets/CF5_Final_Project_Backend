using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.Repositories;
using FinalProjectAPIBackend.Services.Exceptions;


namespace FinalProjectAPIBackend.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Event?> CreateEvent(EventInsertDTO insertDTO)
        {
            var eventEntity = ExtractEvent(insertDTO);
            if (insertDTO.NewVenue != null)
            {
                var venue = _mapper.Map<Venue>(insertDTO.NewVenue);
                eventEntity.Venue = venue;
            }

            if (insertDTO.NewPerformers != null && insertDTO.NewPerformers.Any())
            {
                var performers = insertDTO.NewPerformers.Select(p => _mapper.Map<Performer>(p)).ToList();
                eventEntity.Performers = performers;
            }
            else if (insertDTO.PerformerIds != null && insertDTO.PerformerIds.Any())
            {
                var performers = await _unitOfWork.PerformerRepository.GetPerformersAsync(insertDTO.PerformerIds);
                eventEntity.Performers = performers.ToList();
            }

            await _unitOfWork.EventRepository.AddAsync(eventEntity);
            await _unitOfWork.SaveAsync();
            return eventEntity;
        }

        public async Task<Event?> DeleteEvent(int eventId)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventAsync(eventId);
            if (eventEntity == null) return null;

            await _unitOfWork.EventRepository.DeleteAsync(eventId);

            return eventEntity;
        }
        public async Task<List<Event>> FindAllEvents()
        {
            return await _unitOfWork.EventRepository.GetAllEventsAsync();
        }

        public async Task<List<Event>> FindAllPastEvents()
        {
            return await _unitOfWork.EventRepository.FindAllPastEvents();
        }

        public async Task<List<Event>> FindAllUpcomingEvents()
        {
            return await _unitOfWork.EventRepository.FindAllUpcomingEvents();
        }

        public async Task<Event?> FindEvent(int eventId)
        {
            return await _unitOfWork.EventRepository.GetEventAsync(eventId);
        }

        public async Task<Event?> FindEventByTitle(string title)
        {
            return await _unitOfWork.EventRepository.GetEventByTitleAsync(title);
        }

        public async Task<List<Event>> FindEventsWithPerformer(string performer)
        {
            return await _unitOfWork.EventRepository.GetAllEventsWithPerformerAsync(performer);
        }

        public async Task<List<Event>> GetAllEventsAtVenue(string venueName)
        {
            return await _unitOfWork.EventRepository.GetAllEventsAtVenueAsync(venueName);
        }

        public async Task<List<Event>> GetAllEventsFiltered(int pageNumber, int pageSize, EventFiltersDTO eventFiltersDTO)
        {
            List<Event> events = new();
            List<Func<Event, bool>> filters = new();

            try
            {
                if (!string.IsNullOrEmpty(eventFiltersDTO.Title))
                {
                    filters.Add(u => u.Title == eventFiltersDTO.Title);
                }
                if (eventFiltersDTO.VenueId.HasValue)
                {
                    filters.Add(u => u.VenueId == eventFiltersDTO.VenueId);
                }
                if (eventFiltersDTO.Performers is not null && eventFiltersDTO.Performers.Any())
                {
                    filters.Add(u => u.Performers == eventFiltersDTO.Performers);
                }
                if (eventFiltersDTO.Price.HasValue)
                {
                    filters.Add(u => u.Price == eventFiltersDTO.Price);
                }
                if (eventFiltersDTO.Date.HasValue)
                {
                    filters.Add(u => u.Date == eventFiltersDTO.Date);
                }

                events = await _unitOfWork!.EventRepository.GetAllEventsFilteredAsync(pageNumber, pageSize,
                    filters);
                _logger!.LogInformation("{Message}", "Success in returning filtered events.");
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return events;
        }

        public async Task<List<Event>> GetAllEventsOnDate(DateOnly date)
        {
            return await _unitOfWork.EventRepository.GetAllEventsOnDateAsync(date);
        }
        public async Task<List<DateOnly>> GetAllDatesWithEvents()
        {
            return await _unitOfWork.EventRepository.GetAllDatesWithEvents();
        }

        public async Task<Event?> UpdateEvent(int eventId, EventUpdateDTO updateDTO)
        {
            Event? existingEvent = null;
            Event? eventEntity = null;

            try
            {
                existingEvent = await _unitOfWork.EventRepository.GetEventAsync(eventId);
                if (existingEvent is null) return null;

                existingEvent.Title = updateDTO.Title;
                existingEvent.Description = updateDTO.Description;
                existingEvent.Venue = await _unitOfWork.VenueRepository.GetVenueAsync(updateDTO.VenueId);
                existingEvent.Price = updateDTO.Price;
                existingEvent.Performers = await _unitOfWork.PerformerRepository.GetAllPerformersInEventAsync(updateDTO.EventId)!;
                existingEvent.Date = updateDTO.Date;
                existingEvent.Category = updateDTO.Category;

                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Event updated successfully");
            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "The event was not found.");
                throw;
            }
            return eventEntity;
        }

        private Event ExtractEvent(EventInsertDTO insertDTO)
        {
            var eventEntity = new Event()
            {
                Title = insertDTO.Title,
                Description = insertDTO.Description,
                VenueId = insertDTO.VenueId,
                Venue = insertDTO.NewVenue is not null ? new Venue
                {
                    Name = insertDTO.NewVenue.Name,
                    VenueAddress = new VenueAddress
                    {
                        Street = insertDTO.NewVenue.VenueAddress!.Street,
                        StreetNumber = insertDTO.NewVenue.VenueAddress.StreetNumber,
                        ZipCode = insertDTO.NewVenue.VenueAddress.ZipCode
                    }
                } : null,
                Price = insertDTO.Price,
                Performers = new List<Performer>(),
                Date = insertDTO.Date,
                Category = insertDTO.Category!.Value
            };

            if (insertDTO.NewPerformers is not null)
            {
                foreach (var performer in insertDTO.NewPerformers)
                {
                    eventEntity.Performers.Add(new Performer
                    {
                        Name = performer.Name
                    });
                }
            }
            return eventEntity;
        }
    }
}
