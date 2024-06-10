using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Services
{
    public interface IUserService
    {
        Task<User?> SignUpUserAsync(UserSignupDTO request);
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<User?> UpdateUserAsync(int userId, UserUpdateDTO request);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserById(int userId);
        Task<List<User>> GetAllUsersFilteredAsync(int pageNumber, int pageSize,
            UserFiltersDTO userFiltersDTO);
        string CreateUserToken(int userId, string? userName, string? email,
            string? appSecurityKey);
        Task DeleteUserAsync(int userId);

    }
}
