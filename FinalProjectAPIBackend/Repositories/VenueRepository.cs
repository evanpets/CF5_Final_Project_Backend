using FinalProjectAPIBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectAPIBackend.Repositories
{
    public class VenueRepository : BaseRepository<Venue>, IVenueRepository
    {
        public VenueRepository(FinalProjectAPIBackendDbContext context) : base(context)
        {
        }


        public async Task<Venue?> GetVenueAsync(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return null;
            return venue;
        }

        public async Task<Venue?> GetVenueByNameAsync(string name)
        {
            var venue = await _context.Venues.Where(v => v.Name == name).FirstOrDefaultAsync();
            if (venue == null) return null;
            return venue;
        }

        public async Task<List<Venue>> GetAllVenuesAsync()
        {
            var venues = await _context.Venues.ToListAsync();
            if (venues is null) return new List<Venue>();
            return venues!;
        }

        public async Task<Venue?> UpdateVenueInformationAsync(Venue updatedVenue)
        {
            var existingVenue = await _context.Venues.FindAsync(updatedVenue.VenueId);
            if (existingVenue is null) return null;

            _context.Entry(existingVenue).CurrentValues.SetValues(updatedVenue);
            return updatedVenue;
        }
        public async Task<bool> DeleteVenueAsync(int id)
        {
            var venueToDelete = await _context.Venues.FindAsync(id);
            if (venueToDelete is not null)
            {
                _context.Remove(venueToDelete);
                return true;
            }
            return false;
        }
    }
}
