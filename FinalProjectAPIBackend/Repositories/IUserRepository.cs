using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    /// <summary>
    /// Interface for the user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user by username and password.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <param name="password">The password to search for.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User?> GetUserAsync(string username, string password);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        Task<List<User?>> GetAllUsersAsync();

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="user">The updated user.</param>
        /// <returns>The updated user.</returns>
        Task<User?> UpdateUserAsync(int userId, User user);

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Gets a user by phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to search for.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// Gets all users filtered.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="predicates">The predicates to filter by.</param>
        /// <returns>A list of filtered users.</returns>
        Task<List<User>> GetAllUsersFilteredAsync(int pageNumber, int pageSize, List<Func<User, bool>> predicates);
    }
}
