using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.Repositories;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Event?> CreateEventAsync(EventInsertDTO insertDTO)
        {
            var eventEntity = ExtractEvent(insertDTO);
            if (insertDTO.NewVenue != null)
            {
                var venue = _mapper.Map<Venue>(insertDTO.NewVenue);
                eventEntity.Venue = venue;
            }

            if (insertDTO.NewPerformers != null && insertDTO.NewPerformers.Any())
            {
                var performers = new List<Performer>();
                foreach (var performer in insertDTO.NewPerformers)
                {
                    var existingPerformer = await _unitOfWork.PerformerRepository.GetPerformerByNameAsync(performer.Name!);
                    if (existingPerformer != null)
                    {
                        performers.Add(existingPerformer);
                    }
                    else
                    {
                        var newPerformer = _mapper.Map<Performer>(performer);
                        performers.Add(newPerformer);
                    }
                }
                eventEntity.Performers = performers;

            }
            else if (insertDTO.PerformerIds != null && insertDTO.PerformerIds.Any())
            {
                var performers = await _unitOfWork.PerformerRepository.GetPerformersRangeAsync(insertDTO.PerformerIds);
                eventEntity.Performers = performers.ToList();
            }

            await _unitOfWork.EventRepository.AddAsync(eventEntity);
            await _unitOfWork.SaveAsync();
            return eventEntity;
        }

        public async Task<List<Event>> FindAllEventsAsync()
        {
            return await _unitOfWork.EventRepository.GetAllEventsAsync();
        }

        public async Task<List<Event>> FindAllPastEventsAsync()
        {
            return await _unitOfWork.EventRepository.GetAllPastEventsAsync();
        }

        public async Task<List<Event>> FindAllUpcomingEventsAsync()
        {
            return await _unitOfWork.EventRepository.GetAllUpcomingEventsAsync();
        }

        public async Task<Event?> FindEventAsync(int eventId)
        {
            return await _unitOfWork.EventRepository.GetEventAsync(eventId);
        }

        public async Task<List<Event>> FindAllEventsWithTitleAsync(string title)
        {
            return await _unitOfWork.EventRepository.GetAllEventsWithTitleAsync(title);
        }

        public async Task<List<Event>> FindAllEventsWithPerformerAsync(string performer)
        {
            return await _unitOfWork.EventRepository.GetAllEventsWithPerformerAsync(performer);
        }

        public async Task<List<Event>> FindAllEventsAtVenueAsync(string venueName)
        {
            return await _unitOfWork.EventRepository.GetAllEventsAtVenueAsync(venueName);
        }

        public async Task<IEnumerable<EventReadOnlyDTO>> FindEventsByUserIdAsync(int userId)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<EventReadOnlyDTO>>(events);
        }

        public async Task<bool> IsEventSavedAsync(int userId, int eventId)
        {
            return await _unitOfWork.EventRepository.IsEventSavedAsync(userId, eventId);
        }
        public async Task<List<Event>> GetAllEventsFilteredAsync(int pageNumber, int pageSize, EventFiltersDTO eventFiltersDTO)
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

        public async Task<List<Event>> FindAllEventsOnDateAsync(DateOnly date)
        {
            return await _unitOfWork.EventRepository.GetAllEventsOnDateAsync(date);
        }
        public async Task<List<DateOnly?>> FindAllDatesWithEventsAsync()
        {
            return await _unitOfWork.EventRepository.GetAllDatesWithEventsAsync();
        }

        public async Task<List<Event>> FindAllSavedEventsAsync(int userId)
        {
            return await _unitOfWork.EventRepository.GetAllSavedEventsAsync(userId);
        }

        public async Task<Event?> UpdateEventAsync(int eventId, EventUpdateDTO updateDTO, IFormFile? eventImage)
        {
            Event? existingEvent;

            try
            {
                existingEvent = await _unitOfWork.EventRepository.GetEventAsync(eventId);

                existingEvent!.Title = updateDTO.Title;
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
                    var performerNamesInUpdate = updateDTO.Performers.Select(p => p.Name).ToList();
                    var performersToRemove = existingEvent.Performers!.Where(p => !performerNamesInUpdate.Contains(p.Name)).ToList();

                    foreach (var performerToRemove in performersToRemove)
                    {
                        existingEvent.Performers!.Remove(performerToRemove);
                    }

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
                if (eventImage != null)
                {
                    var _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(eventImage.FileName)}";
                    var imagePath = Path.Combine(_uploadFolderPath, imageFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await eventImage.CopyToAsync(stream);
                    }

                    existingEvent.ImageUrl = Path.Combine("Uploads", imageFileName);
                }

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

        public async Task<Event?> DeleteEventAsync(int eventId)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventAsync(eventId);
            if (eventEntity == null) return null;

            await _unitOfWork.EventRepository.DeleteAsync(eventId);
            await _unitOfWork.SaveAsync();

            return eventEntity;
        }

        public async Task<bool> SaveEventAsync(int userId, int eventId)
        {
            var existingSave = await _unitOfWork.EventRepository.GetSaveAsync(userId, eventId);
            if (existingSave != null)
            {
                return true;
            }
            var save = new EventSave { UserId = userId, EventId = eventId };
            await _unitOfWork.EventRepository.AddSaveAsync(save);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> UnsaveEventAsync(int userId, int eventId)
        {
            var save = await _unitOfWork.EventRepository.GetSaveAsync(userId, eventId);
            if (save != null)
            {
                _unitOfWork.EventRepository.RemoveSave(save);
                return await _unitOfWork.SaveAsync();
            }
            return true;
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
                UserId = insertDTO.UserId,
                ImageUrl = insertDTO.ImageUrl
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
