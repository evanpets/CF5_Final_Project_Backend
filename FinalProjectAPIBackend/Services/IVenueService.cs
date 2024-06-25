using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Venue;

namespace FinalProjectAPIBackend.Services
{
    /// <summary>
    /// Interface for the venue service.
    /// </summary>
    public interface IVenueService
    {
        /// <summary>
        /// Adds a new venue.
        /// </summary>
        /// <param name="insertDTO">The venue information to add.</param>
        /// <returns>The added venue, or null if the operation failed.</returns>
        Task<Venue?> AddVenueAsync(VenueInsertDTO insertDTO);

        /// <summary>
        /// Finds all venues.
        /// </summary>
        /// <returns>A list of all venues.</returns>
        Task<List<Venue>> FindAllVenuesAsync();

        /// <summary>
        /// Finds a venue by name.
        /// </summary>
        /// <param name="name">The name of the venue to find.</param>
        /// <returns>The venue with the specified name, or null if no such venue exists.</returns>
        Task<Venue?> FindVenueByNameAsync(string name);

        /// <summary>
        /// Updates the information of a venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue to update.</param>
        /// <param name="updateDTO">The updated venue information.</param>
        /// <returns>The updated venue, or null if the update failed.</returns>
        Task<Venue?> UpdateVenueInfoAsync(int venueId, VenueUpdateDTO updateDTO);

        /// <summary>
        /// Deletes a venue by ID.
        /// </summary>
        /// <param name="venueId">The ID of the venue to delete.</param>
        /// <returns>The deleted venue, or null if the operation failed.</returns>
        Task<Venue?> DeleteVenueAsync(int venueId);
    }
}