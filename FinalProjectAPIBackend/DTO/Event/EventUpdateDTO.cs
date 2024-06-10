using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Event
{
    public class EventUpdateDTO
    {
        [Required]
        [Editable(false)]
        public int EventId { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Event title should consist of at least 5 characters.")]
        [Required(ErrorMessage = "The event title is a required field.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Description should not exceed 250 characters.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "A valid date must be selected.")]
        public DateOnly? Date { get; set; }
        public string? VenueName { get; set; }
        public string? VenueStreet { get; set; }
        public string? VenueStreetNumber { get; set; }
        public string? VenueZipCode { get; set; }
        public string? VenueCity { get; set; }
        public decimal? Price { get; set; }
        public ICollection<PerformerUpdateDTO>? Performers { get; set; }
        //public ICollection<int>? PerformerIds { get; set; }
        [Required(ErrorMessage = "An event category must be selected.")]
        public EventCategory? Category { get; set; }
        public int? UserId { get; set; }
    }
}
