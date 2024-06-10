using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.Repositories;
using FinalProjectAPIBackend.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public async Task<IEnumerable<EventReadOnlyDTO>> GetEventsByUserIdAsync(int userId)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<EventReadOnlyDTO>>(events);
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
            Event? existingEvent;

            Console.WriteLine($"Updating event with ID: {eventId}");
            Console.WriteLine($"Update DTO: {updateDTO}");
            try
            {
                existingEvent = await _unitOfWork.EventRepository.GetEventAsync(eventId);
                if (existingEvent == null)
                {
                    Console.WriteLine($"Event with ID {eventId} not found");
                    return null;
                }

                Console.WriteLine($"Existing event retrieved: {existingEvent}");

                existingEvent.Title = updateDTO.Title;
                existingEvent.Description = updateDTO.Description;
                existingEvent.Date = updateDTO.Date;
                existingEvent.Category = updateDTO.Category;
                existingEvent.Price = updateDTO.Price;
                var venue = existingEvent.Venue;
                if (venue != null)
                {
                    venue.Name = updateDTO.VenueName;
                    var venueAddress = venue.VenueAddress;
                    if (venueAddress != null)
                    {
                        venueAddress.Street = updateDTO.VenueStreet;
                        venueAddress.StreetNumber = updateDTO.VenueStreetNumber;
                        venueAddress.ZipCode = updateDTO.VenueZipCode;
                        venueAddress.City = updateDTO.VenueCity;
                    }
                }

                if (updateDTO.Performers != null)
                {
                    // Remove performers not in the update DTO
                    var performerNamesInUpdate = updateDTO.Performers.Select(p => p.Name).ToList();
                    var performersToRemove = existingEvent.Performers!.Where(p => !performerNamesInUpdate.Contains(p.Name)).ToList();

                    foreach (var performerToRemove in performersToRemove)
                    {
                        existingEvent.Performers!.Remove(performerToRemove);
                    }

                    // Add new performers and update existing ones
                    foreach (var performerDto in updateDTO.Performers)
                    {
                        var existingPerformer = existingEvent.Performers!.FirstOrDefault(p => p.Name == performerDto.Name);
                        if (existingPerformer == null)
                        {
                            var newPerformer = new Performer
                            {
                                Name = performerDto.Name
                            };
                            existingEvent.Performers!.Add(newPerformer);
                        }
                    }
                }
                //existingEvent.Performers ??= new List<Performer>();
                //var updatedPerformers = new List<PerformerUpdateDTO>();

                //foreach (var performer in existingEvent.Performers)
                //{
                //    Console.WriteLine($"Name: {performer.Name}");
                //    foreach (var performerDTO in updateDTO.Performers!)
                //    {
                //        if (performer.Name == performerDTO.Name)
                //        {
                //            Console.WriteLine($"Matched performer: {performerDTO.Name}");
                //            updatedPerformers.Add(performerDTO);
                //            break;
                //        }
                //        else
                //        {
                //            Console.WriteLine($"Not matched performer: {performerDTO.Name}");
                //            break;
                //        }
                //    }
                //}
                //existingEvent.Performers = updatedPerformers.Select(p => new Performer { Name = p.Name }).ToList();
                //if (existingEvent.Performers.Count > 0)
                //{
                //    foreach (var performer in existingEvent.Performers!)
                //    {
                //        Console.WriteLine($"Updated performer: {performer.Name}");
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("\nPerformers list is empty\n");
                //}

                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Event updated successfully");
            }
            catch (EventNotFoundException)
            {
                _logger!.LogError("{Message}", "The event was not found.");
                throw;
            }

            return existingEvent;
        }

        public async Task<Event?> DeleteEvent(int eventId)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventAsync(eventId);
            if (eventEntity == null) return null;

            await _unitOfWork.EventRepository.DeleteAsync(eventId);
            await _unitOfWork.SaveAsync();

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
                        ZipCode = insertDTO.NewVenue.VenueAddress.ZipCode,
                        City = insertDTO.NewVenue.VenueAddress.City
                    }
                } : null,
                Price = insertDTO.Price,
                Performers = new List<Performer>(),
                Date = insertDTO.Date,
                Category = insertDTO.Category!.Value,
                UserId = insertDTO.UserId
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
