using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.Services;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectAPIBackend.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [Route("register")]
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
            User? user = await _applicationService.UserService.GetUserByUsernameAsync(userSignupDTO!.Username!);

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
            return CreatedAtAction(nameof(GetUserById), new { id = returnedUserDTO.UserId }, returnedUserDTO);
        }

        [HttpGet("get/id/{id}")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById(int id)
        {
            var user = await _applicationService.UserService.GetUserById(id);
            if (user is null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            var returnedUser = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedUser);
        }

        [HttpGet("get/username")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserByUsername(string username)
        {
            var user = await _applicationService.UserService.GetUserByUsernameAsync(username);
            if (user is null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            var returnedUser = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedUser);
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<JwtTokenDTO>> LoginUserAsync(UserLoginDTO credentials)
        {
            var user = await _applicationService.UserService.VerifyAndGetUserAsync(credentials);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Bad Credentials");
            }

            //Console.WriteLine(user.UserId! + user.Username! + user.Email!);
            var userToken =
                _applicationService.UserService.
                CreateUserToken(user.UserId, user.Username!, user.Email!, _configuration["Authentication:SecretKey"]!);

            JwtTokenDTO token = new()
            {
                Token = userToken
            };
            //Console.WriteLine(token.Token);

            return Ok(token);
        }

        [HttpPatch("update/{userId}")]
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

        //[HttpPut("update-account/{id}")]
        //public async Task<ActionResult<UserDTO>> UpdateUserAccount(int id, UserUpdateDTO? userDTO)
        //{
        //    var userId = AppUser!.Id;
        //    if (id != userId)
        //    {
        //        throw new ForbiddenException("Forbidden Access");
        //    }
        //    var user = await _applicationService.UserService.UpdateUserAsync(userId, userDTO!);

        //    var returnedUserDTO = _mapper.Map<UserDTO>(user);
        //    return Ok(returnedUserDTO);
        //}

        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = AppUser!.Id;
            if (id != userId)
            {
                throw new ForbiddenException("Forbidden Access");
            }

            await _applicationService.UserService.DeleteUserAsync(userId);
            return NoContent();
        }

        [HttpGet("check-duplicate-email")]
        public async Task<IActionResult> CheckDuplicateEmail(string email)
        {
            try
            {
                var existingEmail = await _applicationService.UserService.GetUserByEmailAsync(email);
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

        [HttpGet("check-duplicate-username")]
        public async Task<IActionResult> CheckDuplicateUsername(string username)
        {
            try
            {
                var existingUsername = await _applicationService.UserService.GetUserByUsernameAsync(username);
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
    }
}


