using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.Security;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectAPIBackend.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(FinalProjectAPIBackendDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == username);
            if (user is null)
            {
                return null;
            }
            if (!EncryptionUtil.IsValidPassword(password, user.Password!))
            {
                return null;
            }
            return user;
        }


        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            var ExistingUser = await _context.Users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            return ExistingUser;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var ExistingUser = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            return ExistingUser;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var ExistingUser = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return ExistingUser;
        }

        public async Task<List<User>> GetAllUsersFilteredAsync(int pageNumber, int pageSize, List<Func<User, bool>> predicates)
        {
            int skip = pageSize * pageNumber;
            IQueryable<User> query = _context.Users.Skip(skip).Take(pageSize);

            if (predicates != null && predicates.Any())
            {
                query = query.Where(u => predicates.All(predicate => predicate(u)));
            }

            return await query.ToListAsync();
        }

        public async Task<User?> UpdateUserAsync(int userId, User user)
        {
            var ExistingUser = await _context.Users.Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
            if (ExistingUser is null) return null;
            if (ExistingUser.UserId != userId) return null;

            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;

            return ExistingUser;
        }
    }
}
