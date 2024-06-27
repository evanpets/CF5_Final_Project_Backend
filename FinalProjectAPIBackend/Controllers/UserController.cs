using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.Services;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProjectAPIBackend.Controllers
{
    /// <summary>
    /// A controller for methods related to users.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        /// <summary>
        /// Injects the controller with the services needed.
        /// </summary>
        /// <param name="applicationService">The service layer.</param>
        /// <param name="mapper">Allows mapping entities to DTOs.</param>
        /// <param name="configuration">Provides information related to authenticating a user.</param>
        public UserController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a user in the database.
        /// </summary>
        /// <param name="userSignupDTO">The DTO containing the information of the user to be registered.</param>
        /// <returns>A success message.</returns>
        /// <exception cref="InvalidRegistrationException">A problem that occurs during the user registration.</exception>
        /// <exception cref="ServerGenericException">A problem in accessing the server's services.</exception>
        /// <exception cref="UserAlreadyExistsException">If a user with the requested username already exists.</exception>
        [Route("registration")]
        [HttpPost]
        public async Task<ActionResult<UserReadOnlyDTO>> SignupUserAsync(UserSignupDTO? userSignupDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                throw new InvalidRegistrationException("Error In Registration: " + errors);
            }

            if (_applicationService == null)
            {
                throw new ServerGenericException("Application Service Null");
            }
            User? user = await _applicationService.UserService.FindUserByUsernameAsync(userSignupDTO!.Username!);

            if (user is not null)
            {
                throw new UserAlreadyExistsException("User " + user.Username + " already exists.");
            }
            User? returnedUser = await _applicationService.UserService.SignUpUserAsync(userSignupDTO);

            if (returnedUser is null)
            {
                throw new InvalidRegistrationException("Invalid Registration");
            }
            var returnedUserDTO = _mapper.Map<UserReadOnlyDTO>(returnedUser);
            return Created("api/users/" + returnedUserDTO.UserId, returnedUserDTO);
        }

        /// <summary>
        /// Authenticates a user who has requested login.
        /// </summary>
        /// <param name="credentials">The credentials of the user to be authenticated.</param>
        /// <returns>A JWT granting the user access to their account.</returns>
        /// <exception cref="UnauthorizedAccessException">If the user's credentials are invalid.</exception>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<JwtTokenDTO>> LoginUserAsync(UserLoginDTO credentials)
        {

            var user = await _applicationService.UserService.VerifyAndGetUserAsync(credentials);

            if (user == null)
            {
                return BadRequest("Bad Credentials");
                //throw new UnauthorizedAccessException("Bad Credentials");
            }

            var userToken =
                _applicationService.UserService.
                CreateUserToken(user.UserId, user.Username!, user.Email!, user.Role, _configuration["Authentication:SecretKey"]!);

            JwtTokenDTO token = new()
            {
                Token = userToken
            };

            Console.WriteLine("token " + token);
            //Console.WriteLine($"AppUser:{AppUser.Id}, {AppUser.Username}, {AppUser.Email}");
            return Ok(token);
        }

        /// <summary>
        /// Fetches a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to be fetched.</param>
        /// <returns>The user with the requested ID.</returns>
        /// <exception cref="UserNotFoundException">If the user with the requested ID doesn't exist.</exception>
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById(int userId)
        {
            var user = await _applicationService.UserService.FindUserByIdAsync(userId);
            if (user is null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var returnedUser = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedUser);
        }

        /// <summary>
        /// Fetches a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to be fetched.</param>
        /// <returns>The user with the requested username.</returns>
        /// <exception cref="UserNotFoundException">If the user with the requested username doesn't exist.</exception>
        [HttpGet("by-username")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserByUsername([FromQuery] string username)
        {
            var user = await _applicationService.UserService.FindUserByUsernameAsync(username);

            if (user is null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            var returnedUser = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedUser);
        }

        /// <summary>
        /// Fetches a list of all the existing users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <exception cref="UserNotFoundException">If no users are found.</exception>
        [HttpGet]
        public async Task<ActionResult<List<UserReadOnlyDTO>>> GetAllUsers()
        {
            var usersList = await _applicationService.UserService.FindAllUsersAsync();
            if (usersList is null)
            {
                throw new UserNotFoundException("No users found");
            }
            return Ok(usersList);
        }

        /// <summary>
        /// Fetches the email requested and validates it as unique for registration or rejects it.
        /// </summary>
        /// <param name="email">The email input by the user.</param>
        /// <returns>A confirmation of the check and a message pertaining to success or failure of the request.</returns>
        [HttpGet("duplicate-email")]
        public async Task<IActionResult> CheckDuplicateEmail(string email)
        {
            try
            {
                var existingEmail = await _applicationService.UserService.FindUserByEmailAsync(email);
                if (existingEmail is not null)
                {
                    return Ok(new { msg = "Email already in use" });
                }
                return Ok(new { msg = "Email not registered yet" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in finding email");
                Console.WriteLine(ex);
                return BadRequest(new { msg = ex.Message });
            }
        }

        /// <summary>
        /// Fetches the username requested and validates it as unique for registration or rejects it.
        /// </summary>
        /// <param name="username">The username input by the user.</param>
        /// <returns>A confirmation of the check and a message pertaining to success or failure of the request.</returns>
        [HttpGet("duplicate-username")]
        public async Task<IActionResult> CheckDuplicateUsername(string username)
        {
            try
            {
                var existingUsername = await _applicationService.UserService.FindUserByUsernameAsync(username);
                if (existingUsername is not null)
                {
                    return Ok(new { msg = "Username already taken" });
                }
                return Ok(new { msg = "Username available" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in finding username");
                Console.WriteLine(ex);
                return BadRequest(new { msg = ex.Message });
            }
        }

        /// <summary>
        /// Allows a user to update their account information.
        /// </summary>
        /// <param name="userId">The ID of the user to be authenticated.</param>
        /// <param name="patchDTO">The DTO containing the updated information.</param>
        /// <returns>A success message and the updated user.</returns>
        /// <exception cref="ForbiddenException">Flags an attempt to access a different user as unauthorized.</exception>
        [HttpPatch("{userId}")]
        public async Task<ActionResult<UserReadOnlyDTO>> UpdateUserPatch(int userId, UserUpdateDTO patchDTO)
        {
            var appUserId = AppUser!.Id;
            if (userId != appUserId)
            {
                throw new ForbiddenException("Forbidden Access");
            }

            var user = await _applicationService.UserService.UpdateUserAsync(userId, patchDTO);
            Console.WriteLine("Updated user in patch" + user);
            if (user is null)
            {
                return BadRequest(new { msg = "User update failed" });
            }
            var userDTO = _mapper.Map<UserReadOnlyDTO>(user);
            Console.WriteLine("User dto in controller: " + userDTO);
            return Ok(new { msg = "User updated successfully", user = userDTO });
        }
    }
}


