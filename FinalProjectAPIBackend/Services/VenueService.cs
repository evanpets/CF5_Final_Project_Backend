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

        public async Task<Venue?> AddVenueAsync(VenueInsertDTO insertDTO)
        {
            Venue venue = new Venue
            {
                Name = insertDTO.Name,
                VenueAddress = insertDTO.VenueAddress != null ? _mapper.Map<VenueAddress>(insertDTO.VenueAddress) : null
            };

            await _unitOfWork.VenueRepository.AddAsync(venue);
            await _unitOfWork.SaveAsync();
            return venue;
        }

        public async Task<Venue?> DeleteVenueAsync(int venueId)
        {
            var venue = await _unitOfWork.VenueRepository.GetVenueAsync(venueId);

            if (venue is null) return null;

            await _unitOfWork.VenueRepository.DeleteVenueAsync(venueId);
            await _unitOfWork.SaveAsync();
            return venue;
        }

        public async Task<Venue?> FindVenueByNameAsync(string venueName)
        {
            return await _unitOfWork.VenueRepository.GetVenueByNameAsync(venueName);
        }

        public async Task<Venue?> FindVenueByIdAsync(int venueId)
        {
            return await _unitOfWork.VenueRepository.GetVenueAsync(venueId);
        }

        public async Task<List<Venue>> FindAllVenuesAsync()
        {
            return await _unitOfWork.VenueRepository.GetAllVenuesAsync();
        }

        public async Task<Venue?> UpdateVenueInfoAsync(int venueId, VenueUpdateDTO updateDTO)
        {
            Venue? existingVenue;

            try
            {
                existingVenue = await _unitOfWork.VenueRepository.GetVenueAsync(venueId);
                if (existingVenue == null) return null;

                existingVenue.Name = updateDTO.Name;

                if (updateDTO.VenueAddress != null)
                {
                    var venueAddress = _mapper.Map<VenueAddress>(updateDTO.VenueAddress);
                    existingVenue.VenueAddress!.VenueAddressId = venueAddress.VenueAddressId;
                    existingVenue.VenueAddress!.Street = venueAddress.Street;
                    existingVenue.VenueAddress!.StreetNumber = venueAddress.StreetNumber;
                    existingVenue.VenueAddress!.ZipCode = venueAddress.ZipCode;
                    existingVenue.VenueAddress!.City = venueAddress.City;
                }

                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Venue updated successfully");
            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "The venue was not found.");
                throw;
            }
            return existingVenue;
        }
    }
}
