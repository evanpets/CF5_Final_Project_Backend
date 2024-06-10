using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.Models;
using FinalProjectAPIBackend.Repositories;
using FinalProjectAPIBackend.Security;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProjectAPIBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public UserService(IUnitOfWork? unitOfWork, IMapper? mapper, ILogger<UserService>? logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<User?> SignUpUserAsync(UserSignupDTO signupDTO)
        {
            User? user;

            try
            {
                user = ExtractUser(signupDTO);
                User? existingUser = await _unitOfWork!.UserRepository.GetByUsernameAsync(user.Username!);

                if (existingUser is not null) {
                    throw new UserAlreadyExistsException($"A user with the username {user.Username} already exists.");
                }

                user.Password = EncryptionUtil.Encrypt(user.Password!);
                await _unitOfWork!.UserRepository.AddAsync(user);

                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "User " + user.Username + "has signed up successfully.");
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return user;
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
            User? user;

            try
            {
                user = await _unitOfWork!.UserRepository.GetUserAsync(credentials.Username!, credentials.Password!);
                _logger!.LogInformation("{Message}", "User: " + user!.Username + " retrieved successfully.");

            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "A user with the credentials used was not found. Please make sure you have used the correct credentials.");
                throw;
            }
            return user;
        }

        public async Task<User?> GetUserById(int id)
        {
            User? user;
            try
            {
                user = await _unitOfWork!.UserRepository.GetAsync(id);
                _logger!.LogInformation("{Message}", "User with id: " + id + " fetching success");
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user;

            try
            {
                user = await _unitOfWork!.UserRepository.GetByUsernameAsync(username);
                _logger!.LogInformation("{Message}", "A user with the username: " + username + " was found successfully.");

            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "A user with the given username used was not found.");
                throw;
            }
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user;

            try
            {
                user = await _unitOfWork!.UserRepository.GetByEmailAsync(email);
                _logger!.LogInformation("{Message}", "A user with the email: " + email + " was found successfully.");

            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "A user with the given email was not found.");
                throw;
            }
            return user;
        }

        public async Task<List<User>> GetAllUsersFiltered(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO)
        {
            List<User> users = new();
            List<Func<User, bool>> filters = new();

            try
            {
                if (!string.IsNullOrEmpty(userFiltersDTO.Username))
                {
                    filters.Add(u => u.Username == userFiltersDTO.Username);
                }
                if (!string.IsNullOrEmpty(userFiltersDTO.Email))
                {
                    filters.Add(u => u.Email == userFiltersDTO.Email);
                }
                users = await _unitOfWork!.UserRepository.GetAllUsersFilteredAsync(pageNumber, pageSize,
                filters);
                _logger!.LogInformation("{Message}", "Success returning filtered users");
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return users;
        }

        public async Task<User?> UpdateUserAsync(int userId, UserUpdateDTO updateDTO)
        {
            User? existingUser;

            try
            {
                existingUser = await _unitOfWork!.UserRepository.GetAsync(userId);
                if (existingUser is null) return null;

                existingUser.Username = updateDTO.Username;
                existingUser.Password = EncryptionUtil.Encrypt(updateDTO.Password!);
                existingUser.Email = updateDTO.Email;
                existingUser.FirstName = updateDTO.FirstName;
                existingUser.LastName = updateDTO.LastName;
                existingUser.PhoneNumber = updateDTO.PhoneNumber;
                existingUser.Role = UserRole.User;

                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "User: " + existingUser + " updated successfully");
            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "The user was not found.");
                throw;
            }
            return existingUser;
        }

        public string CreateUserToken(int userId, string? username, string? email, string? appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username!),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email!),
                //new Claim(ClaimTypes.Role, UserRole.User.ToString())
            };

            var issuer = "https://localhost:5001";
            var audience = "https://localhost:4200";

            var jwtSecurityToken = new JwtSecurityToken(issuer, audience, claimsInfo, DateTime.UtcNow, DateTime.UtcNow.AddHours(3), signingCredentials);
            
            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken;
        }

        public async Task DeleteUserAsync(int id)
        {
            bool deleted;
            try
            {
                deleted = await _unitOfWork!.UserRepository.DeleteAsync(id);
                if (!deleted)
                {
                    throw new UserNotFoundException("UserNotFound");
                }
            }
            catch (UserNotFoundException)
            {
                _logger!.LogError("{Message}", "A user to be deleted was not found.");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersFilteredAsync(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO)
        {
            List<User> users = new();
            List<Func<User, bool>> filters = new();

            try
            {
                if (!string.IsNullOrEmpty(userFiltersDTO.Username))
                {
                    filters.Add(u => u.Username == userFiltersDTO.Username);
                }
                if (!string.IsNullOrEmpty(userFiltersDTO.Email))
                {
                    filters.Add(u => u.Email == userFiltersDTO.Email);
                }
                users = await _unitOfWork!.UserRepository.GetAllUsersFilteredAsync(pageNumber, pageSize,
                    filters);
                _logger!.LogInformation("{Message}", "Success in returning filtered users.");
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return users;
        }

        private User ExtractUser(UserSignupDTO signupDTO)
        {
            return new User()
            {
                Username = signupDTO.Username,
                Password = signupDTO.Password,
                Email = signupDTO.Email,
                FirstName = signupDTO.FirstName,
                LastName = signupDTO.LastName,
                PhoneNumber = signupDTO.PhoneNumber,
                Role = UserRole.User
            };
        }

    }
}
