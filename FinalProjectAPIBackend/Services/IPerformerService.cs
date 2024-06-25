using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Services
{
    /// <summary>
    /// Interface for the performer service.
    /// </summary>
    public interface IPerformerService
    {
        /// <summary>
        /// Finds a performer by name.
        /// </summary>
        /// <param name="name">The name of the performer to find.</param>
        /// <returns>The performer with the specified name, or null if no such performer exists.</returns>
        Task<Performer?> FindPerformerAsync(string name);

        /// <summary>
        /// Finds all performers.
        /// </summary>
        /// <returns>A list of all performers.</returns>
        Task<List<Performer>> FindAllPerformersAsync();

        /// <summary>
        /// Finds all performers with a specific name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of performers with the specified name.</returns>
        Task<List<Performer>> FindAllPerformersWithNameAsync(string name);
    }
}
