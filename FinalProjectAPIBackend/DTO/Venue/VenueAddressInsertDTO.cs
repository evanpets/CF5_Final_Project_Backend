using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Venue
{
    public class VenueAddressInsertDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Venue street should consist of at least 2 characters.")]
        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Street number is required.")]
        [RegularExpression(@"^(?=.*\d).{1,}$", ErrorMessage = "At least one number is required in the street number.")]
        public string? StreetNumber { get; set; }

        [StringLength(5, MinimumLength = 5, ErrorMessage = "Please check that the ZIP code consists of exactly five characters.")]
        [Required(ErrorMessage = "ZIP code is required.")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "A city is required.")]
        public string? City { get; set; }
    }
}
