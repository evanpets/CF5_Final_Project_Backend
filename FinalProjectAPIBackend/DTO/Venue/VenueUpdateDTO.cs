using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Venue
{
    public class VenueUpdateDTO
    {
        [Required]
        [Editable(false)]
        public int VenueId { get; set; }

        [StringLength(50, ErrorMessage = "Name should not exceed 50 characters.")]
        [Required(ErrorMessage = "Venue must be included.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Venue address must be included.")]
        public VenueAddressUpdateDTO? VenueAddress { get; set; }
    }
}
