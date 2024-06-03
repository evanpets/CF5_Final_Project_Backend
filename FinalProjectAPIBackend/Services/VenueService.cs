using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Venue;
using FinalProjectAPIBackend.Repositories;
using FinalProjectAPIBackend.Services.Exceptions;
using System;

namespace FinalProjectAPIBackend.Services
{
    public class VenueService : IVenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public VenueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Venue?> AddVenue(VenueInsertDTO insertDTO)
        {
            Venue venue = new Venue();

            venue.Name = insertDTO.Name;

            if (insertDTO.VenueAddress != null)
            {
                var venueAddress = _mapper.Map<VenueAddress>(insertDTO.VenueAddress);
                venue.VenueAddress = venueAddress;
            }

            await _unitOfWork.VenueRepository.AddAsync(venue);
            return venue;
        }

        public async Task<Venue?> DeleteVenue(int venueId)
        {
            var venue = await _unitOfWork.VenueRepository.GetVenueAsync(venueId);

            if (venue is null) return null;

            await _unitOfWork.VenueRepository.DeleteVenueAsync(venueId);
            return venue;
        }

        public async Task<Venue?> FindVenueByName(string name)
        {
            return await _unitOfWork.VenueRepository.GetVenueByNameAsync(name);
        }

        public async Task<List<Venue>> GetAllVenues()
        {
            return await _unitOfWork.VenueRepository.GetAllVenuesAsync();
        }

        public async Task<Venue?> UpdateVenueInfo(int venueId, VenueUpdateDTO updateDTO)
        {
            Venue? existingVenue;
            Venue? updatedVenue = null;

            try
            {
                existingVenue = await _unitOfWork.VenueRepository.GetVenueAsync(venueId);
                if (existingVenue is null) return null;

                existingVenue.Name = updateDTO.Name;

                if (updateDTO.VenueAddress is not null)
                {
                    var venueAddress = _mapper.Map<VenueAddress>(updateDTO.VenueAddress);
                    existingVenue.VenueAddress = venueAddress;
                }

                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Venue updated successfully");
            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "The venue was not found.");
                throw;
            }
            return updatedVenue;
        }
    }
}
