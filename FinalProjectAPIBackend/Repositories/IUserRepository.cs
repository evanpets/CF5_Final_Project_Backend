using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> UpdateUserAsync(int userId, User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<List<User>> GetAllUsersFilteredAsync(int pageNumber, int pageSize, List<Func<User, bool>> predicates);
    }
}
