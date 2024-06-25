namespace FinalProjectAPIBackend.Repositories
{
    /// <summary>
    /// A base repository to be inherited by other repositories.
    /// </summary>
    /// <typeparam name="T">A class type to be assigned by inheriting classes.</typeparam>
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Adds an instance of <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="T"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds a range of instances of <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="entities">A collection of instances of <typeparamref name="T"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Retrieves an instance of <typeparamref name="T"/> from the database by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the instance to retrieve.</param>
        /// <returns>The instance of <typeparamref name="T"/>, or null if not found.</returns>
        Task<T?> GetAsync(int id);

        /// <summary>
        /// Retrieves all instances of <typeparamref name="T"/> from the database.
        /// </summary>
        /// <returns>A collection of all instances of <typeparamref name="T"/>.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets the count of instances of <typeparamref name="T"/> in the database.
        /// </summary>
        /// <returns>The count of instances.</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Updates an instance of <typeparamref name="T"/> in the database.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="T"/>.</param>
        void UpdateAsync(T entity);

        /// <summary>
        /// Deletes an instance of <typeparamref name="T"/> from the database by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the instance to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
