using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    public interface IVenueRepository
    {
        Task<Venue?> GetVenueAsync(int id);
        Task<List<Venue>> GetAllVenuesAsync();
        Task<Venue?> UpdateVenueInformationAsync(Venue venue);
        Task<bool> DeleteVenueAsync(int id);

    }
}
