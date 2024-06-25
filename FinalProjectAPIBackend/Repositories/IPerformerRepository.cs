using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    /// <summary>
    /// Interface for the performer repository.
    /// </summary>
    public interface IPerformerRepository
    {
        /// <summary>
        /// Gets a performer by ID.
        /// </summary>
        /// <param name="performerId">The ID of the performer to retrieve.</param>
        /// <returns>The performer with the specified ID, or null if no such performer exists.</returns>
        Task<Performer?> GetPerformerAsync(int performerId);
        /// <summary>
        /// Gets a performer by name.
        /// </summary>
        /// <param name="name">The name of the performer to retrieve.</param>
        /// <returns>The performer with the specified name, or null if no such performer exists.</returns>
        Task<Performer?> GetPerformerByNameAsync(string name);

        /// <summary>
        /// Fetches a number of performers based on their IDs.
        /// </summary>
        /// <param name="performerIds">The list of IDs of the performers.</param>
        /// <returns>The list of performers.</returns>
        Task<List<Performer>> GetPerformersRangeAsync(ICollection<int> performerIds);

        /// <summary>
        /// Gets all performers.
        /// </summary>
        /// <returns>A list of all performers.</returns>
        Task<List<Performer>> GetAllPerformersAsync();

        /// <summary>
        /// Gets all performers with a specific name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of performers with the specified name.</returns>
        Task<List<Performer>> GetAllPerformersWithNameAsync(string name);

        /// <summary>
        /// Updates the information of a performer.
        /// </summary>
        /// <param name="performer">The updated performer information.</param>
        /// <returns>The updated performer, or null if the update failed.</returns>
        Task<Performer?> UpdatePerformerInformationAsync(Performer performer);
    }
}
