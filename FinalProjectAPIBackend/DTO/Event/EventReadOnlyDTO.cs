using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Event
{
    public class EventReadOnlyDTO
    {
        [Required]
        public int EventId { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Event title should consist of at least 5 characters.")]
        [Required(ErrorMessage = "The event title is a required field.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Description should not exceed 250 characters.")]
        public string? Description { get; set; }

        public string? VenueName { get; set; }
        public string? VenueStreet { get; set; }
        public string? VenueStreetNumber { get; set; }
        public string? VenueZipCode { get; set; }
        public string? VenueCity { get; set; }
        public decimal? Price { get; set; }
        public EventCategory Category { get; set; }

        public ICollection<PerformerReadOnlyDTO>? Performers { get; set; }

        [Required(ErrorMessage = "A valid date must be selected.")]
        public DateOnly? Date { get; set; }

        //[Required(ErrorMessage = "An event category must be selected.")]
        //public EventCategory? Category { get; set; }

        public int? UserId { get; set; }


        public override string? ToString()
        {
            return "ReadOnlyDTO:\nTitle: " + Title
            + ",\nDescription: " + Description
            + "\nVenue Name: " + VenueName 
            + "\nVenue Street: " + VenueStreet 
            +"\nVenue Zip Code: " + VenueZipCode 
            +"\nVenue City: " + VenueCity 
            +"\nPrice: " + Price 
            +"\nPerformers" + Performers!.ToString()
            +"\nDate" + Date 
            +"\nCategory: " + Category 
            +"\nUserId: " + UserId + "\n";
        }
    }
}
