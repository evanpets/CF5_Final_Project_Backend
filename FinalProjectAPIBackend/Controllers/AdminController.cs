using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.DTO.Venue;
using FinalProjectAPIBackend.Services;
using FinalProjectAPIBackend.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectAPIBackend.Controllers
{
    /// <summary>
    /// A controller for methods exclusive to admins.
    /// </summary>
    [ApiController]
    [Route("api/admin")]
    public class AdminController : BaseController
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Injects the controller with the services needed.
        /// </summary>
        /// <param name="applicationService">The service layer.</param>
        /// <param name="mapper">Allows mapping entities to DTOs.</param>
        public AdminController(IApplicationService applicationService, IMapper mapper) : base(applicationService)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Allows an admin to update a user.
        /// </summary>
        /// <param name="userId">The ID of the user to be updated.</param>
        /// <param name="patchDTO">The DTO containing the information for the update.</param>
        /// <returns>A 200 code with a success message and a new user.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("users/{userId}")]
        public async Task<ActionResult<UserReadOnlyDTO>> AdminUpdateUser(int userId, UserUpdateDTO patchDTO)
        {
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

        /// <summary>
        /// Allows an admin to delete a user.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        /// <returns>A success message.</returns>
        [HttpDelete("users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDeleteUser(int id)
        {
            await _applicationService.UserService.DeleteUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Allows an admin to insert a venue.
        /// </summary>
        /// <param name="insertDTO">The DTO with the information of the new venue.</param>
        /// <returns>The new venue.</returns>
        /// <exception cref="InvalidCreationException">A problem that occurs during the venue's creation.</exception>
        /// <exception cref="ServerGenericException">A problem in accessing the server's service.</exception>
        /// <exception cref="VenueAlreadyExistsException">If the venue already exists in the database.</exception>
        [Authorize(Roles = "Admin")]
        [HttpPost("venues/new")]
        public async Task<ActionResult<VenueReadOnlyDTO>> AdminInsertVenue(VenueInsertDTO insertDTO)
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

                throw new InvalidCreationException("Error In Venue Creation: " + errors);
            }

            if (_applicationService == null)
            {
                throw new ServerGenericException("Application Service Null");
            }

            Venue? createdVenue = await _applicationService.VenueService.AddVenueAsync(insertDTO);

            if (createdVenue is null)
            {
                throw new InvalidCreationException("Problem during creation");
            }

            var newVenueReadOnlyDTO = _mapper.Map<VenueReadOnlyDTO>(createdVenue);
            Console.WriteLine("New Venue:" + _applicationService.VenueService.FindVenueByIdAsync(newVenueReadOnlyDTO.VenueId) + "Read DTO:" + newVenueReadOnlyDTO.ToString());
            //return Created($"api/events/venues/{newVenueReadOnlyDTO.VenueId}", newVenueReadOnlyDTO);

            return Created("api/events/venues/" + newVenueReadOnlyDTO.VenueId, newVenueReadOnlyDTO);
        }

        /// <summary>
        /// Allows an admin to update a venue's information.
        /// </summary>
        /// <param name="venueId">The ID of the venue to be updated.</param>
        /// <param name="updateDTO">The DTO containing the updated information.</param>
        /// <returns>The updated venue.</returns>
        /// <exception cref="VenueNotFoundException">If the event to be updated doesn't exist.</exception>
        /// <exception cref="InvalidCreationException">A problem that occurs during the venue's creation.</exception>
        [Authorize(Roles = "Admin")]
        [HttpPatch("venues/{venueId}")]
        public async Task<ActionResult<VenueReadOnlyDTO>> AdminUpdateVenue(int venueId, VenueUpdateDTO updateDTO)
        {
            try
            {
                var existingVenue = await _applicationService.VenueService.FindVenueByIdAsync(venueId);
                if (existingVenue is null)
                {
                    throw new VenueNotFoundException("Venue not found.");
                }

                var updatedVenue = await _applicationService.VenueService.UpdateVenueInfoAsync(venueId, updateDTO);
                if (updatedVenue == null)
                {
                    throw new InvalidCreationException("Problem during creation");
                }
                var returnedVenueDTO = _mapper.Map<VenueReadOnlyDTO>(updatedVenue);

                return Ok(new { msg = "Event updated successfully", venue = returnedVenueDTO });
            }
            catch (ServerGenericException ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the venue", message = ex.Message });
            }
        }

        /// <summary>
        /// Allows an admin to delete a venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue to be updated.</param>
        /// <returns>A success message with an empty payload.</returns>
        /// <exception cref="VenueNotFoundException">If the event to be deleted doesn't exist.</exception>
        [Authorize(Roles = "Admin")]
        [HttpDelete("venues/{venueId}")]
        public async Task<ActionResult> AdminDeleteVenue(int venueId)
        {
            var existingVenue = await _applicationService.VenueService.FindVenueByIdAsync(venueId);
            if (existingVenue is null)
            {
                throw new VenueNotFoundException("Venue not found.");
            }

            var deletedEvent = await _applicationService.VenueService.DeleteVenueAsync(venueId);
            if (deletedEvent == null)
            {
                return NotFound(new { msg = "Venue not found" });
            }
            return NoContent();
        }
    }
}
