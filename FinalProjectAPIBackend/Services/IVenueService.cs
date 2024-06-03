using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Venue;

namespace FinalProjectAPIBackend.Services
{
    public interface IVenueService
    {
        Task<Venue?> AddVenue(VenueInsertDTO insertDTO);
        Task<List<Venue>> GetAllVenues();
        Task<Venue?> FindVenueByName(string name);
        Task<Venue?> UpdateVenueInfo(int venueId, VenueUpdateDTO updateDTO);
        Task<Venue?> DeleteVenue(int venueId);
    }
}
