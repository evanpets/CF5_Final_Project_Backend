using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Services
{
        /// <summary>
    /// Interface for the user service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Signs up a new user.
        /// </summary>
        /// <param name="request">The user information to sign up.</param>
        /// <returns>The signed-up user, or null if the operation failed.</returns>
        Task<User?> SignUpUserAsync(UserSignupDTO request);

        /// <summary>
        /// Verifies a user's credentials and returns the user.
        /// </summary>
        /// <param name="credentials">The user's login credentials.</param>
        /// <returns>The verified user, or null if the credentials are invalid.</returns>
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);

        /// <summary>
        /// Updates a user's information.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="request">The updated user information.</param>
        /// <returns>The updated user, or null if the update failed.</returns>
        Task<User?> UpdateUserAsync(int userId, UserUpdateDTO request);

        /// <summary>
        /// Finds a user by username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user with the specified username, or null if no such user exists.</returns>
        Task<User?> FindUserByUsernameAsync(string username);

        /// <summary>
        /// Finds a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to find.</param>
        /// <returns>The user with the specified ID, or null if no such user exists.</returns>
        Task<User?> FindUserByIdAsync(int userId);

        /// <summary>
        /// Finds all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        Task<List<User?>> FindAllUsersAsync();

        /// <summary>
        /// Finds all users filtered by pagination and other parameters.
        /// </summary>
        /// <param name="pageNumber">The page number to start from.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <param name="userFiltersDTO">The filters to apply.</param>
        /// <returns>A list of users filtered according to the parameters.</returns>
        Task<List<User>> GetAllUsersFilteredAsync(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO);

        /// <summary>
        /// Creates a JWT token for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="role">The role of the user.</param> (Note: The role is optional)
        /// <param name="appSecurityKey">The application security key.</param>
        /// <returns>The created JWT token for the specified user.</returns>
        string CreateUserToken(int userId, string? userName, string? email, UserRole? role,
            string? appSecurityKey);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation..</returns>
        Task DeleteUserAsync(int userId);
    }
}
