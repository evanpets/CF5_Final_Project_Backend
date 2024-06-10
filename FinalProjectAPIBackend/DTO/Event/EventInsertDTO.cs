using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.DTO.Venue;
using FinalProjectAPIBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Event
{
    public class EventInsertDTO
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Event title should consist of at least 5 characters.")]
        [Required(ErrorMessage = "The event title is a required field.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Description should not exceed 250 characters.")]
        public string? Description { get; set; }

        public int? VenueId { get; set; }
        public VenueInsertDTO? NewVenue { get; set; }

        public decimal? Price { get; set; }

        public ICollection<int>? PerformerIds { get; set; }
        public ICollection<PerformerInsertDTO>? NewPerformers { get; set; }

        [Required(ErrorMessage = "A valid date must be selected.")]
        public DateOnly? Date { get; set; }

        [Required(ErrorMessage = "An event category must be selected.")]
        public EventCategory? Category { get; set; }

        public int UserId {  get; set; }

        public override string? ToString()
        {
            return "InsertDTO: Title: " + Title
                + "\nDescription: " + Description
                + "\n,Venue Id: " + VenueId
                + "\nVenue Insert DTO:\nName" + NewVenue!.Name +
                ", VenueAddress:\ni)Street" + NewVenue.VenueAddress!.Street +
                ",\nii) Street number" + NewVenue.VenueAddress!.StreetNumber +
                ",\niii)ZIP Code: " + NewVenue.VenueAddress!.ZipCode +
                ",\niv)City: " + NewVenue.VenueAddress!.City +
                ",\nDate" + Date +
                ",\nCategory: " + Category;
        }
    }
}
