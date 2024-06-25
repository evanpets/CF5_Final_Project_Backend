using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    /// <summary>
    /// Interface for the venue repository.
    /// </summary>
    public interface IVenueRepository
    {
        /// <summary>
        /// Gets a venue by ID.
        /// </summary>
        /// <param name="venueId">The ID of the venue to get.</param>
        /// <returns>The venue if found, otherwise null.</returns>
        Task<Venue?> GetVenueAsync(int venueId);

        /// <summary>
        /// Gets all venues.
        /// </summary>
        /// <returns>A list of all venues.</returns>
        Task<List<Venue>> GetAllVenuesAsync();

        /// <summary>
        /// Updates a venue's information.
        /// </summary>
        /// <param name="venue">The updated venue.</param>
        Task<Venue?> UpdateVenueInformationAsync(Venue venue);
        /// <summary>
        /// Deletes a venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteVenueAsync(int venueId);

    }
}
