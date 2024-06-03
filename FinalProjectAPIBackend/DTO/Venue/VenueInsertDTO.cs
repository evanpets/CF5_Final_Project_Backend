using FinalProjectAPIBackend.Data;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Venue
{
    public class VenueInsertDTO
    {
        [StringLength(50, ErrorMessage = "Name should not exceed 50 characters.")]
        [Required(ErrorMessage = "Venue must be included.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Venue address must be included.")]
        public VenueAddressInsertDTO? VenueAddress { get; set; }
    }
}
